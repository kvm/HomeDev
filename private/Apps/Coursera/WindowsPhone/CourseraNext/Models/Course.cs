using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraNext.Models
{
    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string photo { get; set; }
        public string smallIconHover { get; set; }
        public string language { get; set; }
        public string shortDescription { get; set; }
        public List<string> sessionIds { get; set; }
        public string courseHomeLink { get; set; }
    }
}
