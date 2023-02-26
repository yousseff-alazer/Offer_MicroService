using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Offer.CommonDefinitions.Records
{
    public class OfferTranslateRecord
    {
        public int Id { get; set; }
        public DateTime Creationdate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Modificationdate { get; set; }
        public string Purpose { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string LanguageId { get; set; }
        public int? OfferId { get; set; }
    }
}