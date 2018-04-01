using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using BashSoft.Models;
using BashSoft.Exceptions;
using BashSoft.Contracts;
using BashSoft.DataStructures;

namespace BashSoft
{
    public class StudentsRepository:IDatabase
    {

        private bool isDataInitialized = false;
        
        private RepositoryFilter filter;
        private RepositorySorter sorter;
        private Dictionary<string, ICourse> courses;
        private Dictionary<string, IStudent> students;


        public StudentsRepository(RepositoryFilter filter, RepositorySorter sorter)
        {
            this.filter = filter;
            this.sorter = sorter;
            
        }


        public void LoadData(string fileName)
        {
            if (this.isDataInitialized)
            {
                throw new ArgumentException(ExceptionMessages.DataAlreadyInitializedException);

            }
            else
            {
                OutputWriter.WriteMessageOnNewLine("Reading data...");
                this.students = new Dictionary<string, IStudent>();
                this.courses = new Dictionary<string, ICourse>();

                this.ReadData(fileName);
            }
        }
        public void UnloadData()
        {
            if (!this.isDataInitialized)
            {
                throw new ArgumentException(ExceptionMessages.DataNotInitializedExceptionMessage);

            }

            
            this.students = null;
            this.courses = null;
            this.isDataInitialized = false;

        }

        private void ReadData(string fileName)
        {
            string path = SessionData.Path + "\\" + fileName;

            if (File.Exists(path))
            {
                string pattern = @"([A-Z][a-zA-Z#\+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)";
                Regex regex = new Regex(pattern);
                string[] input = File.ReadAllLines(path);

                for (int i = 0; i < input.Length; i++)
                {
                    if (!string.IsNullOrEmpty(input[i]) && regex.IsMatch(input[i]))
                    {
                        Match currentMatch = regex.Match(input[i]);

                        string courseName = currentMatch.Groups[1].Value;
                        string userName = currentMatch.Groups[2].Value;
                        string scoresStr = currentMatch.Groups[3].Value;
                        try
                        {
                            int[] scores = scoresStr
                                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();
                            if (scores.Any(x => x > 100 || x < 0))
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidScore);
                            }

                            if(scores.Length > SoftUniCourse.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                                continue;
                            }

                            if (!this.students.ContainsKey(userName))
                            {
                                this.students.Add(userName, new SoftUniStudent(userName));
                            }

                            if (!this.courses.ContainsKey(courseName))
                            {
                                this.courses.Add(courseName, new SoftUniCourse(courseName));
                            }

                            ICourse course = this.courses[courseName];
                            IStudent student = this.students[userName];

                            student.EnrollInCourse(course);
                            student.SetMarkOnCourse(courseName, scores);

                            course.EnrollStudent(student);


                        }
                        catch (FormatException fex)
                        {
                            OutputWriter.DisplayException(fex.Message + $"at line : {i}");
                          
                        }
                    }

                }

                isDataInitialized = true;
                OutputWriter.WriteMessageOnNewLine("Data read!");
            }
            else
            {
                throw new InvalidPathException();
                //OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
            }



        }

        private bool IsQueryForCoursePossible(string courseName)
        {
            if (isDataInitialized)
            {
                if (this.courses.ContainsKey(courseName))
                {
                    return true;
                }
                OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
                return false;



            }
            OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            return false;
        }
        private bool IsQueryForStudentPossible(string courseName, string studentUserName)
        {
            if (IsQueryForCoursePossible(courseName) && this.courses[courseName].StudentsByName.ContainsKey(studentUserName))
            {
                return true;
            }
            OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            return false;
        }

        public void GetStudentScoresFromCourse(string courseName, string username)
        {
            if (IsQueryForStudentPossible(courseName, username))
            {
                OutputWriter.DisplayStudent(new KeyValuePair<string, double>(username, this.courses[courseName].StudentsByName[username].MarksByCourseName[courseName]));
            }
        }

        public void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach (var studentMarks in this.courses[courseName].StudentsByName)
                {
                    this.GetStudentScoresFromCourse(courseName, studentMarks.Key);
                }

            }
        }

        public void FilterAndTake(string course, string filter, int? studentsToTake = null)
        {
            if (this.IsQueryForCoursePossible(course))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[course].StudentsByName.Count;
                }

                Dictionary<string, double> marks = this.courses[course].StudentsByName.ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[course]);

                this.filter.FilterAndTake(marks, filter, studentsToTake.Value);



            }

        }

        public void OrderAndTake(string course, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(course))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[course].StudentsByName.Count;
                }

                Dictionary<string, double> marks = this.courses[course].StudentsByName.ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[course]);


                this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);


            }


        }

        public ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> comparer)
        {
            SimpleSortedList<ICourse> sortedCourses = new SimpleSortedList<ICourse>(comparer);
            sortedCourses.AddAll(this.courses.Values);

            return sortedCourses;
        }

        public ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> comparer)
        {
            SimpleSortedList<IStudent> sortedStudents = new SimpleSortedList<IStudent>(comparer);
            sortedStudents.AddAll(this.students.Values);

            return sortedStudents;
        }
    }
}
