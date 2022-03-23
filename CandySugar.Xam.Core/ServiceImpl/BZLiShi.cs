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
    public class BZLiShi : IBZLiShi
    {

        public async Task Insert(BZLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var entity = input.ToMapest<BZ_LiShi>();
            entity.Label = string.Join("|", input.Labels);
            var entityCheck = await db.Table<BZ_LiShi>().Where(t => t.Id == input.Id).FirstOrDefaultAsync();
            if (entityCheck == null)
            {
                entity.InitProperty();
                await db.InsertAsync(entity);
                await db.CloseAsync();
            }
        }

        public async Task<(List<BZLiShiDto>, int)> Query(string KeyWord, int PageIndex, int PageSize)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var query = db.Table<BZ_LiShi>();
            if (!KeyWord.IsNullOrEmpty())
                query = query.Where(t => t.Label.Contains(KeyWord));
            var Count = await query.CountAsync();
            var Result = await query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
            return (Result.ToMapest<List<BZLiShiDto>>(), Count);
        }

        public async Task Remove(BZLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.Table<BZ_LiShi>().DeleteAsync(t => t.Id == input.Id);
            await db.CloseAsync();
        }
    }
}
