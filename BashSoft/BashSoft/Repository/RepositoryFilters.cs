using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft
{
    public static class RepositoryFilters
    {
        public static void FilterAndTake(Dictionary<string, List<int>> data, string filter, int studentsToTake)
        {
            if (filter == "excellent")
            {
                FilterAndTake(data, x => x >= 5.0, studentsToTake);
            }
            else if (filter == "average")
            {
                FilterAndTake(data, x => x >= 3.5 && x < 5.0, studentsToTake);
            }
            else if (filter == "poor")
            {
                FilterAndTake(data, x => x < 3.5, studentsToTake);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidStudentFilter);
            }
        }
        private static void FilterAndTake(Dictionary<string, List<int>> data, Predicate<double> filter, int studentsToTake)
        {
            int counter = 0;
            foreach (var user_points in data)
            {
                if (counter == studentsToTake)
                {
                    break;
                }
                double averageScore = user_points.Value.Average();
                double percentageOfAll = averageScore / 100;
                double mark = percentageOfAll * 4 + 2;
                if (filter(mark))
                {
                    OutputWriter.DisplayStudent(user_points);
                    counter++;
                }
            }
        }

    }
}
