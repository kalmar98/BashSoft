﻿using BashSoft.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BashSoft
{
    public class RepositorySorter:IDataSorter
    {
        public void OrderAndTake(Dictionary<string, double> studentsMarks, string comparison, int studentsToTake)
        {
            comparison = comparison.ToLower();
            if (comparison == "ascending")
            {
                this.PrintStudents(studentsMarks
                    .OrderBy(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(x => x.Key, x => x.Value));
            }
            else if (comparison == "descending")
            {
                PrintStudents(studentsMarks
                   .OrderByDescending(x => x.Value)
                   .Take(studentsToTake)
                   .ToDictionary(x => x.Key, x => x.Value));
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidComparisonQuery);
            }
        }

        private void PrintStudents(Dictionary<string, double> studentsSorted)
        {
            foreach (var student in studentsSorted)
            {
                OutputWriter.DisplayStudent(student);
            }
        }

    }
}
