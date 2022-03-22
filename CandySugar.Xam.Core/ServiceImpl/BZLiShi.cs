using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Entity.Model;
using CandySugar.Xam.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Xam.Core.ServiceImpl
{
    public class BZLiShi : IBZLiShi
    {

        public async Task Insert(BZ_LiShi input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var entity = await db.Table<BZ_LiShi>().Where(t => t.Id == input.Id).FirstOrDefaultAsync();
            if (entity == null)
            {
                input.InitProperty();
                await db.InsertAsync(input);
                await db.CloseAsync();
            }
        }

        public async Task<(List<BZ_LiShi>, int)> Query(string KeyWord, int PageIndex, int PageSize)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var query = db.Table<BZ_LiShi>();
            if (!KeyWord.IsNullOrEmpty())
                query = query.Where(t => t.Label.Contains(KeyWord));
            var Count = await query.CountAsync();
            var Result = await query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
            return (Result, Count);
        }

        public async Task Remove(BZ_LiShi input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.Table<BZ_LiShi>().DeleteAsync(t => t.Id == input.Id);
            await db.CloseAsync();
        }
    }
}
