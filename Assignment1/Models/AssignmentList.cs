using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public static class AssignmentList
    {

        public static List<AssignmentItem> AssignmentItemList;

        public static List<StudentAssignmentItem> StudentAssignments;

        public static StudentAssignmentSubmissionItem StudentAssignmentSubmission;

        public static List<StudentAssignmentSubmissionItem> AllStudentSubmissions;

        public static List<StudentAssignmentSubmissionItem> ThisStudentsSubmissions;

        public static AssignmentItem AssignmentItem;

        public static int IndividualStudentTotalPoints;
        public static int IndividualStudentMaxPoints;


        //this method generates all of the assignments by a particular instructor
        //Generates a list of assignmentItems (targeted for instructor views)
        public static void GenerateInstructorAssignmentList(int id)
        {
            AssignmentItemList = new List<AssignmentItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            var query = (from a in gds.Assignments
                         join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join c in gds.Courses on ic.course_id equals c.course_id
                         where ic.instructor_id == Name.user_id && ic.course_id == id
                         select new
                         {
                             AssignmentId = a.assignment_id,
                             InstructorCourseId = a.instructor_course_id,
                             AssignmentName = a.assignment_name,
                             AssignmentDesc = a.assignment_desc,
                             AssignmentType = a.assignment_type,
                             MaxPoints = a.max_points,
                             DueDate = a.due_date,
                             CourseId = c.course_id,
                             CourseNum = c.course_num,
                             CourseName = c.course_name
                         }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                AssignmentItemList.Add(new AssignmentItem());
                AssignmentItemList[i].AssignmentId = item.AssignmentId;
                AssignmentItemList[i].AssignmentDesc = item.AssignmentDesc;
                AssignmentItemList[i].InstructorCourseId = item.InstructorCourseId;
                AssignmentItemList[i].AssignmentName = item.AssignmentName;
                AssignmentItemList[i].AssignmentType = item.AssignmentType;
                AssignmentItemList[i].MaxPoints = item.MaxPoints;
                AssignmentItemList[i].DueDate = item.DueDate;
                AssignmentItemList[i].CourseId = item.CourseId;
                AssignmentItemList[i].CourseNum = item.CourseNum;
                AssignmentItemList[i].CourseName = item.CourseName;
                i++;
            }
        }

        //This method generates all of the student assignments for a particular course
        //Generates a list of StudentAssignmentItems (targeted for student views)
        public static void GenerateStudentAssignmentList(int? id)
        {
            StudentAssignments = new List<StudentAssignmentItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            var query = (from a in gds.Assignments
                         join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join sc in gds.StudentCourses on ic.course_id equals sc.course_id
                         join c in gds.Courses on ic.course_id equals c.course_id
                         where c.course_id == id && sc.student_id == Name.user_id      //Name.user_id is the student, so find matching student_id
                         select new
                         {
                             AssignmentId = a.assignment_id,
                             InstructorCourseId = a.instructor_course_id,
                             AssignmentName = a.assignment_name,
                             AssignmentDesc = a.assignment_desc,
                             AssignmentType = a.assignment_type,
                             MaxPoints = a.max_points,
                             DueDate = a.due_date,
                             CourseId = c.course_id,
                             CourseNum = c.course_num,
                             CourseName = c.course_name,
                             StudentId = sc.student_id
                         }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                StudentAssignments.Add(new StudentAssignmentItem());
                StudentAssignments[i].AssignmentId = item.AssignmentId;
                StudentAssignments[i].AssignmentDesc = item.AssignmentDesc;
                StudentAssignments[i].InstructorCourseId = item.InstructorCourseId;
                StudentAssignments[i].AssignmentName = item.AssignmentName;
                StudentAssignments[i].AssignmentType = item.AssignmentType;
                StudentAssignments[i].MaxPoints = item.MaxPoints;
                StudentAssignments[i].DueDate = item.DueDate;
                StudentAssignments[i].CourseId = item.CourseId;
                StudentAssignments[i].CourseNum = item.CourseNum;
                StudentAssignments[i].CourseName = item.CourseName;
                StudentAssignments[i].StudentId = item.StudentId;
                i++;
            }
        }

        //This method generates all of the submissions by students for a particular assignment
        //Generates an individual StudentAssignmentSubmissionItem.
        //public static void GenerateStudentSubmissionAssignmentItem(int? assignmentId)
        //{
        //    StudentAssignmentSubmission = new StudentAssignmentSubmissionItem();
        //    LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

        //    var query = (from a in gds.Assignments
        //                 join s in gds.StudentAssignments
        //                 on a.assignment_id equals s.assignment_id
        //                 where s.student_id == Name.user_id
        //                 && s.assignment_id == assignmentId
        //                 select new
        //                 {
        //                    AssignmentGradeId = s.assignment_grade_id,
        //                    AssignmentId = a.assignment_id,
        //                    StudentId = s.student_id,
        //                    Grade = s.grade,
        //                    SubmissionDate = s.submission_date,
        //                    TextSubmission = s.text_submission,
        //                    FileSubmission = s.file_submission 
        //                 }).ToList();

        //    foreach (var item in query)
        //    {
        //        StudentAssignmentSubmission.AssignmentGradeId = item.AssignmentGradeId;
        //        StudentAssignmentSubmission.AssignmentId = item.AssignmentId;
        //        StudentAssignmentSubmission.StudentId = item.StudentId;
        //        StudentAssignmentSubmission.Grade = item.Grade;
        //        StudentAssignmentSubmission.SubmissionDate = (DateTime)item.SubmissionDate;
        //        StudentAssignmentSubmission.FileSubmission = item.FileSubmission;
        //        StudentAssignmentSubmission.TextSubmission = item.TextSubmission;

        //        if (item.Grade != null)
        //        {
        //            StudentAssignmentSubmission.isGraded = true;
        //        }
        //        else
        //        {
        //            StudentAssignmentSubmission.isGraded = false;
        //        }

        //    }
        //}

        //This method returns all of the submissions by a particular student with the parameter id 
        //Generates a list of StudentAssignmentSubmissionItems
        public static void GenerateThisStudentsSubmissions(int? studentId)
        {
            ThisStudentsSubmissions = new List<StudentAssignmentSubmissionItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
           // IndividualStudentTotalPoints = new int();
            //IndividualStudentMaxPoints = new int();

            var query = (from a in gds.StudentAssignments
                         join assign in gds.Assignments on a.assignment_id equals assign.assignment_id
                         where a.student_id == studentId
                         select new
                         {
                             AssignmentGradeId = a.assignment_grade_id,
                             AssignmentId = a.assignment_id,
                             StudentId = a.student_id,
                             Grade = a.grade,
                             InstructorFeedback = a.instructor_feedback,
                             SubmissionDate = a.submission_date,
                             TextSubmission = a.text_submission,
                             FileSubmission = a.file_submission,
                             MaxPoints = assign.max_points
                         }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                ThisStudentsSubmissions.Add(new StudentAssignmentSubmissionItem());
                ThisStudentsSubmissions[i].AssignmentGradeId = item.AssignmentGradeId;
                ThisStudentsSubmissions[i].AssignmentId = item.AssignmentId;
                ThisStudentsSubmissions[i].StudentId = item.StudentId;
                ThisStudentsSubmissions[i].Grade = item.Grade;
                ThisStudentsSubmissions[i].MaxPoints = (int)item.MaxPoints;
                ThisStudentsSubmissions[i].SubmissionDate = (DateTime)item.SubmissionDate;
                ThisStudentsSubmissions[i].TextSubmission = item.TextSubmission;
                ThisStudentsSubmissions[i].FileSubmission = item.FileSubmission;
                ThisStudentsSubmissions[i].InstructorFeedback = item.InstructorFeedback;

                if (item.Grade != null)
                {
                    ThisStudentsSubmissions[i].isGraded = true; 
                    //IndividualStudentTotalPoints += (int)item.Grade;    // Total points for all graded assignements
                    //IndividualStudentMaxPoints += (int)item.Grade;      // Max points for all graded assignments
                }
                else
                {
                    ThisStudentsSubmissions[i].isGraded = false;
                }
                i++;
            }
            //Calculate letter grade and save it to database
            //StudentCours studentCours = gds.StudentCourses.Where(x => x.student_id == studentId).FirstOrDefault();
            //double gradePoints = ((double)IndividualStudentTotalPoints / IndividualStudentMaxPoints) * 100;
            //gradePoints = Math.Truncate(100 * gradePoints) / 100;
            //String letterGrade = getLetterGrade(gradePoints);
            //studentCours.letter_grade = letterGrade;
            //gds.SaveChanges();
        }

        //This method returns all of the submissions by a particular student with the parameter id 
        //Generates a list of StudentAssignmentSubmissionItems
        public static void GenerateThisStudentsSubmissionsForCourse(int? studentId, int instructorCourseId)
        {
            ThisStudentsSubmissions = new List<StudentAssignmentSubmissionItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            //IndividualStudentTotalPoints = new int();
            //IndividualStudentMaxPoints = new int();
            int totalPoints = 0;
            int maxPoints = 0;

            var query = (from a in gds.Assignments
                         //join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join sa in gds.StudentAssignments on a.assignment_id equals sa.assignment_id
                         join sc in gds.StudentCourses on sa.student_id equals sc.student_id
                         where sa.student_id == studentId
                         where a.instructor_course_id == instructorCourseId
                         select new
                         {
                             AssignmentGradeId = sa.assignment_grade_id,
                             //AssignmentId = sa.assignment_id,
                             StudentId = sa.student_id,
                             Grade = sa.grade,
                             /*InstructorFeedback = sa.instructor_feedback,
                             SubmissionDate = sa.submission_date,
                             TextSubmission = sa.text_submission,
                             FileSubmission = sa.file_submission,*/
                             MaxPoints = a.max_points
                         }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                ThisStudentsSubmissions.Add(new StudentAssignmentSubmissionItem());
                ThisStudentsSubmissions[i].AssignmentGradeId = item.AssignmentGradeId;
                //ThisStudentsSubmissions[i].AssignmentId = item.AssignmentId;
                ThisStudentsSubmissions[i].StudentId = item.StudentId;
                ThisStudentsSubmissions[i].Grade = item.Grade;
                
                //ThisStudentsSubmissions[i].MaxPoints = (int)item.MaxPoints;

                /*ThisStudentsSubmissions[i].SubmissionDate = (DateTime)item.SubmissionDate;
                ThisStudentsSubmissions[i].TextSubmission = item.TextSubmission;
                ThisStudentsSubmissions[i].FileSubmission = item.FileSubmission;
                ThisStudentsSubmissions[i].InstructorFeedback = item.InstructorFeedback;*/

                if (item.Grade != null)
                {
                    ThisStudentsSubmissions[i].isGraded = true;
                    totalPoints += (int)item.Grade;    // Total points for all graded assignements
                    maxPoints += (int)item.MaxPoints;      // Max points for all graded assignments
                }
                else
                {
                    ThisStudentsSubmissions[i].isGraded = false;
                }
                i++;
            }
            //Calculate letter grade and save it to database
            int courseId = gds.InstructorCourses.Where(x => x.instructor_course_id == instructorCourseId).Select(x => x.course_id).FirstOrDefault();
            StudentCours studentCours = gds.StudentCourses.Where(x => (x.student_id == studentId) && (x.course_id == courseId)).FirstOrDefault();
            double gradePoints = ((double)totalPoints / maxPoints) * 100;
            gradePoints = Math.Truncate(100 * gradePoints) / 100;
            String letterGrade = getLetterGrade(gradePoints);
            studentCours.letter_grade = letterGrade;
            gds.SaveChanges();
        }

        public static String getLetterGrade(double percentage)
        {
            String letterGrade = "";

            if (percentage >= 90.0) letterGrade = "A";
            else if (percentage >= 80.0 && percentage < 90.0) letterGrade = "B";
            else if (percentage >= 70.0 && percentage < 80.0) letterGrade = "C";
            else if (percentage >= 60.0 && percentage < 70.0) letterGrade = "D";
            else if (percentage < 60.0) letterGrade = "F";
            else letterGrade = "N/A";

            return letterGrade;
        }

        //This method returns a single submission by student with studentId and assignment with assignmentId 
        //Generates a list of StudentAssignmentSubmissionItems
        public static void GenerateThisStudentsSubmissionForAssignment(int? assignmentId)
        {
            StudentAssignmentSubmission = new StudentAssignmentSubmissionItem();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

            var query = (from a in gds.StudentAssignments
                         where a.student_id == Name.user_id
                         where a.assignment_id == assignmentId
                         select new
                         {
                             AssignmentGradeId = a.assignment_grade_id,
                             AssignmentId = a.assignment_id,
                             StudentId = a.student_id,
                             Grade = a.grade,
                             InstructorFeedback = a.instructor_feedback,
                             SubmissionDate = a.submission_date,
                             TextSubmission = a.text_submission,
                             FileSubmission = a.file_submission
                         }).ToList();

            foreach (var item in query)
            {
                StudentAssignmentSubmission.AssignmentGradeId = item.AssignmentGradeId;
                StudentAssignmentSubmission.AssignmentId = item.AssignmentId;
                StudentAssignmentSubmission.StudentId = item.StudentId;
                StudentAssignmentSubmission.Grade = item.Grade;
                StudentAssignmentSubmission.SubmissionDate = (DateTime)item.SubmissionDate;
                StudentAssignmentSubmission.TextSubmission = item.TextSubmission;
                StudentAssignmentSubmission.FileSubmission = item.FileSubmission;
                StudentAssignmentSubmission.InstructorFeedback = item.InstructorFeedback;
                if (item.Grade != null)
                {
                    StudentAssignmentSubmission.isGraded = true;
                }
                else
                {
                    StudentAssignmentSubmission.isGraded = false;
                }
            }
        }

        //This method generates all of the submissions for a particular assignment with the parameter id
        //Generates a list of StudentAssignmentSubmissionItems
        public static void GenerateAllSubmissions(int? assignmentId)
        {
            AllStudentSubmissions = new List<StudentAssignmentSubmissionItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

            var query = (from sa in gds.StudentAssignments
                         join u in gds.ulUsers
                         on sa.student_id equals u.ulUser_id
                         join a in gds.Assignments
                         on sa.assignment_id equals a.assignment_id
                         where sa.assignment_id == assignmentId
                         select new
                         {
                             AssignmentGradeId = sa.assignment_grade_id,
                             AssignmentId = sa.assignment_id,
                             StudentId = sa.student_id,
                             FirstName = u.first_name,
                             LastName = u.last_name,
                             Grade = sa.grade,
                             InstructorFeedback = sa.instructor_feedback,
                             SubmissionDate = sa.submission_date,
                             TextSubmission = sa.text_submission,
                             FileSubmission = sa.file_submission
                         }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                AllStudentSubmissions.Add(new StudentAssignmentSubmissionItem());
                AllStudentSubmissions[i].AssignmentGradeId = item.AssignmentGradeId;
                AllStudentSubmissions[i].AssignmentId = item.AssignmentId;
                AllStudentSubmissions[i].StudentId = item.StudentId;
                AllStudentSubmissions[i].Grade = item.Grade;
                AllStudentSubmissions[i].SubmissionDate = (DateTime)item.SubmissionDate;
                AllStudentSubmissions[i].TextSubmission = item.TextSubmission;
                AllStudentSubmissions[i].FileSubmission = item.FileSubmission;
                AllStudentSubmissions[i].FirstName = item.FirstName;
                AllStudentSubmissions[i].LastName = item.LastName;
                AllStudentSubmissions[i].InstructorFeedback = item.InstructorFeedback;
                i++;
            }
        }

        //This method generates the information of a specific assignment with the parameter assignmentId
        //Generates a list assignmentItem
        /// <summary>
        /// Gets a list Assgnment Item by assingment_id
        /// </summary>
        /// <param name="id"></param>
        public static void GenerateAssignmentItem(int? AssignmentId)
        {
            AssignmentItemList = new List<AssignmentItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

            var query = (from a in gds.Assignments
                         join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join sc in gds.StudentCourses on ic.course_id equals sc.course_id
                         join c in gds.Courses on ic.course_id equals c.course_id
                         where a.assignment_id == AssignmentId
                         select new
                         {
                             AssignmentId = a.assignment_id,
                             InstructorCourseId = a.instructor_course_id,
                             AssignmentName = a.assignment_name,
                             AssignmentDesc = a.assignment_desc,
                             AssignmentType = a.assignment_type,
                             SubmissionType = a.submission_type,
                             MaxPoints = a.max_points,
                             DueDate = a.due_date,
                             CourseId = c.course_id,
                             CourseNum = c.course_num,
                             CourseName = c.course_name,
                             StudentId = sc.student_id
                         }).ToList();
            int i = 0;
            foreach (var item in query)
            {
                AssignmentItemList.Add(new AssignmentItem());
                AssignmentItemList[i].AssignmentId = item.AssignmentId;
                AssignmentItemList[i].AssignmentDesc = item.AssignmentDesc;
                AssignmentItemList[i].InstructorCourseId = item.InstructorCourseId;
                AssignmentItemList[i].AssignmentName = item.AssignmentName;
                AssignmentItemList[i].AssignmentType = item.AssignmentType;
                AssignmentItemList[i].MaxPoints = item.MaxPoints;
                AssignmentItemList[i].DueDate = item.DueDate;
                AssignmentItemList[i].CourseId = item.CourseId;
                AssignmentItemList[i].CourseNum = item.CourseNum;
                AssignmentItemList[i].CourseName = item.CourseName;
                AssignmentItemList[i].SubmissionType = item.SubmissionType;
                i++;
            }
        }

        /// <summary>
        /// Gets a single Assgnment Item by assingment_id
        /// </summary>
        /// <param name="id"></param>
        public static void GenerateSingleAssignmentItem(int? id)
        {
            AssignmentItem = new AssignmentItem();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

            var query = (from a in gds.Assignments
                         join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join sc in gds.StudentCourses on ic.course_id equals sc.course_id
                         join c in gds.Courses on ic.course_id equals c.course_id
                         where a.assignment_id == id
                         where sc.student_id == Name.user_id
                         select new
                         {
                             AssignmentId = a.assignment_id,
                             InstructorCourseId = a.instructor_course_id,
                             AssignmentName = a.assignment_name,
                             AssignmentDesc = a.assignment_desc,
                             AssignmentType = a.assignment_type,
                             SubmissionType = a.submission_type,
                             MaxPoints = a.max_points,
                             DueDate = a.due_date,
                             CourseId = c.course_id,
                             CourseNum = c.course_num,
                             CourseName = c.course_name,
                             StudentId = sc.student_id
                         }).ToList();
            foreach (var item in query)
            {
                AssignmentItem.AssignmentId = item.AssignmentId;
                AssignmentItem.AssignmentDesc = item.AssignmentDesc;
                AssignmentItem.InstructorCourseId = item.InstructorCourseId;
                AssignmentItem.AssignmentName = item.AssignmentName;
                AssignmentItem.AssignmentType = item.AssignmentType;
                AssignmentItem.MaxPoints = item.MaxPoints;
                AssignmentItem.DueDate = item.DueDate;
                AssignmentItem.CourseId = item.CourseId;
                AssignmentItem.CourseNum = item.CourseNum;
                AssignmentItem.CourseName = item.CourseName;
                AssignmentItem.SubmissionType = item.SubmissionType;
            }
        }

    }
}