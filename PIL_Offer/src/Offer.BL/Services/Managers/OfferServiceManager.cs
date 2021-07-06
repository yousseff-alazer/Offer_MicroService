using Offer.DAL.DB;
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

        public static DAL.DB.Offer AddOrEditOffer(string baseUrl, long createdBy, OfferRecord record, DAL.DB.Offer oldOffer = null)
        {
            if (oldOffer == null)//new offer
            {
                oldOffer = new DAL.DB.Offer();
                oldOffer.Creationdate = DateTime.Now;
                oldOffer.Createdby = createdBy;
            }
            else
            {
                oldOffer.Modificationdate = DateTime.Now;
                oldOffer.Modifiedby = createdBy;
            }

            //upload
            //var file = record.ApkFile;
            //if (file != null && file.Length > 0)
            //{
            //    var fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
            //    var physicalPath = string.Format(OfferPath, Directory.GetCurrentDirectory() + "/wwwroot", fileName);
            //    var virtualPath = string.Format(OfferPath, baseUrl, fileName);

            //    using (var stream = new FileStream(physicalPath, FileMode.Create))
            //    {
            //        file.CopyTo(stream);
            //    }
            //    oldOffer.ApkFileUrl = virtualPath;
            //}

            return oldOffer;
        }

        public static IQueryable<OfferRecord> ApplyFilter(IQueryable<OfferRecord> query, OfferRecord offerRecord)
        {
            if (offerRecord.Id > 0)
                query = query.Where(c => c.Id == offerRecord.Id);

            return query;
        }
    }
}