using CandySugar.Xam.Common;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using CandySugar.Xam.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Xam.Core.ServiceImpl
{
    public class XSLiShi : IXSLiShi
    {
        public async Task Insert(CandyXSLiShiDto input)
        {
            var entity = input.ToMapest<Candy_XS_LiShi>();
            entity.InitProperty();
            var db = SqliteDbContext.Instance.SqlDb;
            await db.InsertAsync(entity);
            await db.CloseAsync();
        }

        public async Task InsertOrUpdate(CandyXSLiShiDto input)
        {
            if (await CheckFirst(input))
                await Insert(input);
            else
                await Update(input);
        }

        public async Task<bool> CheckFirst(CandyXSLiShiDto input)
        {
            var State = await SqliteDbContext.Instance.SqlDb.Table<Candy_XS_LiShi>().Where(t => t.BookName.Equals(input.BookName)).FirstOrDefaultAsync();
            return State == null;
        }

        public async Task Update(CandyXSLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;

            var entity = await db.Table<Candy_XS_LiShi>().Where(t => t.BookName == input.BookName).FirstOrDefaultAsync();
            entity.Span = DateTime.Now.Ticks;
            entity.ChapterName = input.ChapterName;
            entity.ChapeterAddress = input.ChapeterAddress;
            await db.UpdateAsync(entity);
            await db.CloseAsync();
        }

        public async Task<List<CandyXSLiShiDto>> Query()
        {
            var data = await SqliteDbContext.Instance.SqlDb.Table<Candy_XS_LiShi>().OrderByDescending(t => t.Span).ToListAsync();
            return data.ToMapest<List<CandyXSLiShiDto>>();
        }

        public async Task<bool> Remove(CandyXSLiShiDto input)
        {
            return await SqliteDbContext.Instance.SqlDb.Table<Candy_XS_LiShi>().DeleteAsync(t => t.PId == input.PId) > 0;
        }
    }
}
