using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class PostModel: _Model
    {
        public string artiClass { get; set; }
        public string artiHead{ get; set; }
        public string artiContent{ get; set; }
        public string tagContent{ get; set; }
        public IList<string> imgArray{ get; set; }

    }
}