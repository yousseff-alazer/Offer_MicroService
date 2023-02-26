using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Offer.API.Offer.DAL.DB;
using System.Linq;
using System.Text.Json.Serialization;

namespace Offer.CommonDefinitions.Records
{
    public class OfferRecord
    {
        public int Id { get; set; }
        public DateTime Creationdate { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Validfrom { get; set; }
        public DateTime? Validto { get; set; }
        public DateTime? Modificationdate { get; set; }
        public string Discount { get; set; }
        public bool? Status { get; set; }
        public string Purpose { get; set; }
        public string Imageurl { get; set; }
        public long? Maxusagecount { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string LanguageId { get; set; }
        public IFormFile FormImage { get; set; }

        public bool? Valid { get; set; } // for filter
        public bool? AddUse { get; set; }// for add count

        public string ObjectTypeId { get; set; }
        public string ObjectId { get; set; }
        public long? Usedcount { get; set; }

        public string ObjectUrl { get; set; }

        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string ActionTypeId { get; set; }

        public string ConstantType { get; set; }

        public string ActionType { get; set; }
        public int? OfferTypeId { get; set; }
        public string OfferType { get; set; }
        [JsonIgnore]
        public IEnumerable<OfferTranslate> Translates { get; set; }


        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (Translates != null && Translates.Any())
                    name = Translates?.FirstOrDefault()?.Name;
                else
                    name = value;

            }
        }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (Translates != null && Translates.Any())
                    description = Translates?.FirstOrDefault()?.Description;
                else
                    description = value;

            }
        }

    }
}