using CandySugar.Xam.Common;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using CandySugar.Xam.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using System.Linq;
using XExten.Advance.StaticFramework;
using XExten.Advance.InternalFramework.Securities.Common;

namespace CandySugar.Xam.Core.ServiceImpl
{
    public class LXSLiShi : ILXSLiShi
    {
        public async Task Insert(CandyLXSLiShiDto input)
        {
            var entity = input.ToMapest<Candy_LXS_LiShi>();
            entity.InitProperty();
            var db = SqliteDbContext.Instance.SqlDb;
            await db.InsertAsync(entity);
            await db.CloseAsync();
        }

        public async Task InsertOrUpdate(CandyLXSLiShiDto input)
        {
            if (await CheckFirst(input))
                await Insert(input);
            else
                await Update(input);
        }

        public async Task<bool> CheckFirst(CandyLXSLiShiDto input)
        {
            var State = await SqliteDbContext.Instance.SqlDb.Table<Candy_LXS_LiShi>().Where(t => t.BookName.Equals(input.BookName)).FirstOrDefaultAsync();
            return State == null;
        }

        public async Task Update(CandyLXSLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;

            var entity = await db.Table<Candy_LXS_LiShi>().Where(t => t.BookName == input.BookName).FirstOrDefaultAsync();
            entity.Span = DateTime.Now.Ticks;
            entity.ChapterName = input.ChapterName;
            entity.ChapeterAddress = input.ChapeterAddress;
            await db.UpdateAsync(entity);
            await db.CloseAsync();
        }

        public async Task<List<CandyLXSLiShiDto>> Query()
        {
            var data = await SqliteDbContext.Instance.SqlDb.Table<Candy_LXS_LiShi>().OrderByDescending(t => t.Span).ToListAsync();

          return  data.Select(t => new CandyLXSLiShiDto
            {
                Cover = t.Cover,
                ChapeterAddress = t.ChapeterAddress,
                BookName = t.BookName,
                ChapterName = t.ChapterName,
                Content = SyncStatic.Decompress(t.Content, SecurityType.Base64),
                Image = t.Image,
                IsBook = t.IsBook,
                PId = t.PId,
                Span = t.Span,
            }).ToList();
        }

        public async Task<bool> Remove(CandyLXSLiShiDto input)
        {
            return await SqliteDbContext.Instance.SqlDb.Table<Candy_LXS_LiShi>().DeleteAsync(t => t.PId == input.PId) > 0;
        }
    }
}
