using CandySugar.Xam.Common.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IBZLiShi
    {
        Task Insert(BZ_LiShi input);

        Task<(List<BZ_LiShi>, int)> Query(string KeyWord, int PageIndex, int PageSize);

        Task Remove(BZ_LiShi input);
    }
}
