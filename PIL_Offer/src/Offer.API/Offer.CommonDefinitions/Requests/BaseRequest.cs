using Offer.DAL.DB;

namespace Offer.CommonDefinitions.Requests
{
    public class BaseRequest
    {
        public OfferDbContext _context;

        public const int DefaultPageSize = 37;

        public bool IsDesc { get; set; }

        public string OrderByColumn { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public long CreatedBy { get; set; }

        public long RoleID { get; set; }
        public long LanguageId { get; set; }
        public string RoleName { get; set; } //not in all requests
        public string BaseUrl { get; set; }
        public bool GetMineOnly { get; set; }
    }
}