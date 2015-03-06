using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraNext.Models
{
    public class Enrollment
    {
        public int? id { get; set; }
        public int? sessionId { get; set; }
        public bool isSigTrack { get; set; }
        public int courseId { get; set; }
        public bool active { get; set; }
        public int? startDate { get; set; }
        public int? endDate { get; set; }
        public string startStatus { get; set; }
    }
}
