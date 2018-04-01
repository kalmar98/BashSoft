using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft.Contracts
{
    public interface IRequester
    {
        void GetStudentScoresFromCourse(string courseName, string username);
        void GetAllStudentsFromCourse(string courseName);

        ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> comparer);
        ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> comparer);
    }
}
