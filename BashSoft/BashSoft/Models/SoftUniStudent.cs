using BashSoft.Contracts;
using BashSoft.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BashSoft.Models
{
    public class SoftUniStudent:IStudent
    {
        private string username;
        private Dictionary<string, ICourse> enrolledCourses;
        private Dictionary<string, double> marksByCourseName;

        public string Username
        {
            get { return this.username; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidStringException();
                }

                this.username = value;
            }
        }

        public IReadOnlyDictionary<string, ICourse> EnrolledCourses
        {
            get
            {
                return enrolledCourses;
            }
        }

        public IReadOnlyDictionary<string, double> MarksByCourseName
        {
            get
            {
                return marksByCourseName;
            }
        }

        public SoftUniStudent(string username)
        {
            this.Username = username;
            this.enrolledCourses = new Dictionary<string, ICourse>();
            this.marksByCourseName = new Dictionary<string, double>();
        }

        public void EnrollInCourse(ICourse course)
        {
            if (this.enrolledCourses.ContainsKey(course.Name))
            {
                throw new DuplicateEntryInStructureException(this.Username, course.Name);
            }

            this.enrolledCourses.Add(course.Name, course);
        }

        public void SetMarkOnCourse(string courseName, params int[] scores)
        {
            if (!this.enrolledCourses.ContainsKey(courseName))
            {
                throw new CourseNotFoundException();
            }

            if (scores.Length > SoftUniCourse.NumberOfTasksOnExam)
            {
                throw new InvalidNumberOfScoresException();
            }

            this.marksByCourseName.Add(courseName, CalculateMark(scores));
        }

        private double CalculateMark(int[] scores)
        {
            double percentageOfSolvedExam = scores.Sum() / (double)(SoftUniCourse.NumberOfTasksOnExam * SoftUniCourse.MaxScoreOnExamTask);
            double mark = percentageOfSolvedExam * 4 + 2;
            return mark;
        }

        public int CompareTo(IStudent other)
        {
            return this.Username.CompareTo(other.Username);
        }
        public override string ToString()
        {
            return this.Username;
        }
    }
}
