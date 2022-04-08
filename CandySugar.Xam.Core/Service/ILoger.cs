using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface ILoger
    {
        Task Insert(CandyGlobalLogDto input);
        Task Delete(CandyGlobalLogDto input);
        Task Clear();
        Task<List<CandyGlobalLogDto>> Query();
    }
}
