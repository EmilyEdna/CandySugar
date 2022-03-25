
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
    public class Loger : ILoger
    {
        public async Task Delete(CandyGlobalLogDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.Table<Candy_Global_Log>().DeleteAsync(t => t.PId == input.PId);
            await db.CloseAsync();
        }

        public async Task Insert(CandyGlobalLogDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var entity = input.ToMapest<Candy_Global_Log>();
            await db.InsertAsync(entity);
            await db.CloseAsync();
        }

        public async Task<List<CandyGlobalLogDto>> Query()
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var data = await db.Table<Candy_Global_Log>().ToListAsync();
            await db.CloseAsync();
            return data.ToMapest<List<CandyGlobalLogDto>>();
        }
    }
}
