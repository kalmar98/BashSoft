using System;
using System.Collections.Generic;
using BashSoft.Contracts;

namespace BashSoft
{
    public class RepositoryFilter:IDataFilter
    {
        public void FilterAndTake(Dictionary<string, double> studentsWithMarks, string filter, int studentsToTake)
        {
            if (filter == "excellent")
            {
                FilterAndTake(studentsWithMarks, x => x >= 5.0, studentsToTake);
            }
            else if (filter == "average")
            {
                FilterAndTake(studentsWithMarks, x => x >= 3.5 && x < 5.0, studentsToTake);
            }
            else if (filter == "poor")
            {
                FilterAndTake(studentsWithMarks, x => x < 3.5, studentsToTake);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidStudentFilter);
            }
        }
        private void FilterAndTake(Dictionary<string, double> studentsWithMarks, Predicate<double> filter, int studentsToTake)
        {
            int counter = 0;
            foreach (var studentMark in studentsWithMarks)
            {
                if (counter == studentsToTake)
                {
                    break;
                }
             
                if (filter(studentMark.Value))
                {
                    OutputWriter.DisplayStudent(new KeyValuePair<string, double>(studentMark.Key, studentMark.Value));
                    counter++;
                }
            }
        }

    }
}
