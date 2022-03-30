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
    public class AXLiShi : IAXLiShi
    {

        public async Task Insert(CandyAXLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var dto = await db.Table<Candy_AX_LiShi>().FirstOrDefaultAsync(t => t.VId == input.VId);
            if (dto == null)
            {
                var entity = input.ToMapest<Candy_AX_LiShi>();
                entity.InitProperty();
                await db.InsertAsync(entity);
            }
            await db.CloseAsync();
        }

        public async Task<(List<CandyAXLiShiDto>, int)> Query(string KeyWord, int PageIndex, int PageSize)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var query = db.Table<Candy_AX_LiShi>();
            if (!KeyWord.IsNullOrEmpty())
                query = query.Where(t => t.Title.Contains(KeyWord));

            var Count = await query.CountAsync();
            var Result = await query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
            await db.CloseAsync();
            return (Result.ToMapest<List<CandyAXLiShiDto>>(), Count);
        }

        public async Task Remove(CandyAXLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.Table<Candy_AX_LiShi>().DeleteAsync(t => t.VId == input.VId);
            await db.CloseAsync();
        }
    }
}
