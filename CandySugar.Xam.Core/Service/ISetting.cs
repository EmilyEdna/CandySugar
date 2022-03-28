using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface ISetting
    {
        Task Save(CandySettingDto input);
        Task<CandySettingDto> Query();
    }
}
