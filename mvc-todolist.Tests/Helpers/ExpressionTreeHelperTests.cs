
using FluentAssertions;
using mvc_todolist.Commons.Helpers;
using System.Linq.Expressions;
using mvc_todolist.Models.Entities;
namespace mvc_todolist.Tests.HelperTest
{
    [TestFixture]
    public class GetFilterExpressionTreeTests
    {
        [Test]
        public void GetFilterExpressionTree_WithNullCondition_ThrowsArgumentException()
        {
            // Arrange
            // Action
            Action act = () => ExpressionTreeHelper.GetFilterExpressionTree<Person>(null, "and");

            // Assert

            act.Should().Throw<ArgumentException>().WithMessage("Condition null");
        }

        [Test]
        public void GetFilterExpressionTree_WithSingleCondition_ReturnsValidFilter()
        {
            // Arrange
            string condition = "Age>=10";
            // Action
            Expression<Func<Person, bool>> expr =
                ExpressionTreeHelper.GetFilterExpressionTree<Person>(condition, "and");
            // Assert
            Func<Person, bool> filter = expr.Compile();

            filter(new Person { Age = 15,  BirthYear = 2000 }).Should().BeTrue();
            filter(new Person { Age = 5, BirthYear = 2000 }).Should().BeFalse();
        }

        [Test]
        public void GetFilterExpressionTree_WithMultipleConditionsAndJoin_ReturnsValidFilter()
        {
            // Arrange
            string condition = "Age>=10;BirthYear==2000";
            string join = "and";

            // Action
            Expression<Func<Person, bool>> expr =
                ExpressionTreeHelper.GetFilterExpressionTree<Person>(condition, join);
            Func<Person, bool> filter = expr.Compile();

            // Assert
            filter(new Person { Age = 15, BirthYear = 2000 }).Should().BeTrue();
            filter(new Person { Age = 5, BirthYear = 2000 }).Should().BeFalse();
            filter(new Person { Age = 15, BirthYear = 1990 }).Should().BeFalse();
        }

        [Test]
        public void GetFilterExpressionTree_WithMultipleConditionsOrJoin_ReturnsValidFilter()
        {
            // Arrange
            string condition = "Age>=10;BirthYear==2000";
            string join = "or";

            // Action
            Expression<Func<Person, bool>> expr =
                ExpressionTreeHelper.GetFilterExpressionTree<Person>(condition, join);
            Func<Person, bool> filter = expr.Compile();

            // Assert
            filter(new Person { Age = 15, BirthYear = 1990 }).Should().BeTrue();
            filter(new Person { Age = 5, BirthYear = 2000 }).Should().BeTrue();
            filter(new Person { Age = 5, BirthYear = 1990 }).Should().BeFalse();
        }

        [Test]
        public void GetFilterExpressionTree_WithInvalidCondition_ThrowsException()
        {
            // Arrange
            string condition = "Age>=10;InvalidCondition";

            // Action
            Action act = () => ExpressionTreeHelper.GetFilterExpressionTree<Person>(condition, "and");

            // Assert
            act.Should().Throw<Exception>().WithMessage("Invalid condition: InvalidCondition");
        }


        [Test]
        public void GetFilterExpressionTree_WithNotExistsPropertiesInCondition_ThrowsException()
        {
            // Arrange
            string condition = "Age>=10;AgeNumber>=100";

            // Action
            Action act = () => ExpressionTreeHelper.GetFilterExpressionTree<Person>(condition, "and");

            // Assert
            act.Should().Throw<Exception>().WithMessage("Invalid condition: AgeNumber>=100");
        }
    }
}
