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
    public class YYLiShi : IYYLiShi
    {
        public async Task AddPlayList(CandyYYLiShiDto input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var entity = input.ToMapest<Candy_YY_LiShi>();
            entity.InitProperty();
            var song = await db.Table<Candy_YY_LiShi>().Where(t => t.SongId == input.SongId).FirstOrDefaultAsync();
            if (song == null)
                await db.InsertAsync(entity);
            await db.CloseAsync();
        }

        public async Task ClearPlayList()
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.DeleteAllAsync<Candy_YY_LiShi>();
            await db.CloseAsync();
        }

        public async Task<List<CandyYYLiShiDto>> GetPlayList()
        {
            var db = SqliteDbContext.Instance.SqlDb;
            var data = await db.Table<Candy_YY_LiShi>().OrderByDescending(t => t.Span).ToListAsync();
            await db.CloseAsync();
            return data.ToMapest<List<CandyYYLiShiDto>>();
        }

        public async Task<int> PlayCount()
        {
            return await SqliteDbContext.Instance.SqlDb.Table<Candy_YY_LiShi>().CountAsync();
        }

        public async Task RemovePlayList(Guid input)
        {
            var db = SqliteDbContext.Instance.SqlDb;
            await db.DeleteAsync<Candy_YY_LiShi>(input);
            await db.CloseAsync();
        }
    }
}
