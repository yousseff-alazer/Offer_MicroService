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
    public class OfferUserServiceManager
    {
        private const string OfferUserPath = "{0}/ContentFiles/OfferUser/{1}";

        public static OfferUser AddOrEditOfferUser(string baseUrl, long createdBy, OfferUserRecord record, OfferUser oldOfferUser = null)
        {
            if (oldOfferUser == null)//new offerUser
            {
                oldOfferUser = new OfferUser();
                oldOfferUser.Creationdate = DateTime.Now;
                oldOfferUser.Createdby = createdBy;
            }
            else
            {
                oldOfferUser.Modificationdate = DateTime.Now;
                oldOfferUser.Modifiedby = createdBy;
            }

            //upload
            //var file = record.ApkFile;
            //if (file != null && file.Length > 0)
            //{
            //    var fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
            //    var physicalPath = string.Format(OfferUserPath, Directory.GetCurrentDirectory() + "/wwwroot", fileName);
            //    var virtualPath = string.Format(OfferUserPath, baseUrl, fileName);

            //    using (var stream = new FileStream(physicalPath, FileMode.Create))
            //    {
            //        file.CopyTo(stream);
            //    }
            //    oldOfferUser.ApkFileUrl = virtualPath;
            //}

            return oldOfferUser;
        }

        public static IQueryable<OfferUserRecord> ApplyFilter(IQueryable<OfferUserRecord> query, OfferUserRecord offerUserRecord)
        {
            if (offerUserRecord.Id > 0)
                query = query.Where(c => c.Id == offerUserRecord.Id);

            return query;
        }
    }
}