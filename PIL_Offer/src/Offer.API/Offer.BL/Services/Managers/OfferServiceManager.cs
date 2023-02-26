using Offer.API.Offer.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offer.CommonDefinitions.Records;
using System.IO;
using Offer.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Offer.API.Offer.CommonDefinitions.Records;

namespace Offer.BL.Services.Managers
{
    public class OfferServiceManager
    {
        private const string S3Path = "https://storage.pil.live/api/storage/";
        private const string S3DeletePath = "https://storage.pil.live/api/storage/delete";
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
                oldOffer.ModifiedBy = record.CreatedBy;
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
            if (record.ObjectId != null)
            {
                oldOffer.ObjectId = record.ObjectId;
            }
            if (record.ObjectTypeId != null)
            {
                oldOffer.ObjectTypeId = record.ObjectTypeId;
            }
            if (record.ObjectUrl != null)
            {
                oldOffer.ObjectUrl = record.ObjectUrl;
            }
            if (record.MaxValue != null)
            {
                oldOffer.MaxValue = record.MaxValue;
            }
            if (record.MinValue != null)
            {
                oldOffer.MinValue = record.MinValue;
            }
            if (record.ActionTypeId != null)
            {
                oldOffer.ActionTypeId = record.ActionTypeId;
            } 
            if (record.AddUse != null && record.AddUse == true)
            {
                oldOffer.Usedcount = oldOffer.Usedcount + 1;
            }
            if (record.ConstantType != null)
            {
                oldOffer.ConstantType = record.ConstantType;
            }
            if (record.ActionType != null)
            {
                oldOffer.ActionType = record.ActionType;
            }
            if (record.OfferTypeId != null)
            {
                oldOffer.OfferTypeId = record.OfferTypeId;
            }
            //    Imageurl = c.Imageurl
            //upload
            if (record.FormImage != null)
            {
                var allowedExtensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };
                var extension = Path.GetExtension(record.FormImage.FileName);
                if (allowedExtensions.Contains(extension))
                {
                    if (!string.IsNullOrWhiteSpace(oldOffer.Imageurl)&&oldOffer.Imageurl.Contains("pilservices.s3"))
                    {
                        var S3Record = new Data()
                        {
                            file = oldOffer.Imageurl
                        };
                        var jsonString = JsonConvert.SerializeObject(S3Record, Formatting.None);
                        var Response =
                            UIHelper.AddRequestToServiceApi(
                                S3DeletePath, jsonString);
                        //var responseResult = Response.Content.ReadAsStringAsync().Result;
                    }
                    var httpResponse =
UIHelper.Upload(
S3Path, record.FormImage, record.ObjectId);
                    if (!string.IsNullOrWhiteSpace(httpResponse))
                    {
                        //var token2 = JToken.Parse(httpResponse);
                        var model = JsonConvert.DeserializeObject<S3Record>(httpResponse);
                        if (model!=null) { oldOffer.Imageurl = model?.Data?.file; }
                        
                    }
                    //var file = record.FormImage.OpenReadStream();
                    //var fileName = record.FormImage.FileName;
                    //if (file.Length > 0)
                    //{
                    //    var newFileName = Guid.NewGuid().ToString() + "-" + fileName;
                    //    var physicalPath = string.Format(OfferPath, Directory.GetCurrentDirectory() + "/wwwroot", newFileName);
                    //    string dirPath = Path.GetDirectoryName(physicalPath);

                    //    if (!Directory.Exists(dirPath))
                    //        Directory.CreateDirectory(dirPath);
                    //    var virtualPath = string.Format(OfferPath, baseUrl, newFileName);

                    //    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    //    {
                    //        file.CopyTo(stream);
                    //    }
                    //    oldOffer.Imageurl = virtualPath;
                    //}
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

            if (!string.IsNullOrWhiteSpace(offerRecord.ObjectId))
                query = query.Where(c => c.ObjectId != null && c.ObjectId.Trim().Contains(offerRecord.ObjectId.Trim()));

            if (!string.IsNullOrWhiteSpace(offerRecord.OfferType))
                query = query.Where(c => c.OfferType != null && c.OfferType.Trim().Contains(offerRecord.OfferType.Trim()));
            if (offerRecord.OfferTypeId > 0)
                query = query.Where(c => c.OfferTypeId == offerRecord.OfferTypeId);
            return query;
        }
    }
}