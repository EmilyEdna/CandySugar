using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IDMLiShi
    {
        Task Insert(CandyDMLiShiDto input);

        Task Update(CandyDMLiShiDto input);

        Task InsertOrUpdate(CandyDMLiShiDto input);

        Task<bool> CheckFirst(CandyDMLiShiDto input);

        Task<List<CandyDMLiShiDto>> Query();

        Task<bool> Remove(CandyDMLiShiDto input);

    }
}
