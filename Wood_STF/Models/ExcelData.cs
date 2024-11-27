using System;
using System.Collections.Generic;
using System.Text;

namespace Wood_STF.Models
{
    public class ExcelData
    {
        public List<string> Headers { get; set; } = new List<string>();
        public List<List<string>> Values { get; set; } = new List<List<string>>();
    }
}
