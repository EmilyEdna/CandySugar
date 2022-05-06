using CandySugar.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Core.Service
{
    public interface ILiShi
    {
        #region 小说历史
        Task AddNovelHistory(CandyNovelHistoryDto input);
        Task<List<CandyNovelHistoryDto>> GetNovelHistory();
        Task ClearNovelHistory();
        #endregion
        #region 漫画历史
        Task AddMangaHistory(CandyMangaHistoryDto input);
        Task<List<CandyMangaHistoryDto>> GetMangaHistory();
        Task ClearMangaHistory();
        #endregion
        #region 动漫历史
        Task AddAnimeHistory(CandyAnimeHistoryDto input);
        Task<List<CandyAnimeHistoryDto>> GetAnimeHistory();
        Task ClearAnimeHistory();
        #endregion
    }
}
