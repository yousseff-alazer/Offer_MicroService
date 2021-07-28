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

        [HttpGet]
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
        /// Return Offer With id .
        /// </summary>
        [HttpGet("{id}", Name = "GetOffer")]
        [Produces("application/json")]
        public IActionResult GetOffer(int id)
        {
            var offerRequest = new OfferRequest
            {
                _context = _context,
                OfferRecord = new OfferRecord
                {
                    Id = id,
                    //Valid = true
                }
            };
            var offerResponse = OfferService.ListOffer(offerRequest);
            return Ok(offerResponse);
        }

        /// <summary>
        /// Return List Of Offers With filter valid and any  needed filter like id,name,...  .
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
            //model.IsDesc = true;
            //model.OrderByColumn = "Id";

            offerResponse = OfferService.ListOffer(model);
            if (offerResponse.Success && offerResponse.OfferRecords.Count > 0 && model.OfferRecord != null
                && !string.IsNullOrWhiteSpace(model.OfferRecord.CreatedBy))
            {
                var offerIds = offerResponse.OfferRecords.Select(c => c.Id).ToList();
                var offerUserReq = new OfferUserRequest()
                {
                    _context = _context,
                    OfferUserRecord = new OfferUserRecord()
                    {
                        OfferIds = offerIds,
                        CreatedBy = model.OfferRecord.CreatedBy
                    }
                };
                var offerUserResponse = OfferUserService.ListOfferUser(offerUserReq);
                if (offerUserResponse.Success && offerUserResponse.OfferUserRecords.Count > 0)
                {
                    var usedOfferIds = offerUserResponse.OfferUserRecords.Where(c => c.Offerid != null).Select(c => c.Offerid).ToList();
                    offerResponse.OfferRecords = offerResponse.OfferRecords.Where(c => !usedOfferIds.Contains(c.Id)).ToList();
                }
            }

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

        ///// <summary>
        ///// Use Offer By User.
        ///// </summary>
        //[HttpPost]
        //[Route("Redeem")]
        //[Produces("application/json")]
        //public IActionResult Redeem([FromBody] OfferUserRequest model)
        //{
        //    var offerResponse = new OfferUserResponse();
        //    if (model == null || (model != null && (model.OfferUserRecord.Offerid == null || !string.IsNullOrWhiteSpace(model.OfferUserRecord.UserId))))
        //    {
        //        offerResponse.Message = "Wrong Body";
        //        offerResponse.Success = false;
        //        return Ok(offerResponse);
        //    }
        //    model._context = _context;
        //    offerResponse = OfferUserService.AddOfferUser(model);
        //    return Ok(offerResponse);
        //}

        /// <summary>
        /// Use Offer.
        /// </summary>
        [HttpGet("{offerid}/{userid}", Name = "UseOffer")]
        [Produces("application/json")]
        public IActionResult UseOffer(int offerid, string userid)
        {
            var offerResponse = new OfferUserResponse();
            if (offerid == 0 || !string.IsNullOrWhiteSpace(userid))
            {
                offerResponse.Message = "Wrong Input";
                offerResponse.Success = false;
                return Ok(offerResponse);
            }
            var offerUserReq = new OfferUserRequest()
            {
                _context = _context,
                OfferUserRecord = new OfferUserRecord()
                {
                    UserId = userid,
                    Offerid = offerid
                }
            };
            offerResponse = OfferUserService.AddOfferUser(offerUserReq);
            if (offerResponse.Success)
            {
                var offerEditReq = new OfferRequest()
                {
                    _context = _context,
                    OfferRecord = new OfferRecord()
                    {
                        Id = offerid,
                        AddUse = true
                    }
                };
                OfferService.EditOffer(offerEditReq);
            }
            return Ok(offerResponse);
        }
    }
}