
using mvc_todolist.Commons.Helpers;
using System.Collections;
using System.Linq.Expressions;
using mvc_todolist.Models.Entities;
namespace mvc_todolist.Tests.Commons.DataSources
{
    public class PersonFilterTestCase : IEnumerable<object[]>
    {
        private string[] _filterByGender = new[] { "Gender==0", "Gender==1", "Gender==2", ""};
        private string[] _filterByBirthYear = new[] { "BirthYear==2000", "BirthYear>2000", "BirthYear<2000", ""};
        private Expression<Func<Person, bool>> _defaultExpression = p => true;
        public IEnumerator<object[]> GetEnumerator()
        {
            for (int i = 0; i < _filterByGender.Length; i++)
            {
                for (int j = 0; j < _filterByBirthYear.Length; j++)
                {
                    var condition = $"{_filterByGender[i]};{_filterByBirthYear[j]}";
                    if (condition == ";")
                    {
                        break;
                    }
                    yield return new[]
                    {
                        ExpressionTreeHelper.GetFilterExpressionTree<Person>(condition, "and")
                    };
                }
            }
            yield return new[] {
                _defaultExpression
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
