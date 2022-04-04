using LambdaHelper.Infrastructure;
using System.Linq;
using Xunit;

namespace LambdaHelper.Tests
{
    /// <summary>
    /// Test suite for <see cref="ExpressionHelper"/>.
    /// </summary>
    public class ExpressionHelperFilterTests
    {
        private readonly ExpressionHelper _expressionHelper;
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initialises an instance of <see cref="PasswordCheckTests"/>.
        /// </summary>
        public ExpressionHelperFilterTests()
        {
            _expressionHelper = new ExpressionHelper();
            _dbContext = new DbContext();
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "ExpressionHelper.TranslateFilterToLambda{T}(string)" />
        /// Search for Users those are matched to the given filter query by nested property value
        /// </summary>
        [Fact]
        public void TranslateFilterToLambda_ShouldReturnListOfT_WhenFilterQueryEqualOrGreaterThan()
        {
            //Arrange
            var filter = "Firstname eq Hossein _Or Age gt 5";

            //Act
            var predicate = _expressionHelper.TranslateFilterToLambda<Person>(filter);
            var result = _dbContext.People.Where(predicate);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.FirstName == "Hossein");
            Assert.Contains(result, x => x.FirstName == "Hossein" || x.Age > 5);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "ExpressionHelper.TranslateFilterToLambda{T}(string)" />
        /// Complie the given Filter query to a valid Lambda Expression that could execute on a Queryable object 
        /// </summary>
        [Fact]
        public void TranslateFilterToLambda_ShouldReturnListOfT_WhenFilterQueryWithAND()
        {
            //Arrange
            var filter = "Address.Number cn 1 _And Age gt 20";

            //Act
            var predicate = _expressionHelper.TranslateFilterToLambda<Person>(filter);
            var result = _dbContext.People.Where(predicate);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, x => x.Address.Number.Contains("1"));
            Assert.Contains(result, x => x.Address.Number.Contains("1") && x.Age > 20);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "ExpressionHelper.TranslateFilterToLambda{T}(string)" />
        /// Complie the given Filter query to a valid Lambda Expression that could execute on a Queryable object 
        /// </summary>
        [Fact]
        public void TranslateFilterToLambda_ShouldReturnListOfT_WhenFilterContainSpaceCharInsideValue()
        {
            //Arrange
            var filter = "Fullname cn 'Hossein S' _And Lastname eq SB";

            //Act
            var predicate = _expressionHelper.TranslateFilterToLambda<Person>(filter);
            var result = _dbContext.People.Where(predicate);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, x => x.FullName.Contains("Hossein S"));
            Assert.Contains(result, x => x.FullName.Contains("Hossein S") && x.LastName == "SB");
        }
    }
}
