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
    public class Setting : ISetting
    {
        public async Task Save(CandySettingDto input)
        {
            var entity = input.ToMapest<Candy_Setting>();
            var db = SqliteDbContext.Instance.SqlDb;
            var opt = await db.Table<Candy_Setting>().FirstOrDefaultAsync();
            if (opt == null)
                await db.InsertAsync(entity);
            else
                await db.UpdateAsync(entity);
            await db.CloseAsync();
        }

        public async Task<CandySettingDto> Query() 
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var res = await db.Table<Candy_Setting>().FirstOrDefaultAsync();
            return res.ToMapest<CandySettingDto>();
        }
    }
}
