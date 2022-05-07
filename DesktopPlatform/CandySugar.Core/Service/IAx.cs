using CandySugar.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Core.Service
{
    public interface IAx
    {
        void SaveCover(CandyGalCoverDto input);
        byte[] GetCover(string input);
        Task AddFavorite(CandyAxFavoriteDto input);
        Task<CandyAxFavoritePageDto> GetFavorite(string key, int PageIndex);
        Task RemoveFavorite(int VId);
        Task<List<int>> GetAllFavorite();
    }
}
