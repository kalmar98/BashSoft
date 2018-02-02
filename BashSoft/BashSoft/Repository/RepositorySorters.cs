using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft
{
    public static class RepositorySorters
    {
        public static void OrderAndTake(Dictionary<string,List<int>> data,string comparison, int studentsToTake)
        {
            comparison = comparison.ToLower();
            if(comparison == "ascending")
            {
                PrintStudents(data
                    .OrderBy(x=>x.Value.Sum())
                    .Take(studentsToTake)
                    .ToDictionary(x=>x.Key,x=>x.Value));
            }
            else if (comparison == "descending")
            {
                PrintStudents(data
                   .OrderByDescending(x => x.Value.Sum())
                   .Take(studentsToTake)
                   .ToDictionary(x => x.Key, x => x.Value));
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidComparisonQuery);
            }
        }

        private static void PrintStudents(Dictionary<string, List<int>> studentsSorted)
        {
            foreach (var student in studentsSorted)
            {
                OutputWriter.DisplayStudent(student);
            }
        } 
        
    }
}
