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
    public class OfferTypeServiceManager
    {
        private const string OfferTypePath = "{0}/ContentFiles/OfferType/{1}";

        public static OfferType AddOrEditOfferType(string baseUrl /*, long createdBy*/,
            OfferTypeRecord record, OfferType oldOfferType = null)
        {
            if (oldOfferType == null) //new offerType
            {
                oldOfferType = new OfferType();
                oldOfferType.Modificationdate = DateTime.Now;
                oldOfferType.CreatedBy = record.CreatedBy;
            }
            else
            {
                oldOfferType.Modificationdate = DateTime.Now;
                oldOfferType.ModifiedBy = record.CreatedBy;
            }

            if (!string.IsNullOrWhiteSpace(record.Name)) oldOfferType.Name = record.Name;
            if (!string.IsNullOrWhiteSpace(record.Description)) oldOfferType.Description = record.Description;
            return oldOfferType;
        }

        public static IQueryable<OfferTypeRecord> ApplyFilter(IQueryable<OfferTypeRecord> query,
            OfferTypeRecord offerTypeRecord)
        {
            if (offerTypeRecord.Id > 0)
                query = query.Where(c => c.Id == offerTypeRecord.Id);
            //if (offerTypeRecord.Valid != null && offerTypeRecord.Valid.Value == true)
            //    query = query.Where(c => c.Validfrom != null && c.Validfrom.Value.Date <= DateTime.UtcNow.Date
            //    && c.Validto != null && c.Validto.Value.Date >= DateTime.UtcNow.Date && c.Status != null && c.Status.Value == true
            //    && c.Usedcount <= c.Maxusagecount);

            //if (!string.IsNullOrWhiteSpace(offerTypeRecord.ObjectTypeId))
            //    query = query.Where(c => c.ObjectTypeId != null && c.ObjectTypeId.Trim().Contains(offerTypeRecord.ObjectTypeId.Trim()));

            return query;
        }
    }
}