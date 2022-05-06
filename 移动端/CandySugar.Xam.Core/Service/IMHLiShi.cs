using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IMHLiShi
    {
        Task InsertOrUpdate(CandyMHLiShiDto input);
        Task<List<CandyMHLiShiDto>> Query();
        Task<bool> Remove(CandyMHLiShiDto input);
    }
}
