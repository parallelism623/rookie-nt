using mvc_todolist.Commons.Helpers;
using System.Linq.Expressions;

namespace mvc_todolist.Commons
{
    public class QueryParameters<T>
    {
        public string Filter { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public string LogicFilter { get; set; } = string.Empty;
        public string Search { get; set; } = string.Empty;

        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;

        public Expression<Func<T, bool>> FilterExpression()
        {
           try{
               return ExpressionTreeHelper.GetFilterExpressionTree<T>(Filter, LogicFilter);
            }
            catch{
                return default!;
            }
        }
    }
}
