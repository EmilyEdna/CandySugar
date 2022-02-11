using CandySugar.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Core.Service
{
    public interface IBiZhi
    {
        Task RemoveFavorite(long Id);
        Task AddFavorite(CandyWallpaperDto input);
        Task<CandyWallpaperPageDto> GetFavorite(string lable, int pageIndex);
        Task<List<long>> GetAllFavorite();
    }
}
