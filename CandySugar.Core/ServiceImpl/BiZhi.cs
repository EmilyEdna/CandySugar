using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Common.Entity.Model;
using CandySugar.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Core.ServiceImpl
{
    public class BiZhi : SqlSugarDbContext, IBiZhi
    {
        public Task AddFavorite(CandyWallpaperDto input)
        {
            var entity = input.ToMapest<CandyFavorite>();
            var incloud = Db.Queryable<CandyFavorite>().Where(t => t.Id == input.Id).First();
            if (incloud != null)
                return Task.CompletedTask;
            return Db.Insertable(entity).ExecuteCommandAsync();
        }

        public async  Task<List<long>> GetAllFavorite()
        {
            return await Db.Queryable<CandyFavorite>().Select(t => t.Id).ToListAsync();
        }

        public async Task<CandyWallpaperPageDto> GetFavorite(string lable, int pageIndex)
        {
            var Total = 0;
            var result = Db.Queryable<CandyFavorite>()
                .WhereIF(!lable.IsNullOrEmpty(), t => t.Label.Contains(lable))
                .ToPageList(pageIndex, 16, ref Total);
            Total = (Total + 16 - 1) / 16;
            return await Task.FromResult(new CandyWallpaperPageDto
            {
                Total = Total,
                Result = result.ToMapest<List<CandyWallpaperDto>>()
            });
        }

        public Task RemoveFavorite(long Id)
        {
            return Db.Deleteable<CandyFavorite>(t => t.Id == Id).ExecuteCommandAsync();
        }
    }
}
