using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Common.Entity.Model;
using CandySugar.Core.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.Core.ServiceImpl
{
    public class LiShi : SqlSugarDbContext, ILiShi
    {
        #region 小说历史
        public async Task AddNovelHistory(CandyNovelHistoryDto input)
        {
            var entity = input.ToMapest<CandyNovelHistory>();
            var Novel = Db.Queryable<CandyNovelHistory>().Where(t => t.ChapterName == input.ChapterName && t.BookName == input.BookName).First();
            if (Novel == null)
                await Db.Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }
        public async Task<List<CandyNovelHistoryDto>> GetNovelHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            return await Db.Queryable<CandyNovelHistory>().Where(t => t.Span >= today)
                  .OrderBy(t => t.Span, OrderByType.Desc).ToPageList(1, 100)
                  .ToMapestAsync<List<CandyNovelHistoryDto>>();
        }
        public async Task ClearNovelHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            await Db.Deleteable<CandyNovelHistory>().Where(t => t.Span < today).ExecuteCommandAsync();
        }
        #endregion

        #region 漫画历史
        public async Task AddMangaHistory(CandyMangaHistoryDto input)
        {
            var entity = input.ToMapest<CandyMangaHistory>();
            entity.Chapters = SyncStatic.Compress(entity.Chapters, SecurityType.Base64);
            var Novel = Db.Queryable<CandyMangaHistory>().Where(t => t.Name == input.Name && t.Title == input.Title).First();
            if (Novel == null)
                await Db.Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }
        public async Task<List<CandyMangaHistoryDto>> GetMangaHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            return (await Db.Queryable<CandyMangaHistory>().Where(t => t.Span >= today)
                  .OrderBy(t => t.Span, OrderByType.Desc).ToPageListAsync(1, 100))
                  .Select(t => new CandyMangaHistoryDto
                  {
                      Address = t.Address,
                      PId = t.PId,
                      Name = t.Name,
                      Span = t.Span,
                      TagKey = t.TagKey,
                      Title = t.Title,
                      Chapters = SyncStatic.Decompress(t.Chapters, SecurityType.Base64)
                  }).ToList();

        }
        public async Task ClearMangaHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            await Db.Deleteable<CandyMangaHistory>().Where(t => t.Span < today).ExecuteCommandAsync();
        }
        #endregion

        #region 动漫历史
        public async Task AddAnimeHistory(CandyAnimeHistoryDto input)
        {
            var entity = input.ToMapest<CandyAnimeHistory>();
            var Novel = Db.Queryable<CandyAnimeHistory>().Where(t => t.AnimeName == input.AnimeName && t.CollectName == input.CollectName).First();
            if (Novel == null)
                await Db.Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }
        public async Task<List<CandyAnimeHistoryDto>> GetAnimeHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            return await Db.Queryable<CandyAnimeHistory>().Where(t => t.Span >= today)
                  .OrderBy(t => t.Span, OrderByType.Desc).ToPageList(1, 100)
                  .ToMapestAsync<List<CandyAnimeHistoryDto>>();
        }
        public async Task ClearAnimeHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            await Db.Deleteable<CandyAnimeHistory>().Where(t => t.Span < today).ExecuteCommandAsync();
        }
        #endregion
    }
}
