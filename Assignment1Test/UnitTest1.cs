using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assignment1;

namespace Assignment1Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InstructorCanCreateCourseTest()
        {
            /*
             Start with the known instructor id then find out how many courses this instructor is teaching right now
             Call that course number N
             Create a new course for this instructor using the application code from test
             Again, Find out how many courses this instructor is teaching now
             Expecting N + 1
             (Started with N courses and added 1 course)
            */
            Cours c = new Cours();
            c.course_name = "ABC";

        }
    }
}
