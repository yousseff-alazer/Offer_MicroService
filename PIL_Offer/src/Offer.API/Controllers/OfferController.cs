using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Offer.BL.Services;
using Offer.CommonDefinitions.Requests;
using Offer.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Offer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly OfferDbContext _context;

        public OfferController(OfferDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var offerRequest = new OfferRequest
            {
                _context = _context
            };
            var offerResponse = OfferService.ListOffer(offerRequest);
            return Ok(offerResponse);
        }

        [HttpPost]
        [Route("GetFiltered")]
        public IActionResult GetFiltered([FromBody] OfferRequest model)
        {
            model._context = _context;
            model.IsDesc = true;
            model.OrderByColumn = "Id";
            model.GetMineOnly = true;

            var offerResponse = OfferService.ListOffer(model);
            return Ok(offerResponse);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] OfferRequest model)
        {
            if (model == null)
            {
                model = new OfferRequest();
            }
            model._context = _context;
            var offerResponse = OfferService.AddOffer(model);
            return Ok(offerResponse);
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody] OfferRequest model)
        {
            if (model == null)
            {
                model = new OfferRequest();
            }
            model._context = _context;
            var offerResponse = OfferService.EditOffer(model);
            return Ok(offerResponse);
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete([FromBody] OfferRequest model)
        {
            if (model == null)
            {
                model = new OfferRequest();
            }
            model._context = _context;
            var offerResponse = OfferService.DeleteOffer(model);
            return Ok(offerResponse);
        }

        [HttpPost]
        [Route("Claim")]
        public IActionResult Claim([FromBody] OfferUserRequest model)
        {
            if (model == null)
            {
                model = new OfferUserRequest();
            }
            model._context = _context;
            var offerResponse = OfferUserService.AddOfferUser(model);
            return Ok(offerResponse);
        }
    }
}