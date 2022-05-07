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
    public class DMLiShi : IDMLiShi
    {
        public async Task<bool> CheckFirst(CandyDMLiShiDto input)
        {
            var State = await SqliteDbContext.Instance.SqlDb.Table<Candy_DM_LiShi>()
                .Where(t => t.AnimeName.Equals(input.AnimeName))
                .FirstOrDefaultAsync();
            return State == null;
        }

        public async Task Insert(CandyDMLiShiDto input)
        {
            var entity = input.ToMapest<Candy_DM_LiShi>();
            entity.InitProperty();
            var db = SqliteDbContext.Instance.SqlDb;
            await db.InsertAsync(entity);
            await db.CloseAsync();
        }

        public async Task InsertOrUpdate(CandyDMLiShiDto input)
        {
            if (await CheckFirst(input))
                await Insert(input);
            else
                await Update(input);
        }

        public async Task<List<CandyDMLiShiDto>> Query()
        {
            var data = await SqliteDbContext.Instance.SqlDb.Table<Candy_DM_LiShi>().OrderByDescending(t => t.Span).ToListAsync();
            return data.ToMapest<List<CandyDMLiShiDto>>();
        }

        public async Task<bool> Remove(CandyDMLiShiDto input)
        {
            return await SqliteDbContext.Instance.SqlDb.Table<Candy_DM_LiShi>().DeleteAsync(t => t.PId == input.PId) > 0;
        }

        public async Task Update(CandyDMLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;

            var entity = await db.Table<Candy_DM_LiShi>().Where(t => t.AnimeName == input.AnimeName).FirstOrDefaultAsync();
            entity.Span = DateTime.Now.Ticks;
            entity.PlayURL = input.PlayURL;
            entity.CollectionName = input.CollectionName;
            await db.UpdateAsync(entity);
            await db.CloseAsync();
        }
    }
}
