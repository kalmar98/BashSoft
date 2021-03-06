﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft.Contracts
{
    public interface IDatabase:IRequester,IFilteredTaker,IOrderedTaker
    {
        void LoadData(string fileName);
        void UnloadData();
    }
}
