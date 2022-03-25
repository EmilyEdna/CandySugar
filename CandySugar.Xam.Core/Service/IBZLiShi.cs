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
        Task Insert(CandyBZLiShiDto input);

        Task<(List<CandyBZLiShiDto>, int)> Query(string KeyWord, int PageIndex, int PageSize);

        Task Remove(CandyBZLiShiDto input);
    }
}
