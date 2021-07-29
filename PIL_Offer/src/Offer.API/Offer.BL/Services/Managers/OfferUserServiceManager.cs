using Offer.API.Offer.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offer.CommonDefinitions.Records;
using System.IO;

using Offer.API.Offer.DAL.DB;

namespace Offer.BL.Services.Managers
{
    public class OfferUserServiceManager
    {
        private const string OfferUserPath = "{0}/ContentFiles/OfferUser/{1}";

        public static OfferUser AddOrEditOfferUser(string baseUrl, OfferUserRecord record, OfferUser oldOfferUser = null)
        {
            if (oldOfferUser == null)//new offerUser
            {
                oldOfferUser = new OfferUser();
                oldOfferUser.Creationdate = DateTime.Now;
                oldOfferUser.CreatedBy = record.CreatedBy;
            }
            else
            {
                oldOfferUser.Modificationdate = DateTime.Now;
                oldOfferUser.ModifiedBy = record.CreatedBy;
            }
            if (record.Offerid != null)
            {
                oldOfferUser.Offerid = record.Offerid;
            }
            if (!string.IsNullOrWhiteSpace(record.UserId))
            {
                oldOfferUser.UserId = record.UserId;
            }
            return oldOfferUser;
        }

        public static IQueryable<OfferUserRecord> ApplyFilter(IQueryable<OfferUserRecord> query, OfferUserRecord offerUserRecord)
        {
            if (offerUserRecord.Id > 0)
                query = query.Where(c => c.Id == offerUserRecord.Id);
            if (offerUserRecord.OfferIds.Count > 0)
                query = query.Where(c => offerUserRecord.OfferIds.Contains(c.Id));

            return query;
        }
    }
}