using System;
using System.Collections.Generic;

namespace Better4You.ViewModel
{
    public class RecordInfoView
    {
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string Created { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string Modified { get; set; }
        public string ModifiedReason { get; set; }
        public KeyValuePair<long,string> RecordStatus { get; set; } 
    }
}
