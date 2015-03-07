using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraNext
{
    public class DummyDataSource
    {
        public static ViewModels.CoursesPageViewModel GetCoursesDummyData()
        {
            string courses = "[{\"id\":2,\"name\":\"Machine Learning\",\"shortName\":\"ml\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/ml/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/ml/small-icon.hover.png\"},{\"id\":26,\"name\":\"Algorithms, Part I\",\"shortName\":\"algs4partI\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/algs4partI/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/algs4partI/small-icon.hover.png\"},{\"id\":45,\"name\":\"A History of the World since 1300\",\"shortName\":\"wh1300\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/wh1300/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/wh1300/small-icon.hover.png\"},{\"id\":57,\"name\":\"Introduction to Astronomy\",\"shortName\":\"introastro\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/introastro/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/introastro/small-icon.hover.png\"},{\"id\":58,\"name\":\"Think Again: How to Reason and Argue\",\"shortName\":\"thinkagain\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/thinkagain/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/thinkagain/small-icon.hover.png\"},{\"id\":66,\"name\":\"Learn to Program: Crafting Quality Code\",\"shortName\":\"programming2\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/programming2/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/programming2/small-icon.hover.png\"},{\"id\":124,\"name\":\"Introduction to Mathematical Thinking\",\"shortName\":\"maththink\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/maththink/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/maththink/small-icon.hover.png\"},{\"id\":128,\"name\":\"Algorithms: Design and Analysis, Part 2\",\"shortName\":\"algo2\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/algo2/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/algo2/small-icon.hover.png\"},{\"id\":131,\"name\":\"Creative, Serious and Playful Science of Android Apps\",\"shortName\":\"androidapps101\",\"photo\":\"https://s3.amazonaws.com/coursera/topics/androidapps101/large-icon.png\",\"smallIconHover\":\"https://d1z850dzhxs7de.cloudfront.net/topics/androidapps101/small-icon.hover.png\"},{\"id\":171,\"name\":\"Machine Learning\",\"shortName\":\"machlearning\",\"photo\":\"https://coursera-course-photos.s3.amazonaws.com/23/3dbe00f0cfefe0396ac6976a242385/machinelearninglogo.jpeg\",\"smallIconHover\":\"https://d15cw65ipctsrr.cloudfront.net/c7/85dbfadc3808b314c25be1cb54f37a/machinelearninglogo.jpg\"},{\"id\":278,\"name\":\"Social Psychology\",\"shortName\":\"socialpsychology\",\"photo\":\"https://coursera-course-photos.s3.amazonaws.com/dc/99ebd7aa52f3c39d32e97ca6e3a66c/Course-Image3200x1800.png\",\"smallIconHover\":\"https://d15cw65ipctsrr.cloudfront.net/e1/d3059dd666773914baa9d41fc13183/Course-Image3200x1800.png\"},{\"id\":566,\"name\":\"Diabetes: Diagnosis, Treatment, and Opportunities\",\"shortName\":\"ucsfdiabetes\",\"photo\":\"https://coursera-course-photos.s3.amazonaws.com/8b/67266287bcf7ee9649e98799ec4dc4/iStock_000011432889Small.jpg\",\"smallIconHover\":\"https://d15cw65ipctsrr.cloudfront.net/d3/043b263cc3da09fb0713ae8b4f7720/iStock_000011432889Small.jpg\"},{\"id\":1195,\"name\":\"The Brain and Space\",\"shortName\":\"brainspace\",\"photo\":\"https://coursera-course-photos.s3.amazonaws.com/51/021214d4173c4d935e0341a36e914f/brain_icon.jpg\",\"smallIconHover\":\"https://d15cw65ipctsrr.cloudfront.net/5f/f01ef17fb6061a6c0e6f0f9661a883/brain_icon.jpg\"},{\"id\":1298,\"name\":\"The Data Scientist’s Toolbox\",\"shortName\":\"datascitoolbox\",\"photo\":\"https://coursera-course-photos.s3.amazonaws.com/37/6352a069b511e3ae92c39913bb30e0/DataScientistsToolbox.jpg\",\"smallIconHover\":\"https://d15cw65ipctsrr.cloudfront.net/3b/00769069b511e3af9a07984d9d273b/DataScientistsToolbox.jpg\"}]";

            var courseList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CourseraNext.Models.Course>>(courses);

            var viewModel = new ViewModels.CoursesPageViewModel();
            viewModel.courses = courseList;

            // clean image urls
            //foreach (var course in viewModel.courses)
            //{
            //    course.photo = course.photo.Replace(\"\\\", \"\");
            //}

            return viewModel;
        }
    }
}
