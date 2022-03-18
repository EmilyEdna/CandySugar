using CandySugar.Xam.Common.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IDMLiShi
    {
        Task Insert(DM_LiShi input);

        Task Update(DM_LiShi input);

        Task InsertOrUpdate(DM_LiShi input);

        Task<bool> CheckFirst(DM_LiShi input);

        Task<List<DM_LiShi>> Query();

        Task<bool> Remove(DM_LiShi input);

    }
}
