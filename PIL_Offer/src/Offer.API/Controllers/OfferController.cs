using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Offer.API.Offer.DAL.DB;
using Offer.BL.Services;
using Offer.CommonDefinitions.Records;
using Offer.CommonDefinitions.Requests;
using Offer.CommonDefinitions.Responses;
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
        [Produces("application/json")]
        public IActionResult GetAll()
        {
            var offerRequest = new OfferRequest
            {
                _context = _context
            };
            var offerResponse = OfferService.ListOffer(offerRequest);
            return Ok(offerResponse);
        }

        /// <summary>
        /// Return List Of Offers With filter if needed like id,name,...  .
        /// </summary>
        [HttpPost]
        [Route("GetFiltered")]
        [Produces("application/json")]
        public IActionResult GetFiltered([FromBody] OfferRequest model)
        {
            var offerResponse = new OfferResponse();
            if (model == null)
            {
                model = new OfferRequest();
            }
            model._context = _context;
            model.IsDesc = true;
            model.OrderByColumn = "Id";

            offerResponse = OfferService.ListOffer(model);
            return Ok(offerResponse);
        }

        //[HttpPost]
        //[Route("Add")]
        //public IActionResult Add([FromForm] OfferRecord model)
        //{
        //    var offerResponse = new OfferResponse();
        //    if (model == null)
        //    {
        //        offerResponse.Message = "Empty Body";
        //        offerResponse.Success = false;
        //        return Ok(offerResponse);
        //    }
        //    var offerReq = new OfferRequest();
        //    offerReq.OfferRecord = model;
        //    offerReq._context = _context;
        //    offerReq.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
        //    offerResponse = OfferService.AddOffer(offerReq);
        //    return Ok(offerResponse);
        //}

        //[HttpPost]
        //[Route("Edit")]
        //public IActionResult Edit([FromForm] OfferRecord model)
        //{
        //    var offerResponse = new OfferResponse();
        //    if (model == null)
        //    {
        //        offerResponse.Message = "Empty Body";
        //        offerResponse.Success = false;
        //        return Ok(offerResponse);
        //    }
        //    var offerReq = new OfferRequest();
        //    offerReq.OfferRecord = model;
        //    offerReq._context = _context;
        //    offerReq.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
        //    offerResponse = OfferService.EditOffer(offerReq);
        //    return Ok(offerResponse);
        //}

        //[HttpPost]
        //[Route("Delete")]
        //public IActionResult Delete([FromBody] OfferRecord model)
        //{
        //    var offerResponse = new OfferResponse();
        //    if (model == null)
        //    {
        //        offerResponse.Message = "Empty Body";
        //        offerResponse.Success = false;
        //        return Ok(offerResponse);
        //    }
        //    var offerReq = new OfferRequest();
        //    offerReq.OfferRecord = model;
        //    offerReq._context = _context;
        //    offerReq.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
        //    offerResponse = OfferService.DeleteOffer(offerReq);
        //    return Ok(offerResponse);
        //}

        /// <summary>
        /// Creates Offer, Uncheck Send empty value in Id,Creationdate,Isdeleted,IsDesc,PageSize,PageIndex.
        /// </summary>
        [HttpPost]
        [Route("Add")]
        [Produces("application/json")]
        public IActionResult Add([FromForm] OfferRequest model)
        {
            var offerResponse = new OfferResponse();
            if (model == null)
            {
                offerResponse.Message = "Empty Body";
                offerResponse.Success = false;
                return Ok(offerResponse);
            }

            model._context = _context;
            model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
            offerResponse = OfferService.AddOffer(model);
            return Ok(offerResponse);
        }

        /// <summary>
        /// Update Offer , Uncheck Send empty value in Id,Creationdate,Isdeleted,IsDesc,PageSize,PageIndex.
        /// </summary>
        [HttpPost]
        [Route("Edit")]
        [Produces("application/json")]
        public IActionResult Edit([FromForm] OfferRequest model)
        {
            var offerResponse = new OfferResponse();
            if (model == null)
            {
                offerResponse.Message = "Empty Body";
                offerResponse.Success = false;
                return Ok(offerResponse);
            }
            model._context = _context;
            model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
            offerResponse = OfferService.EditOffer(model);
            return Ok(offerResponse);
        }

        /// <summary>
        /// Remove Offer .
        /// </summary>
        [HttpPost]
        [Route("Delete")]
        [Produces("application/json")]
        public IActionResult Delete([FromBody] OfferRequest model)
        {
            var offerResponse = new OfferResponse();
            if (model == null)
            {
                offerResponse.Message = "Empty Body";
                offerResponse.Success = false;
                return Ok(offerResponse);
            }
            model._context = _context;
            model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
            offerResponse = OfferService.DeleteOffer(model);
            return Ok(offerResponse);
        }

        /// <summary>
        /// Use Offer By User For One Time.
        /// </summary>
        [HttpPost]
        [Route("Redeem")]
        [Produces("application/json")]
        public IActionResult Redeem([FromBody] OfferUserRequest model)
        {
            var offerResponse = new OfferUserResponse();
            if (model == null || (model != null && (model.OfferUserRecord.Offerid == null || !string.IsNullOrWhiteSpace(model.OfferUserRecord.Userid))))
            {
                offerResponse.Message = "Wrong Body";
                offerResponse.Success = false;
                return Ok(offerResponse);
            }
            model._context = _context;
            offerResponse = OfferUserService.AddOfferUser(model);
            return Ok(offerResponse);
        }
    }
}