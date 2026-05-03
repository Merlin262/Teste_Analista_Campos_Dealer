using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCamposDealer.Application.Common
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);

        public PagedResult<TOut> Map<TOut>(Func<T, TOut> mapper) => new PagedResult<TOut>
        {
            Data = Data.Select(mapper).ToList(),
            Page = Page,
            PageSize = PageSize,
            Total = Total
        };
    }
}
