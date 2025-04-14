
using mvc_todolist.Commons;
using mvc_todolist.Models.Entities;
using System.Collections;

namespace mvc_todolist.Tests.Commons.DataSources
{
    public class RookiesControllerIndexResult : IEnumerable<(QueryParameters<Person>, int)>
    {
        public IEnumerator<(QueryParameters<Person>, int)> GetEnumerator()
        {
            yield return (QueryParametersFilterPersonByMaleGender(), 3);
            yield return (QueryParametersFilterPersonByFeMaleGender(), 4);
            yield return (QueryParametersFilterPersonByOtherGender(), 5);
            yield return (QueryParametersFilterPersonByRangeBirthYear(), 6);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private QueryParameters<Person> QueryParametersFilterPersonByMaleGender()
        {
            return new() { Filter = "Gender==0" };
        }
        private QueryParameters<Person> QueryParametersFilterPersonByFeMaleGender()
        {
            return new() { Filter = "Gender==1" };
        }
        private QueryParameters<Person> QueryParametersFilterPersonByOtherGender()
        {
            return new() { Filter = "Gender==2" };
        }

        private QueryParameters<Person> QueryParametersFilterPersonByRangeBirthYear()
        {
            return new() { Filter = "BirthYear>=1000;BirthYear<=3000" };
        }
    }
}
