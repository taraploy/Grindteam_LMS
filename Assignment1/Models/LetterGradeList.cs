using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public static class LetterGradeList
    {
        public static List<LetterGradeItem> LetterGradeItems;
        public static int PercentA;
        public static int PercentB;
        public static int PercentC;
        public static int PercentD;
        public static int PercentF;
        public static int PercentUngraded;
        public static double HighScore;
        public static double LowScore;


        public static void GenerateCourseLetterGrades(int courseId)
        {

            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            LetterGradeItems = new List<LetterGradeItem>();
            PercentA = new int();
            PercentB = new int();
            PercentC = new int();
            PercentD = new int();
            PercentF = new int();
            PercentUngraded = new int();
            int totalStudents = 0;

            var query = (from s in gds.StudentCourses
                         join ic in gds.InstructorCourses on s.course_id equals ic.course_id
                         where ic.course_id == courseId
                         select new
                         {
                             InstructorCourseId = ic.instructor_course_id,
                             CourseId = ic.course_id,
                             StudentId = s.student_id,
                             LetterGrade = s.letter_grade
                         }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                LetterGradeItems.Add(new LetterGradeItem());
                AssignmentList.GenerateThisStudentsSubmissions(item.StudentId);
                //LetterGradeItems[i].totalPoints = AssignmentList.IndividualStudentTotalPoints;
                //LetterGradeItems[i].maxPoints = AssignmentList.IndividualStudentMaxPoints;
                LetterGradeItems[i].courseId = item.CourseId;
                LetterGradeItems[i].studentId = item.StudentId;
                LetterGradeItems[i].letterGrade = item.LetterGrade;

                if (item.LetterGrade == "A") PercentA++;
                else if (item.LetterGrade == "B") PercentB++;
                else if (item.LetterGrade == "C") PercentC++;
                else if (item.LetterGrade == "D") PercentD++;
                else if (item.LetterGrade == "F") PercentF++;
                else PercentUngraded++; totalStudents++;
            }
        }

        public static void GenerateStudentAssignmentGradePerformance(int assignmentId)
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            LetterGradeItems = new List<LetterGradeItem>();
            List<int> PointList = new List<int>();
            HighScore = new int();
            LowScore = new int();

            var query = (from a in gds.Assignments
                         join sa in gds.StudentAssignments on a.assignment_id equals sa.assignment_id
                         where a.assignment_id == assignmentId
                         select new
                         {
                             Grade = sa.grade
                         }).ToList();
            foreach (var item in query)
            {
                PointList.Add((int)item.Grade);
            }

            //Calculate chart values
            HighScore = PointList[0];
            LowScore = PointList[0];
            for (int j = 0; j < PointList.Count; j++)
            {
                if (PointList[j] > HighScore)
                {
                    HighScore = PointList[j];
                }
                else if (PointList[j] < LowScore)
                {
                    LowScore = PointList[j];
                }
            }

            /*double[] pointArray = PointList.ToArray();
            Q3 = Percentile(pointArray, 75);
            Median = Percentile(pointArray, 50);
            Q1 = Percentile(pointArray, 25);*/

        }

        /*public static double Percentile(double[] sortedData, double p)
        {
            // algo derived from Aczel pg 15 bottom
            if (p >= 100.0d) return sortedData[sortedData.Length - 1];

            double position = (sortedData.Length + 1) * p / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;

            double n = p / 100.0d * (sortedData.Length - 1) + 1.0d;

            if (position >= 1)
            {
                leftNumber = sortedData[(int)Math.Floor(n) - 1];
                rightNumber = sortedData[(int)Math.Floor(n)];
            }
            else
            {
                leftNumber = sortedData[0]; // first data
                rightNumber = sortedData[1]; // first data
            }

            //if (leftNumber == rightNumber)
            if (Equals(leftNumber, rightNumber))
                return leftNumber;
            double part = n - Math.Floor(n);
            return leftNumber + part * (rightNumber - leftNumber);
        } // end of internal function percentile*/
    }
}