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
    public class OfferTypeService : BaseService
    {
        public static OfferTypeResponse ListOfferType(OfferTypeRequest request)
        {
            var res = new OfferTypeResponse();
            RunBase(request, res, req =>

            {
                try
                {
                    var query = request._context.OfferTypes.Where(c => !c.Isdeleted).Select(c =>
                        new OfferTypeRecord
                        {
                            Id = c.Id,
                            Creationdate = c.Creationdate,
                            CreatedBy = c.CreatedBy,
                            ModifiedBy = c.ModifiedBy,
                            Modificationdate = c.Modificationdate,
                            Name = c.Name,
                            Description = c.Description
                        });

                    if (request.OfferTypeRecord != null)
                        query = OfferTypeServiceManager.ApplyFilter(query, request.OfferTypeRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);

                    query = request.PageSize > 0 ? ApplyPaging(query, request.PageSize, request.PageIndex) : ApplyPaging(query, request.DefaultPageSize, 0);

                    res.OfferTypeRecords = query.ToList();
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

        public static OfferTypeResponse DeleteOfferType(OfferTypeRequest request)
        {
            var res = new OfferTypeResponse();
            RunBase(request, res, (OfferTypeRequest req) =>
            {
                try
                {
                    var model = request.OfferTypeRecord;
                    var offerType = request._context.OfferTypes.FirstOrDefault(c => !c.Isdeleted && c.Id == model.Id);
                    if (offerType != null)
                    {
                        //update offerType IsDeleted
                        offerType.Isdeleted = true;
                        offerType.Modificationdate = DateTime.Now;
                        request._context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid offerType";
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

        public static OfferTypeResponse EditOfferType(OfferTypeRequest request)
        {
            var res = new OfferTypeResponse();
            RunBase(request, res, (OfferTypeRequest req) =>
            {
                try
                {
                    var model = request.OfferTypeRecord;
                    var offerType = request._context.OfferTypes.Find(model.Id);
                    if (offerType != null)
                    {
                        //update whole offerType
                        offerType = OfferTypeServiceManager.AddOrEditOfferType(request.BaseUrl, request.OfferTypeRecord, offerType);
                        request._context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid offerType";
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

        public static OfferTypeResponse AddOfferType(OfferTypeRequest request)
        {
            var res = new OfferTypeResponse();
            RunBase(request, res, (OfferTypeRequest req) =>
            {
                try
                {
                    var OfferTypeExist = request._context.OfferTypes.Any(m => m.Name.ToLower() == request.OfferTypeRecord.Name.ToLower() && !m.Isdeleted);
                    if (!OfferTypeExist)
                    {
                        var offerType = OfferTypeServiceManager.AddOrEditOfferType(request.BaseUrl, request.OfferTypeRecord);
                        request._context.OfferTypes.Add(offerType);
                        request._context.SaveChanges();
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "OfferType already exist";
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
    }
}