﻿using Investly.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos.IRepos
{
   public interface IInvestorRepo :IRepo<Investor>
    {
        public Tuple<int, int> GetTotalActiveInactive(int activeStatus,int InactiveStatus);
    }
}
