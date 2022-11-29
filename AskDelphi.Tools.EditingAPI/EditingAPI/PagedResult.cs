using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
{
    public class PagedResult<T> where T : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Result { get; set; }
        public int TotalAvailable { get; set; }
    }
}
