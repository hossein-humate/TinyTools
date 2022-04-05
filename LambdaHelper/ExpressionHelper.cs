using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LambdaHelper
{
    // |---------------------------------------------------------------------|
    // |Humate Conditional operators to filter                               |
    // |---------------------------------------------------------------------|
    // |Examples for &filter={value}    to          C# compiled syntax       |
    // |---------------------------------------------------------------------|
    // |&filter=prop eq 2               |     x => x.prop == 2               |
    // |&filter=prop lt 2               |     x => x.prop > 2                |
    // |&filter=prop gt 2               |     x => x.prop < 2                |
    // |&filter=prop le 2               |     x => x.prop >= 2               |
    // |&filter=prop ge 2               |     x => x.prop <= 2               |
    // |&filter=prop cn Hossein         |     x => x.prop.Contains("Hossein")|
    // |&filter=prop cn 'John D'        |     x => x.prop.Contains("John D") |
    // |&filter=prop.nest eq 2          |     x => x.prop.nest == 2          |
    // |---------------------------------------------------------------------|

    // |----------------------------------------------------------------------------|
    // |Humate Conditional operators to orderby                                     |
    // |----------------------------------------------------------------------------|
    // |Examples for &orderby={value}    to          C# compiled syntax             |
    // |----------------------------------------------------------------------------|
    // |&orderby=prop                   |     source.OrderBy(x => x.prop)           |
    // |&orderby=prop desc              |     source.OrderByDescending(x => x.prop) |
    // |----------------------------------------------------------------------------|
    /// <summary>
    /// Exention class to provide Expression helper for Filter or Sort Order operation and Query.
    /// </summary>
    public class ExpressionHelper
    {
        private const string label = "x";

        ///<remarks>
        /// Note: This method result used for IQueryable functions predicate
        ///</remarks>
        /// <inheritdoc cref="FilterToLambda{T}(string)"/>
        public Func<T, bool> TranslateFilterToLambda<T>(string filter)
        {
            try
            {
                var lambda = FilterToLambda<T>(filter);
                return lambda.Compile();
            }
            catch (Exception)
            {
                throw new Exception("Cannot compile your request.");
            }
        }

        /// <summary>
        /// Extract a Lambda Expression from filter query
        /// To see the valid filter check this filter value below and consider the <see cref="T"/>
        /// generic type for Organisation model:
        /// filter = "Name cn Hossein _Or  gt 20 _Or Size lt 70 _And BillingEmail eq test@test.com _And Size gt 20 _Or OrganisationId eq 1"
        /// <br/><br/>
        /// The filter query started with 'Name' as parameter and use 'cn' as function to find 'Hossein' as value. 
        /// The first part of the filter query compile as C# Lambda: <code>x => x.Name.Contains("Hossein")</code>
        /// The second part of the filter query compile as C# Lambda: <code>x => x.Size > 20</code>
        /// Consider the first part as A and second one as B, to apply multiple conditional filter you need to use
        /// '_And' or '_Or' keywords beetwen A and B query parts.
        /// For example A _Or B Compile as:<code>x => x.Name.Contains("Hossein") || x.Size > 20 </code>
        /// </summary>
        /// <param name="filter">Represent the Filter query on each and every parameters of <see cref="T"/> generic type</param>
        /// <remarks>
        /// </remarks>
        /// <returns>Return a <see cref="Expression{Func{T, bool}}"/> result as a Lambda expression queryable</returns>
        /// <exception cref="Exception">Throw an exception with message 'Cannot compile your request.' or
        /// 'Cannot create Expression form filter extraction.' if the provided filter query not match the reference
        /// type or value type in the <see cref="T"/> generic object.</exception>
        public Expression<Func<T, bool>> FilterToLambda<T>(string filter)
        {
            try
            {
                var parameterExpression = Expression.Parameter(typeof(T), label);
                Expression expression = string.IsNullOrEmpty(filter) ? Expression.Constant(true) : default;
                if (!string.IsNullOrEmpty(filter))
                {
                    var ands = filter.Split("_And");
                    List<string> ors = new(), allExp = new();
                    foreach (var andQuery in ands)
                    {
                        var orArray = andQuery.Split("_Or");
                        if (orArray.Length > 1)
                        {
                            foreach (var orQuery in orArray)
                            {
                                var lambdaExpression = GetExpression<T>(parameterExpression, orQuery);
                                expression = expression == null ? lambdaExpression :
                                    Expression.Or(lambdaExpression, expression);
                            }
                        }
                        else
                        {
                            var lambdaExpression1 = GetExpression<T>(parameterExpression, andQuery);
                            expression = expression == null ? lambdaExpression1 :
                                Expression.And(lambdaExpression1, expression);
                        }
                    }

                    if (expression == null)
                        throw new Exception("Cannot create Expression form filter extraction.");
                }
                return Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
            }
            catch (Exception)
            {
                throw new Exception("Cannot compile your request.");
            }
        }

        /// <summary>
        /// Translate the orderby query string to the Lambda Expression OrderBy ASC or DESC depend on flag
        /// To see the valid orderby check this orderBy value below and consider the <see cref="T"/>
        /// generic type for Organisation model:
        /// orderBy = "OrganisationId,Name desc,CreateDate"
        /// <br/><br/>
        /// The orderby query started with 'Name' as parameter and use Ascending as defualt sorting functionality.
        /// You can also add 'desc' in front of the parameter name to change sorting to the Descending.
        /// The first part of the orderby query compile as C# Lambda: <code>lastOrderedSource = source.OrderBy(x => x.OrganisationId)</code>
        /// The second part of the orderby query compile as C# Lambda: <code>lastOrderedSource = lastOrderedSource.ThenByDescending(x => x.Name)</code>
        /// The third part of the orderby query compile as C# Lambda: <code>lastOrderedSource = lastOrderedSource.ThenBy(x => x.CreateDate)</code>
        /// </summary>
        /// <param name="source">Represent the List of <see cref="T"/> Entity generic type</param>
        /// <param name="orderBy">Represent the OrderBy query on each or every parameters of <see cref="T"/> generic type</param>
        /// <remarks>
        /// </remarks>
        /// <returns>Return a <see cref="List{T}"/> result as a ordered list</returns>
        /// <exception cref="Exception">Throw an exception with message 'Cannot process your null or empty OrderBy value.' 
        /// if the orderby value is null or empty. 'Cannot process your OrderBy query.' if the provided parameters
        /// in orderby query not match the reference type or if the 'desc' flag is not exist with wrong value.</exception>
        public List<T> ApplySort<T>(IList<T> source, string orderBy)
        {
            List<T> result = source.ToList();
            if (!string.IsNullOrEmpty(orderBy) && source.Any())
            {
                IOrderedEnumerable<T> ordered = default;
                var properties = orderBy.Split(',');
                if (properties.Length > 1)
                {
                    bool isFirst = true;
                    foreach (var property in properties)
                    {
                        var parts = property.Split(' ');
                        var propertyInfo = typeof(T).GetProperty(parts[0].ToLower(), BindingFlags.IgnoreCase
                            | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo == null)
                        {
                            throw new Exception("Cannot process your OrderBy query.");
                        }

                        if (parts.Length > 1)
                        {
                            if (parts[1].ToLower() == "desc")
                            {
                                ordered = isFirst ? source.OrderByDescending(x => propertyInfo.GetValue(x, null))
                                    : ordered?.ThenByDescending(x => propertyInfo.GetValue(x, null));
                                isFirst = false;
                            }
                            else
                            {
                                throw new Exception("Cannot process your OrderBy query.");
                            }
                        }
                        else
                        {
                            ordered = isFirst ? source.OrderBy(x => propertyInfo.GetValue(x, null))
                                    : ordered?.ThenBy(x => propertyInfo.GetValue(x, null));
                            isFirst = false;
                        }
                    }
                }
                else
                {
                    var parts = properties[0].Split(' ');
                    var propertyInfo = typeof(T).GetProperty(parts[0].ToLower(), BindingFlags.IgnoreCase
                           | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        throw new Exception("Cannot process your OrderBy query.");
                    }

                    if (parts.Length > 1)
                    {
                        if (parts[1].ToLower() == "desc")
                        {
                            ordered = source.OrderByDescending(x => propertyInfo.GetValue(x, null));
                        }
                        else
                        {
                            throw new Exception("Cannot process your OrderBy query.");
                        }
                    }
                    else
                    {
                        ordered = source.OrderBy(x => propertyInfo.GetValue(x, null));
                    }
                }
                result = ordered.ToList();
            }
            return result;
        }

        private Expression GetExpression<T>(ParameterExpression paramExpression, string filterQuery)
        {
            try
            {
                string propertyName, conditionalOperator, constantvalue;
                if (filterQuery.Trim().Contains("'"))
                {
                    // FilerQuery contain for example: FullName cn 'Hossein SB'
                    var quotes = filterQuery.Substring(filterQuery.IndexOf("'"), filterQuery.LastIndexOf("'") - filterQuery.IndexOf("'") + 1);
                    var propertyAndOperator = filterQuery.Trim().Split(quotes);
                    if (propertyAndOperator.Length != 2)
                        throw new Exception("String value in filter expression has not correct format.");
                    var filterParts = propertyAndOperator[0].Trim().Split(' ');
                    if (filterParts.Length != 2)
                        throw new Exception("Filter expression is not correct.");
                    propertyName = filterParts[0];
                    conditionalOperator = filterParts[1];
                    constantvalue = quotes[1..quotes.LastIndexOf("'")];
                }
                else
                {
                    // FilerQuery contain for example: Firstname eq Hossein
                    var filterParts = filterQuery.Trim().Split(' ');
                    if (filterParts.Length != 3)
                        throw new Exception("Filter expression is not correct.");
                    propertyName = filterParts[0];
                    conditionalOperator = filterParts[1];
                    constantvalue = filterParts[2];
                }

                //Catch nested Entity as parameter in query
                string[] nestedProps = propertyName.Split('.');
                Expression memberExpression = paramExpression;
                PropertyInfo propInfo = default;
                Type type = typeof(T);
                if (nestedProps.Length > 1)
                {
                    for (int i = 0; i < nestedProps.Length; i++)
                    {
                        propInfo = GetPropertyInfo(type, nestedProps[i]);
                        type = propInfo.PropertyType;
                        memberExpression = Expression.PropertyOrField(memberExpression, nestedProps[i]);
                    }
                }
                else
                {
                    propInfo = GetPropertyInfo(type, propertyName);
                    memberExpression = Expression.Property(paramExpression, propertyName);
                }

                ConstantExpression constant = GetConstantExpression(propInfo, constantvalue);
                if (constant == null)
                {
                    throw new Exception("Cannot process your request.");
                }

                BinaryExpression expression = GetBinaryExpression(
                    memberExpression, constant, propInfo, conditionalOperator);
                if (expression == null)
                    throw new Exception("Cannot process your request.");

                return expression;
            }
            catch (Exception)
            {
                throw new Exception("Cannot process your request.");
            }
        }

        private static ConstantExpression GetConstantExpression(PropertyInfo propInfo, string constantvalue)
        {
            try
            {
                ConstantExpression constant = default;
                if (propInfo != null)
                    switch (Type.GetTypeCode(propInfo.PropertyType))
                    {
                        case TypeCode.Boolean:
                            constant = Expression.Constant(bool.Parse(constantvalue));
                            break;
                        case TypeCode.Byte:
                            constant = Expression.Constant(byte.Parse(constantvalue));
                            break;
                        case TypeCode.Decimal:
                            constant = Expression.Constant(decimal.Parse(constantvalue));
                            break;
                        case TypeCode.Double:
                            constant = Expression.Constant(double.Parse(constantvalue));
                            break;
                        case TypeCode.Int16:
                            constant = Expression.Constant(short.Parse(constantvalue));
                            break;
                        case TypeCode.Int32:
                            constant = Expression.Constant(int.Parse(constantvalue));
                            break;
                        case TypeCode.Int64:
                            constant = Expression.Constant(long.Parse(constantvalue));
                            break;
                        case TypeCode.DateTime:
                            constant = Expression.Constant(DateTime.Parse(constantvalue));
                            break;
                        case TypeCode.Object:
                            if (propInfo.PropertyType == typeof(Guid) || propInfo.PropertyType == typeof(Guid?))
                            {
                                constant = Expression.Constant(Guid.Parse(constantvalue));
                            }
                            break;
                        default:
                            constant = Expression.Constant(constantvalue);
                            break;
                    }
                return constant;
            }
            catch (Exception)
            {
                throw new Exception("Value type cannot parse to the Parameter type.");
            }
        }

        private static BinaryExpression GetBinaryExpression(
            Expression property,
            ConstantExpression constant,
            PropertyInfo propInfo,
            string conditionalOperator)
        {
            BinaryExpression expression;
            switch (conditionalOperator.ToLower())
            {
                case "eq":
                    expression = Expression.Equal(property, constant);
                    break;
                case "lt":
                    expression = Expression.MakeBinary(ExpressionType.LessThan, property, constant);
                    break;
                case "gt":
                    expression = Expression.MakeBinary(ExpressionType.GreaterThan, property, constant);
                    break;
                case "le":
                    expression = Expression.MakeBinary(ExpressionType.LessThanOrEqual, property, constant);
                    break;
                case "ge":
                    expression = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, property, constant);
                    break;
                case string opt when opt == "cn" && propInfo != null:
                    if (Type.GetTypeCode(propInfo.PropertyType) == TypeCode.String)
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) })
                            ?? throw new Exception("Cannot process your request.");
                        var containsMethodExp = Expression.Call(property, method, constant);
                        expression = Expression.Equal(containsMethodExp, Expression.Constant(bool.Parse("true")));
                    }
                    else
                    {
                        throw new Exception("Only String type can use contain filter functionality.");
                    }
                    break;
                default:
                    throw new Exception("Cannot process your request.");
            }
            return expression;
        }

        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            return type.GetProperty(propertyName, BindingFlags.IgnoreCase |
                            BindingFlags.Public | BindingFlags.Instance)
                            ?? throw new Exception("Cannot process your request.");
        }
    }
}
