using Offer.CommonDefinitions.Records;
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
    public class OfferTranslateService : BaseService
    {
        public static OfferTranslateResponse ListOfferTranslate(OfferTranslateRequest request)
        {
            var res = new OfferTranslateResponse();
            RunBase(request, res, req =>

            {
                try
                {
                    var query = request._context.OfferTranslates.Where(c => !c.Isdeleted).Select(c =>
                        new OfferTranslateRecord
                        {
                            Id = c.Id,
                            Creationdate = c.Creationdate,
                            CreatedBy = c.CreatedBy,
                            ModifiedBy = c.ModifiedBy,
                            Modificationdate = c.Modificationdate,
                            Name = c.Name,
                            Description = c.Description,
                            OfferId = c.OfferId,
                            Purpose = c.Purpose,
                            LanguageId = c.LanguageId,
                        });

                    if (request.OfferTranslateRecord != null)
                        query = OfferTranslateServiceManager.ApplyFilter(query, request.OfferTranslateRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);

                    query = request.PageSize > 0 ? ApplyPaging(query, request.PageSize, request.PageIndex) : ApplyPaging(query, request.DefaultPageSize, 0);

                    res.OfferTranslateRecords = query.ToList();
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

        public static OfferTranslateResponse DeleteOfferTranslate(OfferTranslateRequest request)
        {
            var res = new OfferTranslateResponse();
            RunBase(request, res, req =>
            {
                try
                {
                    var model = request.OfferTranslateRecord;
                    var offerTranslate =
                        request._context.OfferTranslates.FirstOrDefault(c => !c.Isdeleted && c.Id == model.Id);
                    if (offerTranslate != null)
                    {
                        //update offerTranslate IsDeleted
                        offerTranslate.Isdeleted = true;
                        offerTranslate.Modificationdate = DateTime.Now;
                        request._context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid offerTranslate";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message + " " + ex.StackTrace + " " + ex.InnerException;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }

                return res;
            });
            return res;
        }

        public static OfferTranslateResponse EditOfferTranslate(OfferTranslateRequest request)
        {
            var res = new OfferTranslateResponse();
            RunBase(request, res, req =>
            {
                try
                {
                    foreach (var model in req.OfferTranslateRecords)
                    {
                        var offerTranslate = request._context.OfferTranslates.Find(model.Id);
                        if (offerTranslate != null)
                        {
                            //update whole offerTranslate
                            offerTranslate = OfferTranslateServiceManager.AddOrEditOfferTranslate(request.BaseUrl,
                                model, offerTranslate);
                            request._context.SaveChanges();

                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Invalid offerTranslate";
                            res.Success = false;
                        }
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

        public static OfferTranslateResponse AddOfferTranslate(OfferTranslateRequest request)
        {
            var res = new OfferTranslateResponse();
            RunBase(request, res, req =>
            {
                try
                {
                    foreach (var model in req.OfferTranslateRecords)
                    {
                        var OfferTranslateExist = request._context.OfferTranslates.Any(m =>
                            m.Name.ToLower() == model.Name.ToLower() && !m.Isdeleted && m.OfferId == model.OfferId && m.LanguageId == model.LanguageId);
                        if (!OfferTranslateExist)
                        {
                            var offerTranslate =
                                OfferTranslateServiceManager.AddOrEditOfferTranslate(request.BaseUrl,
                                    model);
                            request._context.OfferTranslates.Add(offerTranslate);
                            request._context.SaveChanges();
                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "OfferTranslate already exist";
                            res.Success = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message+ex.StackTrace;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }

                return res;
            });
            return res;
        }
    }
}