using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IHistory
    {
        Task<List<CandyHistoryDto>> InsertAndQuery(CandyHistoryDto input, bool IsInsertAndQuery = false);
        Task Remove(CandyHistoryDto input);
        Task Clear(CandyHistoryDto input);
    }
}
