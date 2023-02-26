using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Offer.API.Offer.DAL.DB;
using Offer.BL.Services;
using Offer.CommonDefinitions.Records;
using Offer.CommonDefinitions.Requests;
using Offer.CommonDefinitions.Responses;
using Offer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Offer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OfferController : ControllerBase
    {
        private readonly OfferDbContext _context;

        public OfferController(OfferDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //[Route("Test")]
        //[Produces("application/json")]
        //public IActionResult Test()
        //{
        //    LogHelper.LogException("Message", "stackTrace");
        //    try
        //    {
        //        var li = _context.Offers.ToList();
        //        return Ok(li);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message + " stack : " + ex.StackTrace);
        //    }
        //}

        [HttpGet]
        [Route("GetAll")]
        [Produces("application/json")]
        public IActionResult GetAll()
        {
            var offerResponse = new OfferResponse();
            try
            {
                var offerRequest = new OfferRequest
                {
                    _context = _context
                };
                offerResponse = OfferService.ListOffer(offerRequest);
            }
            catch (Exception ex)
            {
                offerResponse.Message = ex.Message;
                offerResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
            return Ok(offerResponse);
        }

        /// <summary>
        /// Return Offer With id .
        /// </summary>
        [HttpGet("{id}", Name = "GetOffer")]
        [Produces("application/json")]
        public IActionResult GetOffer(int id)
        {
            var offerResponse = new OfferResponse();
            try
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
                offerResponse = OfferService.ListOffer(offerRequest);
            }
            catch (Exception ex)
            {
                offerResponse.Message = ex.Message;
                offerResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
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
            try
            {
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
            }
            catch (Exception ex)
            {
                offerResponse.Message = ex.Message;
                offerResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
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
        //    offerReq.// BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
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
        //    offerReq.// BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
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
        //    offerReq.// BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
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
            try
            {
                if (model == null)
                {
                    offerResponse.Message = "Empty Body";
                    offerResponse.Success = false;
                    return Ok(offerResponse);
                }

                model._context = _context;
                // model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
                offerResponse = OfferService.AddOffer(model);
            }
            catch (Exception ex)
            {
                offerResponse.Message = ex.Message;
                offerResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

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
            try
            {
                if (model == null)
                {
                    offerResponse.Message = "Empty Body";
                    offerResponse.Success = false;
                    return Ok(offerResponse);
                }
                model._context = _context;
                // model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
                offerResponse = OfferService.EditOffer(model);
            }
            catch (Exception ex)
            {
                offerResponse.Message = ex.Message;
                offerResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
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
            try
            {
                if (model == null)
                {
                    offerResponse.Message = "Empty Body";
                    offerResponse.Success = false;
                    return Ok(offerResponse);
                }
                model._context = _context;
                // model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
                offerResponse = OfferService.DeleteOffer(model);
            }
            catch (Exception ex)
            {
                offerResponse.Message = ex.Message;
                offerResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

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
            try
            {
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
            }
            catch (Exception ex)
            {
                offerResponse.Message = ex.Message;
                offerResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
            return Ok(offerResponse);
        }

        //[HttpGet]
        //[Route("CreateCoupon")]
        //[Produces("application/json")]
        //public IActionResult CreateCoupon()
        //{
        //    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    var random = new Random();
        //    var result = new string(
        //        Enumerable.Repeat(chars, 20)
        //                  .Select(s => s[random.Next(s.Length)])
        //                  .ToArray());
        //    return Ok(result);
        //}

        /// <summary>
        /// Return Offer With id .
        /// </summary>
        [HttpGet("GetOfferTranslate/{Offerid}", Name = "GetOfferTranslate")]
        [Produces("application/json")]
        public IActionResult GetOfferTranslate(int Offerid)
        {
            var offerTranslateResponse = new OfferTranslateResponse();
            try
            {
                var offerTranslateRequest = new OfferTranslateRequest
                {
                    _context = _context,
                    OfferTranslateRecord = new OfferTranslateRecord
                    {
                        OfferId = Offerid
                    }
                };
                offerTranslateResponse = OfferTranslateService.ListOfferTranslate(offerTranslateRequest);
            }
            catch (Exception ex)
            {
                offerTranslateResponse.Message = ex.Message;
                offerTranslateResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
            return Ok(offerTranslateResponse);
        }

        /// <summary>
        /// Creates OfferTranslate.
        /// </summary>
        [HttpPost]
        [Route("AddOfferTranslate")]
        [Produces("application/json")]
        public IActionResult AddOfferTranslate([FromBody] OfferTranslateRequest model)
        {
            var offerTranslateResponse = new OfferTranslateResponse();
            try
            {
                if (model == null)
                {
                    offerTranslateResponse.Message = "Empty Body";
                    offerTranslateResponse.Success = false;
                    return Ok(offerTranslateResponse);
                }
                offerTranslateResponse.Success = true;
                var editedTranslateOffer = model.OfferTranslateRecords.Where(c => c.Id > 0).ToList();
                if (editedTranslateOffer != null && editedTranslateOffer.Count() > 0)
                {
                    var editReq = new OfferTranslateRequest
                    {
                        _context = _context,
                        // BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase,
                        OfferTranslateRecords = editedTranslateOffer
                    };
                    offerTranslateResponse = OfferTranslateService.EditOfferTranslate(editReq);
                }

                var addedTranslateOffer = model.OfferTranslateRecords.Where(c => c.Id == 0).ToList();
                if (addedTranslateOffer != null && addedTranslateOffer.Count() > 0)
                {
                    var addReq = new OfferTranslateRequest
                    {
                        _context = _context,
                        // BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase,
                        OfferTranslateRecords = addedTranslateOffer
                    };
                    offerTranslateResponse = OfferTranslateService.AddOfferTranslate(addReq);
                }

            }
            catch (Exception ex)
            {
                offerTranslateResponse.Message = ex.Message + ex.StackTrace;
                offerTranslateResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Ok(offerTranslateResponse);
        }




        /// <summary>
        /// Remove OfferTranslate .
        /// </summary>
        [HttpPost]
        [Route("DeleteOfferTranslate")]
        [Produces("application/json")]
        public IActionResult DeleteOfferTranslate([FromBody] OfferTranslateRequest model)
        {
            var offerTranslateResponse = new OfferTranslateResponse();
            try
            {
                if (model == null)
                {
                    offerTranslateResponse.Message = "Empty Body";
                    offerTranslateResponse.Success = false;
                    return Ok(offerTranslateResponse);
                }
                model._context = _context;
                // model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
                offerTranslateResponse = OfferTranslateService.DeleteOfferTranslate(model);
            }
            catch (Exception ex)
            {
                offerTranslateResponse.Message = ex.Message;
                offerTranslateResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Ok(offerTranslateResponse);
        }

        [HttpGet]
        [Route("GetAllOfferType")]
        [Produces("application/json")]
        public IActionResult GetAllOfferType()
        {
            var offerTypeResponse = new OfferTypeResponse();
            try
            {
                var offerTypeRequest = new OfferTypeRequest
                {
                    _context = _context
                };
                offerTypeResponse = OfferTypeService.ListOfferType(offerTypeRequest);
            }
            catch (Exception ex)
            {
                offerTypeResponse.Message = ex.Message;
                offerTypeResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
            return Ok(offerTypeResponse);
        }

        /// <summary>
        /// Return OfferType With id .
        /// </summary>
        [HttpGet()]
        [Route("GetOfferType")]
        [Produces("application/json")]
        public IActionResult GetOfferType(int id)
        {
            var offerTypeResponse = new OfferTypeResponse();
            try
            {
                var offerTypeRequest = new OfferTypeRequest
                {
                    _context = _context,
                    OfferTypeRecord = new OfferTypeRecord
                    {
                        Id = id,
                        //Valid = true
                    }
                };
                offerTypeResponse = OfferTypeService.ListOfferType(offerTypeRequest);
            }
            catch (Exception ex)
            {
                offerTypeResponse.Message = ex.Message;
                offerTypeResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
            return Ok(offerTypeResponse);
        }

        /// <summary>
        /// Return List Of OfferTypes With filter valid and any  needed filter like id,name,...  .
        /// </summary>
        [HttpPost]
        [Route("GetFilteredOfferType")]
        [Produces("application/json")]
        public IActionResult GetFilteredOfferType([FromBody] OfferTypeRequest model)
        {
            var offerTypeResponse = new OfferTypeResponse();
            try
            {
                if (model == null)
                {
                    model = new OfferTypeRequest();
                }
                model._context = _context;
                //model.IsDesc = true;
                //model.OrderByColumn = "Id";

                offerTypeResponse = OfferTypeService.ListOfferType(model);
            }
            catch (Exception ex)
            {
                offerTypeResponse.Message = ex.Message;
                offerTypeResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Ok(offerTypeResponse);
        }

        /// <summary>
        /// Creates OfferType, Uncheck Send empty value in Id,Creationdate,Isdeleted,IsDesc,PageSize,PageIndex.
        /// </summary>
        [HttpPost]
        [Route("AddOfferType")]
        [Produces("application/json")]
        public IActionResult AddOfferType([FromBody] OfferTypeRequest model)
        {
            var offerTypeResponse = new OfferTypeResponse();
            try
            {
                if (model == null)
                {
                    offerTypeResponse.Message = "Empty Body";
                    offerTypeResponse.Success = false;
                    return Ok(offerTypeResponse);
                }

                model._context = _context;
                // model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
                offerTypeResponse = OfferTypeService.AddOfferType(model);
            }
            catch (Exception ex)
            {
                offerTypeResponse.Message = ex.Message;
                offerTypeResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Ok(offerTypeResponse);
        }

        /// <summary>
        /// Update OfferType , Uncheck Send empty value in Id,Creationdate,Isdeleted,IsDesc,PageSize,PageIndex.
        /// </summary>
        [HttpPost]
        [Route("EditOfferType")]
        [Produces("application/json")]
        public IActionResult EditOfferType([FromBody] OfferTypeRequest model)
        {
            var offerTypeResponse = new OfferTypeResponse();
            try
            {
                if (model == null)
                {
                    offerTypeResponse.Message = "Empty Body";
                    offerTypeResponse.Success = false;
                    return Ok(offerTypeResponse);
                }
                model._context = _context;
                // model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
                offerTypeResponse = OfferTypeService.EditOfferType(model);
            }
            catch (Exception ex)
            {
                offerTypeResponse.Message = ex.Message;
                offerTypeResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
            return Ok(offerTypeResponse);
        }

        /// <summary>
        /// Remove OfferType .
        /// </summary>
        [HttpPost]
        [Route("DeleteOfferType")]
        [Produces("application/json")]
        public IActionResult DeleteOfferType([FromBody] OfferTypeRequest model)
        {
            var offerTypeResponse = new OfferTypeResponse();
            try
            {
                if (model == null)
                {
                    offerTypeResponse.Message = "Empty Body";
                    offerTypeResponse.Success = false;
                    return Ok(offerTypeResponse);
                }
                model._context = _context;
                // model.BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase;
                offerTypeResponse = OfferTypeService.DeleteOfferType(model);
            }
            catch (Exception ex)
            {
                offerTypeResponse.Message = ex.Message;
                offerTypeResponse.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Ok(offerTypeResponse);
        }


    }
}