using Offer.API.Offer.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offer.CommonDefinitions.Records;
using System.IO;

namespace Offer.BL.Services.Managers
{
    public class OfferServiceManager
    {
        private const string OfferPath = "{0}/ContentFiles/Offer/{1}";

        public static API.Offer.DAL.DB.Offer AddOrEditOffer(string baseUrl/*, long createdBy*/, OfferRecord record, API.Offer.DAL.DB.Offer oldOffer = null)
        {
            if (oldOffer == null)//new offer
            {
                oldOffer = new API.Offer.DAL.DB.Offer();
                oldOffer.Creationdate = DateTime.Now;
                oldOffer.CreatedBy = record.CreatedBy;
            }
            else
            {
                oldOffer.Modificationdate = DateTime.Now;
                oldOffer.ModifiedBy = record.ModifiedBy;
            }
            if (!string.IsNullOrWhiteSpace(record.Name))
            {
                oldOffer.Name = record.Name;
            }
            if (!string.IsNullOrWhiteSpace(record.Description))
            {
                oldOffer.Description = record.Description;
            }
            if (record.Validfrom != null)
            {
                oldOffer.Validfrom = record.Validfrom;
            }
            if (record.Validto != null)
            {
                oldOffer.Validto = record.Validto;
            }
            if (!string.IsNullOrWhiteSpace(record.Discount))
            {
                oldOffer.Discount = record.Discount;
            }
            if (record.Status != null)
            {
                oldOffer.Status = record.Status;
            }
            if (!string.IsNullOrWhiteSpace(record.Purpose))
            {
                oldOffer.Purpose = record.Purpose;
            }
            if (record.LanguageId != null)
            {
                oldOffer.LanguageId = record.LanguageId;
            }
            if (record.Maxusagecount != null)
            {
                oldOffer.Maxusagecount = record.Maxusagecount;
            }
            if (record.AddUse != null && record.AddUse == true)
            {
                oldOffer.Usedcount = oldOffer.Usedcount + 1;
            }
            //    Imageurl = c.Imageurl
            //upload
            if (record.FormImage != null)
            {
                var allowedExtensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };
                var extension = Path.GetExtension(record.FormImage.FileName);
                if (allowedExtensions.Contains(extension))
                {
                    var file = record.FormImage.OpenReadStream();
                    var fileName = record.FormImage.FileName;
                    if (file.Length > 0)
                    {
                        var newFileName = Guid.NewGuid().ToString() + "-" + fileName;
                        var physicalPath = string.Format(OfferPath, Directory.GetCurrentDirectory() + "/wwwroot", newFileName);
                        var virtualPath = string.Format(OfferPath, baseUrl, newFileName);

                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        oldOffer.Imageurl = virtualPath;
                    }
                }
            }

            return oldOffer;
        }

        public static IQueryable<OfferRecord> ApplyFilter(IQueryable<OfferRecord> query, OfferRecord offerRecord)
        {
            if (offerRecord.Id > 0)
                query = query.Where(c => c.Id == offerRecord.Id);
            if (offerRecord.Valid != null && offerRecord.Valid.Value == true)
                query = query.Where(c => c.Validfrom != null && c.Validfrom.Value.Date <= DateTime.UtcNow.Date
                && c.Validto != null && c.Validto.Value.Date >= DateTime.UtcNow.Date && c.Status != null && c.Status.Value == true
                && c.Usedcount <= c.Maxusagecount);

            if (!string.IsNullOrWhiteSpace(offerRecord.ObjectTypeId))
                query = query.Where(c => c.ObjectTypeId != null && c.ObjectTypeId.Trim().Contains(offerRecord.ObjectTypeId.Trim()));

            return query;
        }
    }
}