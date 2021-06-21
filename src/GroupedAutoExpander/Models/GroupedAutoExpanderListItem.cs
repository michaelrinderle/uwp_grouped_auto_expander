using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupedAutoExpander.Models
{
    public class GroupedAutoExpanderListItem
    {
        public string ItemID { get; internal set; }
        public string HeaderName { get; set; }
        public bool IsExpanded { get; set; }
    }
}
