using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IBZLiShi
    {
        Task Insert(BZLiShiDto input);

        Task<(List<BZLiShiDto>, int)> Query(string KeyWord, int PageIndex, int PageSize);

        Task Remove(BZLiShiDto input);
    }
}
