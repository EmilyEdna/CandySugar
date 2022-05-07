using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IAXLiShi
    {
        Task Insert(CandyAXLiShiDto input);

        Task<(List<CandyAXLiShiDto>,int)> Query(string KeyWord, int PageIndex, int PageSize);

        Task Remove(CandyAXLiShiDto input);
    }
}
