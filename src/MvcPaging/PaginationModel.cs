using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcPaging
{  
    public class PaginationModel
    {
        public bool Active { get; set; }

        public bool IsCurrent { get; set; }

        public int? PageIndex { get; set; }

        public string DisplayText { get; set; }

    }
}
