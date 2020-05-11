using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class SetFunctionModel
    {
        public string FUNCTION_ID { get; set; }
        public string FUNCTION_NAME { get; set; }
        public string FUNCTION_URL { get; set; }
        public string PARENT_ID { get; set; }
        public string SORT_ID { get; set; }
        public string IS_STOP { get; set; }
    }
}