﻿using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Offer.CommonDefinitions.Requests;
using Offer.Helpers;
using Offer.CommonDefinitions.Responses;
using System.Net;
using Offer.BL.Services.Managers;

namespace Offer.BL.Services
{
    public class OfferService : BaseService
    {
        public static OfferResponse ListOffer(OfferRequest request)
        {
            var res = new OfferResponse();
            RunBase(request, res, (OfferRequest req) =>
             {
                 try
                 {
                     var query = request._context.Offers.Where(c => !c.Isdeleted).Select(c => new OfferRecord
                     {
                         Id = c.Id,
                         CreatedBy = c.CreatedBy,
                         Creationdate = c.Creationdate,
                         Name =  c.Name,
                         Description =  c.Description,
                         Validfrom = c.Validfrom,
                         Validto = c.Validto,
                         Modificationdate = c.Modificationdate,
                         ModifiedBy = c.ModifiedBy,
                         Discount = c.Discount,
                         Status = c.Status,
                         Purpose = c.Purpose,
                         Imageurl = c.Imageurl,
                         Maxusagecount = c.Maxusagecount,
                         Usedcount = c.Usedcount,
                         LanguageId = c.LanguageId,
                         ObjectTypeId = c.ObjectTypeId,
                         ObjectId = c.ObjectId,
                         ObjectUrl = c.ObjectUrl,
                         ActionTypeId = c.ActionTypeId,
                         MaxValue = c.MaxValue,
                         MinValue = c.MinValue,
                         ConstantType = c.ConstantType,
                         ActionType=c.ActionType,
                         Translates = !string.IsNullOrWhiteSpace(request.LanguageId) &&
                                         c.OfferTranslates != null
                                ? c.OfferTranslates.Where(t => t.LanguageId == request.LanguageId)
                                : null,
                         OfferTypeId=c.OfferTypeId,
                         OfferType=c.OfferType!=null?c.OfferType.Name:""
                     });

                     if (request.OfferRecord != null)
                         query = OfferServiceManager.ApplyFilter(query, request.OfferRecord);

                     res.TotalCount = query.Count();

                     query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);
                     query = request.PageSize > 0 ? ApplyPaging(query, request.PageSize, request.PageIndex) : ApplyPaging(query, request.DefaultPageSize, 0);

                     res.OfferRecords = query.ToList();
                     res.Message = HttpStatusCode.OK.ToString();
                     res.Success = true;
                     res.StatusCode = HttpStatusCode.OK;
                 }
                 catch (Exception ex)
                 {
                     res.Message = ex.Message;
                     res.Success = false;
                     LogHelper.LogException(ex.Message, ex.StackTrace);
                 }
                 return res;
             });
            return res;
        }

        public static OfferResponse DeleteOffer(OfferRequest request)
        {
            var res = new OfferResponse();
            RunBase(request, res, (OfferRequest req) =>
             {
                 try
                 {
                     var model = request.OfferRecord;
                     var offer = request._context.Offers.FirstOrDefault(c => !c.Isdeleted && c.Id == model.Id);
                     if (offer != null)
                     {
                         //update offer IsDeleted
                         offer.Isdeleted = true;
                         offer.Modificationdate = DateTime.Now;
                         request._context.SaveChanges();

                         res.Message = HttpStatusCode.OK.ToString();
                         res.Success = true;
                         res.StatusCode = HttpStatusCode.OK;
                     }
                     else
                     {
                         res.Message = "Invalid offer";
                         res.Success = false;
                     }
                 }
                 catch (Exception ex)
                 {
                     res.Message = ex.Message;
                     res.Success = false;
                     LogHelper.LogException(ex.Message, ex.StackTrace);
                 }
                 return res;
             });
            return res;
        }

        public static OfferResponse EditOffer(OfferRequest request)
        {
            var res = new OfferResponse();
            RunBase(request, res, (OfferRequest req) =>
            {
                try
                {
                    var model = request.OfferRecord;
                    var offer = request._context.Offers.Find(model.Id);
                    if (offer != null)
                    {
                        //update whole offer
                        offer = OfferServiceManager.AddOrEditOffer(request.BaseUrl, request.OfferRecord, offer);
                        request._context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid offer";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }

        public static OfferResponse AddOffer(OfferRequest request)
        {
            var res = new OfferResponse();
            RunBase(request, res, (OfferRequest req) =>
            {
                try
                {
                    var OfferExist = request._context.Offers.Any(m => m.Name.ToLower() == request.OfferRecord.Name.ToLower() && !m.Isdeleted);
                    if (!OfferExist)
                    {
                        var offer = OfferServiceManager.AddOrEditOffer(request.BaseUrl, request.OfferRecord);
                        request._context.Offers.Add(offer);
                        request._context.SaveChanges();
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Offer already exist";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message + " " + ex.StackTrace + " " + ex.InnerException +ex.Data;
                    res.Success = false;
                    LogHelper.LogException(ex.Message + ex.InnerException, ex.StackTrace);
                }
                return res;
            });
            return res;
        }
    }
}