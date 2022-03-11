using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Entity.Model;
using CandySugar.Xam.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.ServiceImpl
{
    public class XSLiShi : IXSLiShi
    {
        public async Task Insert(XS_LiShi input)
        {
            input.InitProperty();
            var db = SqliteDbContext.Instance.SqlDb;
            await db.InsertAsync(input);
            await db.CloseAsync();
        }

        public async Task InsertOrUpdate(XS_LiShi input)
        {
            if (await CheckFirst(input))
                await Insert(input);
            else
                await Update(input);
        }

        public async Task<bool> CheckFirst(XS_LiShi input)
        {
            var State = await SqliteDbContext.Instance.SqlDb.Table<XS_LiShi>().Where(t => t.BookName.Equals(input.BookName)).FirstOrDefaultAsync();
            return State == null;
        }

        public async Task Update(XS_LiShi input)
        {
            var db = SqliteDbContext.Instance.SqlDb;

            var entity = await db.Table<XS_LiShi>().Where(t => t.BookName == input.BookName).FirstOrDefaultAsync();
            entity.Span = DateTime.Now.Ticks;
            entity.ChapterName = input.ChapterName;
            entity.ChapeterAddress = input.ChapeterAddress;
            await db.UpdateAsync(entity);
            await db.CloseAsync();
        }

        public async Task<List<XS_LiShi>> Query()
        {
            return await SqliteDbContext.Instance.SqlDb.Table<XS_LiShi>().OrderByDescending(t => t.Span).ToListAsync();
        }
    }
}
