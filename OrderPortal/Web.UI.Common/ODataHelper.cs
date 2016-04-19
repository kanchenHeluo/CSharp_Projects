using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Security.Cryptography;
using System.Data.Services.Client;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Web.UI.Common
{
    public static class ODataHelper
    {     
        public static DataServiceQuery<T> ODataRestQuery<T>(DataServiceContext entities, IDictionary<string, IEnumerable> filters)
        {
            var dataServiceQuery = entities.CreateQuery<T>(typeof(T).Name).ODataFilter<T>(filters);
            return dataServiceQuery;
        }

        public static DataServiceQuery<T> ODataFilter<T>(this DataServiceQuery<T> query, IDictionary<string, IEnumerable> filters)
        {
            //Filter predicate for generating query
            Expression filterPredicate = null;

            //List of expression joined with 'and' clause
            var expressions = new List<Expression>();

            //Expression for an entity type
            var param = Expression.Parameter(typeof(T), "entity");

            //Looping filters and generating 'or' filters for query
            foreach (KeyValuePair<string, IEnumerable> filter in filters)
            {
                //Expression containing the 'or' criteria
                Expression filterExpression = null;

                //Left side of the filter expression
                var left = Expression.PropertyOrField(param, filter.Key);

                //Build Dynamic 'or' linq query
                foreach (var id in filter.Value)
                {
                    //Build a comparision expression
                    Expression comparison = Expression.Equal(left, Expression.Convert(Expression.Constant(id), left.Type));

                    //Add this to the complete Filter Expression with 'or' operator
                    filterExpression = (filterExpression == null) ? comparison : Expression.Or(filterExpression, comparison);
                }

                //Adding all the 'or' expression into the list
                expressions.Add(filterExpression);
            }

            //Looping expression to create 'and' expression
            foreach (Expression expression in expressions)
            {
                //Adding expression with 'and' clause
                filterPredicate = (filterPredicate == null) ? expression : Expression.And(filterPredicate, expression);
            }

            //Convert the Filter Expression into a Lambda expression of type Func<Lists,bool>
            var filterLambdaExpression = Expression.Lambda<Func<T, bool>>(filterPredicate, param);

            //Creating DataServiceQuery with above filter lambda expression
            return (DataServiceQuery<T>)query.Where<T>(filterLambdaExpression);
        }


    }
}
