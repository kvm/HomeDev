using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraNext.Models
{
    public class ProfileInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
        public int? privacy { get; set; }
    }
}
