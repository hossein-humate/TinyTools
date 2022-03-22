using LambdaHelper.Infrastructure;
using System;
using System.Linq;

namespace LambdaHelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start ...");

            var dbContext = new DbContext();
            var expressionHelper = new ExpressionHelper();

            //var filter = "Firstname cn Bo _Or Age gt 20 _Or Age lt 70 _And Lastname eq Bo _And Age ge 20 _Or Id eq 1";
            var orderBy = "Address.Number,firstName desc";
            var res = expressionHelper.ApplySort(dbContext.People, orderBy);

            var filter = "Address.Number cn 123 _And Age gt 20";
            //var filter = "age gt 20";
            var predicate = expressionHelper.TranslateFilterToLambda<Person>(filter);
            var result3 = dbContext.People.Where(predicate).ToList();

            Console.ReadLine();
        }
    }
}
