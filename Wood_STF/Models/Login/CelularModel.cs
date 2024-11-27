using System;
using System.Collections.Generic;
using System.Text;

namespace Wood_STF.Models.Login
{
    public class CelularModel
    {
        public int ID { get; set; }
        public string IPCell { get; set; }
        public string NCell { get; set; }
        public DateTime FInicio { get; set; }
        public DateTime FFinalMarc { get; set; }
        public DateTime FFinalDesp { get; set; }
        public DateTime FFinalCarg { get; set; }

    }
}
