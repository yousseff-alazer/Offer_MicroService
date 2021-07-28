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
    public class OfferUserService : BaseService
    {
        public static OfferUserResponse ListOfferUser(OfferUserRequest request)
        {
            var res = new OfferUserResponse();
            RunBase(request, res, (OfferUserRequest req) =>
             {
                 try
                 {
                     var query = request._context.OfferUsers.Where(c => !c.Isdeleted).Select(c => new OfferUserRecord
                     {
                         Id = c.Id,
                         Offerid = c.Offerid,
                         UserId = c.UserId
                     });

                     if (request.OfferUserRecord != null)
                         query = OfferUserServiceManager.ApplyFilter(query, request.OfferUserRecord);

                     res.TotalCount = query.Count();

                     query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);

                     if (request.PageSize > 0)
                         query = ApplyPaging(query, request.PageSize, request.PageIndex);

                     res.OfferUserRecords = query.ToList();
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

        public static OfferUserResponse DeleteOfferUser(OfferUserRequest request)
        {
            var res = new OfferUserResponse();
            RunBase(request, res, (OfferUserRequest req) =>
             {
                 try
                 {
                     var model = request.OfferUserRecord;
                     var offerUser = request._context.OfferUsers.FirstOrDefault(c => !c.Isdeleted && c.Id == model.Id);
                     if (offerUser != null)
                     {
                         //update offerUser IsDeleted
                         offerUser.Isdeleted = true;
                         offerUser.Modificationdate = DateTime.Now;
                         request._context.SaveChanges();

                         res.Message = HttpStatusCode.OK.ToString();
                         res.Success = true;
                         res.StatusCode = HttpStatusCode.OK;
                     }
                     else
                     {
                         res.Message = "Invalid offerUser";
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

        public static OfferUserResponse EditOfferUser(OfferUserRequest request)
        {
            var res = new OfferUserResponse();
            RunBase(request, res, (OfferUserRequest req) =>
            {
                try
                {
                    var model = request.OfferUserRecord;
                    var offerUser = request._context.OfferUsers.Find(model.Id);
                    if (offerUser != null)
                    {
                        //update whole offerUser
                        offerUser = OfferUserServiceManager.AddOrEditOfferUser(request.BaseUrl/*, request.CreatedBy*/, request.OfferUserRecord, offerUser);
                        request._context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid offerUser";
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

        public static OfferUserResponse AddOfferUser(OfferUserRequest request)
        {
            var res = new OfferUserResponse();
            RunBase(request, res, (OfferUserRequest req) =>
            {
                try
                {
                    var OfferUserExist = request._context.OfferUsers.Any(m => m.UserId == request.OfferUserRecord.UserId && m.Offerid == request.OfferUserRecord.Offerid && !m.Isdeleted);
                    if (!OfferUserExist)
                    {
                        var offerUser = OfferUserServiceManager.AddOrEditOfferUser(request.BaseUrl/*, request.CreatedBy*/, request.OfferUserRecord);
                        request._context.OfferUsers.Add(offerUser);
                        request._context.SaveChanges();
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "OfferUser already exist";
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