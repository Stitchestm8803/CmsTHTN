using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsTHTN.Core.Modals
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Results { get; set; }
        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
