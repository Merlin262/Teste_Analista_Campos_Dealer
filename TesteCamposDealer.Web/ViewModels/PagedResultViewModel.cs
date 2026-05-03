using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteCamposDealer.Web.ViewModels
{
    public class PagedResultViewModel<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }
}