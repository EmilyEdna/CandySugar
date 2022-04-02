using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IYYLiShi
    {
        Task AddPlayList(CandyYYLiShiDto input);
        Task<List<CandyYYLiShiDto>> GetPlayList();
        Task ClearPlayList();
        Task RemovePlayList(Guid input);
        Task<int> PlayCount();
    }
}
