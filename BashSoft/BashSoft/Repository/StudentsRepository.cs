using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace BashSoft
{
    public static class StudentsRepository
    {
        
        public static bool isDataInitialized = false;
        private static Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;

        public static void InitializeData(string fileName)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Reading data...");
                studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
                ReadData(fileName);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.DataAlreadyInitializedException);
            }
        }

        private static void ReadData(string fileName)
        {
            string path = SessionData.Path + "\\" + fileName;
            
            if (File.Exists(path))
            {
                string pattern = @"([A-Z][A-Za-z#+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Z][a-z]{0,3}\d{2}_\d{2,4})\s+(\d+)";
                Regex regex = new Regex(pattern);
                string[] input = File.ReadAllLines(path);

                for (int i = 0; i < input.Length; i++)
                {
                    if (!string.IsNullOrEmpty(input[i]) && regex.IsMatch(input[i]))
                    {
                        Match currentMatch = regex.Match(input[i]);

                        string course = currentMatch.Groups[1].Value;
                        string student = currentMatch.Groups[2].Value;
                        int score;
                        bool hasParsed = int.TryParse(currentMatch.Groups[3].Value,out score);

                        if(hasParsed && score>=0 && score <= 100)
                        {
                            if (!studentsByCourse.ContainsKey(course))
                            {
                                studentsByCourse.Add(course, new Dictionary<string, List<int>>());
                            }

                            if (!studentsByCourse[course].ContainsKey(student))
                            {
                                studentsByCourse[course].Add(student, new List<int>());
                            }

                            studentsByCourse[course][student].Add(score);
                        }
                        
                    }
                    
                }

                isDataInitialized = true;
                OutputWriter.WriteMessageOnNewLine("Data read!");
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
            }
            

            
        }

        private static bool IsQueryForCoursePossible(string courseName)
        {
            if (isDataInitialized)
            {
                if (studentsByCourse.ContainsKey(courseName))
                {
                    return true;
                }
                OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
                return false;


                
            }
            OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            return false;
        }
        private static bool IsQueryForStudentPossible(string courseName,string studentUserName)
        {
            if(IsQueryForCoursePossible(courseName) && studentsByCourse[courseName].ContainsKey(studentUserName))
            {
                return true;
            }
            OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            return false;
        }
        
        public static void GetStudentScoresFromCourse(string courseName,string username)
        {
            if (IsQueryForStudentPossible(courseName, username))
            {
                OutputWriter.DisplayStudent(new KeyValuePair<string, List<int>>(username,studentsByCourse[courseName][username]));
            }
        }

        public static void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach (var studentMarks in studentsByCourse[courseName])
                {
                    OutputWriter.DisplayStudent(studentMarks);
                }
                
            }
        }

        public static void FilterAndTake(string course, string filter, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(course))
            {
                if(studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[course].Count;
                }

                RepositoryFilters.FilterAndTake(studentsByCourse[course], filter, studentsToTake.Value);

                
                
            }
            
        }

        public static void OrderAndTake(string course, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(course))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[course].Count;
                }

              

                RepositorySorters.OrderAndTake(studentsByCourse[course], comparison, studentsToTake.Value);

               
            }
            

        }
    }
}
