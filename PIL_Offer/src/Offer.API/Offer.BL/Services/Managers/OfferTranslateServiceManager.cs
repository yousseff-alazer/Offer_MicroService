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
    public class OfferTranslateServiceManager
    {
        private const string S3Path = "{0}/ContentFiles/OfferTranslate/{1}";

        public static OfferTranslate AddOrEditOfferTranslate(string baseUrl /*, long createdBy*/,
            OfferTranslateRecord record, OfferTranslate oldOfferTranslate = null)
        {
            if (oldOfferTranslate == null) //new offerTranslate
            {
                oldOfferTranslate = new OfferTranslate();
                oldOfferTranslate.Modificationdate = DateTime.Now;
                oldOfferTranslate.CreatedBy = record.CreatedBy;
            }
            else
            {
                oldOfferTranslate.Modificationdate = DateTime.Now;
                oldOfferTranslate.ModifiedBy = record.CreatedBy;
            }

            if (!string.IsNullOrWhiteSpace(record.Name)) oldOfferTranslate.Name = record.Name;
            if (!string.IsNullOrWhiteSpace(record.Purpose)) oldOfferTranslate.Purpose = record.Purpose;
            if (!string.IsNullOrWhiteSpace(record.Description)) oldOfferTranslate.Description = record.Description;
            if (!string.IsNullOrWhiteSpace(record.LanguageId)) oldOfferTranslate.LanguageId = record.LanguageId;
            if (record.OfferId > 0) oldOfferTranslate.OfferId = record.OfferId;
            return oldOfferTranslate;
        }

        public static IQueryable<OfferTranslateRecord> ApplyFilter(IQueryable<OfferTranslateRecord> query,
            OfferTranslateRecord offerTranslateRecord)
        {
            if (offerTranslateRecord.Id > 0)
                query = query.Where(c => c.Id == offerTranslateRecord.Id);
            if (offerTranslateRecord.OfferId > 0)
                query = query.Where(c => c.OfferId == offerTranslateRecord.OfferId);
            //if (offerTranslateRecord.Valid != null && offerTranslateRecord.Valid.Value == true)
            //    query = query.Where(c => c.Validfrom != null && c.Validfrom.Value.Date <= DateTime.UtcNow.Date
            //    && c.Validto != null && c.Validto.Value.Date >= DateTime.UtcNow.Date && c.Status != null && c.Status.Value == true
            //    && c.Usedcount <= c.Maxusagecount);

            //if (!string.IsNullOrWhiteSpace(offerTranslateRecord.ObjectTypeId))
            //    query = query.Where(c => c.ObjectTypeId != null && c.ObjectTypeId.Trim().Contains(offerTranslateRecord.ObjectTypeId.Trim()));

            return query;
        }
    }
}