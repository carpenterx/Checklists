using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklists.Models
{
    public class ChecklistVariable
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public ChecklistVariable(string label, string value = "")
        {
            Label = label;
            Value = value;
        }
    }
}
