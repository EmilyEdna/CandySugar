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
    public class MHLiShi : IMHLiShi
    {
        public async Task InsertOrUpdate(CandyMHLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var Lishi = await db.Table<Candy_MH_LiShi>().FirstOrDefaultAsync(t => t.TagKey == input.TagKey && t.Name.Equals(input.Name));
            if (Lishi == null)
            {
                var entity = input.ToMapest<Candy_MH_LiShi>();
                entity.InitProperty();
                await db.InsertAsync(entity);
            }
            else
            {
                Lishi.Address = input.Address;
                Lishi.Title = input.Title;
                await db.UpdateAsync(Lishi);
            }
            await db.CloseAsync();
        }

        public async Task<List<CandyMHLiShiDto>> Query()
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var data = await db.Table<Candy_MH_LiShi>().OrderByDescending(t => t.Span).ToListAsync();
            await db.CloseAsync();
            return data.ToMapest<List<CandyMHLiShiDto>>();
        }

        public async Task<bool> Remove(CandyMHLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var data = await db.Table<Candy_MH_LiShi>().DeleteAsync(t => t.PId == input.PId) > 0;
            await db.CloseAsync();
            return data;
        }
    }
}
