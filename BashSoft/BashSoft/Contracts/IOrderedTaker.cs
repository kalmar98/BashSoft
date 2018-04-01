using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft.Contracts
{
    public interface IOrderedTaker
    {
        void OrderAndTake(string course, string comparison, int? studentsToTake = null);
    }
}
