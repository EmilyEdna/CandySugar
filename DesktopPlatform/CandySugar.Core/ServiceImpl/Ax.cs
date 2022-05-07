using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Common.Entity.Model;
using CandySugar.Core.Service;
using XExten.Advance.LinqFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace CandySugar.Core.ServiceImpl
{
    public class Ax : SqlSugarDbContext, IAx
    {
        public async Task AddFavorite(CandyAxFavoriteDto input)
        {
            var entity = input.ToMapest<CandyAxFavorite>();
            entity.IsFavorite= true;
            await Db.Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public byte[] GetCover(string input)
        {
            var db = Db;
            var ck = db.Queryable<CandyGalCover>().First(t => t.Cover == input);
            if (ck == null) return new byte[0];
            return Convert.FromBase64String(db.Queryable<CandyGalCover>()
                .Where(t => t.Cover == input)
                .Select(t => t.CoverBase64).First());
        }

        public async Task<CandyAxFavoritePageDto> GetFavorite(string key, int PageIndex)
        {
            RefAsync<int> Total = 0;
            var data = await Db.Queryable<CandyAxFavorite>().WhereIF(!key.IsNullOrEmpty(), t => t.Title.Contains(key)).ToPageListAsync(PageIndex, 10, Total);
            return new CandyAxFavoritePageDto
            {
                Total = Total,
                Result = data.ToMapest<List<CandyAxFavoriteDto>>()
            };
        }

        public async Task RemoveFavorite(int VId)
        {
            await Db.Deleteable<CandyAxFavorite>(t => t.VId == VId).ExecuteCommandAsync();
        }

        public void SaveCover(CandyGalCoverDto input)
        {
            var db = Db;
            var ck = db.Queryable<CandyGalCover>().First(t => t.Cover == input.Cover);
            if (ck == null)
            {
                input.ConvertToBase64();
                var entity = input.ToMapest<CandyGalCover>();
                db.Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommand();
            }
        }

        public async Task<List<int>> GetAllFavorite()
        {
           return await Db.Queryable<CandyAxFavorite>().Select(t => t.VId).ToListAsync();
        }
    }
}
