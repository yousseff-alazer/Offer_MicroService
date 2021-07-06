using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Offer.CommonDefinitions.Requests;
using Offer.CommonDefinitions.Responses;
using Offer.DAL.DB;
using Offer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Offer.BL.Services
{
    public class BaseService
    {
        protected const string IDColumn = "Id";
        protected const string OrderByCommand = "OrderBy";
        protected const string OrderByDescCommand = "OrderByDescending";

        protected static IQueryable<Q> OrderByDynamic<Q>(IQueryable<Q> query, string orderByColumn, bool isDesc)
        {
            var QType = typeof(Q);
            // Dynamically creates a call like this: query.OrderBy(p => p.SortColumn)
            var parameter = Expression.Parameter(QType, "p");
            Expression resultExpression = null;
            var property = QType.GetProperty(orderByColumn ?? IDColumn);
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            // this is the part p => p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), isDesc ? OrderByDescCommand : OrderByCommand,
                new[] { QType, property.PropertyType }, query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<Q>(resultExpression);
        }

        protected static IQueryable<Q> ApplyPaging<Q>(IQueryable<Q> query, int pageSize, int pageIndex)
        {
            var skipedPages = pageSize * pageIndex;
            query = query.Skip(skipedPages).Take(pageSize);
            return query;
        }

        public static RS RunBase<RQ, RS>(RQ request, RS response, Func<RQ, RS> myMethod)
            where RQ : BaseRequest
            where RS : BaseResponse
        {
            try
            {
                var methodName = myMethod.Method.Name;
                var serviceName = methodName.Substring(0, methodName.LastIndexOf(">")).Replace('<', ' ').Trim();

                //check if user role can access this service
                var canAccess = true;

                if (!canAccess)
                {
                    response.Message = "Not allowed";
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.MethodNotAllowed;
                    return response;
                }

                response = myMethod(request);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return response;
        }

        public static DbContextOptions<OfferDbContext> GetDbContextConnectionOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<OfferDbContext>().UseNpgsql(connectionString).Options;
        }
    }
}