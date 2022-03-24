using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Entity.Model;
using CandySugar.Xam.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.ServiceImpl
{
    public class DMLiShi : IDMLiShi
    {
        public async Task<bool> CheckFirst(DM_LiShi input)
        {
            var State = await SqliteDbContext.Instance.SqlDb.Table<DM_LiShi>()
                .Where(t => t.AnimeName.Equals(input.AnimeName))
                .FirstOrDefaultAsync();
            return State == null;
        }

        public async Task Insert(DM_LiShi input)
        {
            input.InitProperty();
            var db = SqliteDbContext.Instance.SqlDb;
            await db.InsertAsync(input);
            await db.CloseAsync();
        }

        public async Task InsertOrUpdate(DM_LiShi input)
        {
            if (await CheckFirst(input))
                await Insert(input);
            else
                await Update(input);
        }

        public async Task<List<DM_LiShi>> Query()
        {
           return await SqliteDbContext.Instance.SqlDb.Table<DM_LiShi>().OrderByDescending(t => t.Span).ToListAsync();
        }

        public async Task<bool> Remove(DM_LiShi input)
        {
            return await SqliteDbContext.Instance.SqlDb.Table<DM_LiShi>().DeleteAsync(t => t.PId == input.PId) > 0;
        }

        public async Task Update(DM_LiShi input)
        {
            var db = SqliteDbContext.Instance.SqlDb;

            var entity = await db.Table<DM_LiShi>().Where(t => t.AnimeName == input.AnimeName).FirstOrDefaultAsync();
            entity.Span = DateTime.Now.Ticks;
            entity.PlayURL = input.PlayURL;
            entity.CollectionName = input.CollectionName;
            await db.UpdateAsync(entity);
            await db.CloseAsync();
        }
    }
}
