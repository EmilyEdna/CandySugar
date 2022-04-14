using CandySugar.Xam.Common;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using CandySugar.Xam.Common.Enum;
using CandySugar.Xam.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Xam.Core.ServiceImpl
{
    public class History : IHistory
    {
        public async Task Clear(CandyHistoryDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.Table<Candy_History>().DeleteAsync(t => t.CheckType == input.CheckType);
            await db.CloseAsync();
        }

        public async Task<List<CandyHistoryDto>> InsertAndQuery(CandyHistoryDto input, bool IsInsertAndQuery = false)
        {
            var data = new List<CandyHistoryDto>();
            var db = SqliteDbContext.Instance.SqlDb;
            if (IsInsertAndQuery)
            {
                var check = await db.Table<Candy_History>().FirstOrDefaultAsync(t => t.CheckType == input.CheckType && input.CheckText.Contains(t.CheckText));
                if (check == null)
                {
                    var entity = input.ToMapest<Candy_History>();
                    entity.InitProperty();
                    await db.InsertAsync(entity);
                }
                data = (await db.Table<Candy_History>().Where(t => t.CheckType == input.CheckType).ToListAsync()).ToMapest<List<CandyHistoryDto>>();
            }
            else
                data = (await db.Table<Candy_History>().Where(t => t.CheckType == input.CheckType).ToListAsync()).ToMapest<List<CandyHistoryDto>>();
            await db.CloseAsync();
            return data;
        }

        public async Task Remove(CandyHistoryDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.Table<Candy_History>().DeleteAsync(t => t.PId == input.PId);
            await db.CloseAsync();
        }
    }
}
