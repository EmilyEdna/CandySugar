using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IXSLiShi
    {
        Task Insert(CandyXSLiShiDto input);

        Task Update(CandyXSLiShiDto input);

        Task InsertOrUpdate(CandyXSLiShiDto input);

        Task<bool> CheckFirst(CandyXSLiShiDto input);

        Task<List<CandyXSLiShiDto>> Query();

        Task<bool> Remove(CandyXSLiShiDto input);
    }
}
