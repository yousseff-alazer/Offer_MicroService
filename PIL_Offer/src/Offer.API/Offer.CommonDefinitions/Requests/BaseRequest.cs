using Offer.API.Offer.DAL.DB;

namespace Offer.CommonDefinitions.Requests
{
    public class BaseRequest
    {
        public OfferDbContext _context;

        public const int DefaultPageSize = 30;

        public bool IsDesc { get; set; }

        public string OrderByColumn { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        //public long? CreatedBy { get; set; }

        public string BaseUrl { get; set; }
    }
}