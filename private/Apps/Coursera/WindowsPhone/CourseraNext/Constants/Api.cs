using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraNext.Constants
{
    class Api
    {
        public const string CourseCatalogUrl = "https://api.coursera.org/api/catalog.v1/courses?ids={0}&fields=name,photo,language,shortDescription&includes=sessions";
    }
}
