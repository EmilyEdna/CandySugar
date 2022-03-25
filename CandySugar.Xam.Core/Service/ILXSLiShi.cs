using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface ILXSLiShi
    {
        Task Insert(CandyLXSLiShiDto input);

        Task Update(CandyLXSLiShiDto input);

        Task InsertOrUpdate(CandyLXSLiShiDto input);

        Task<bool> CheckFirst(CandyLXSLiShiDto input);

        Task<List<CandyLXSLiShiDto>> Query();

        Task<bool> Remove(CandyLXSLiShiDto input);
    }
}
