using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Common.Entity.Model;
using CandySugar.Core.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Core.ServiceImpl
{
    public class YinYue : SqlSugarDbContext, IYinYue
    {
        public async Task AddLyric(string SongId, int Platform, string Lyric)
        {
            var model = new CandyPlayLyrics
            {
                Lyric = Lyric,
                Platform = Platform,
                SongId = SongId,
            };
            await Db.Insertable(model).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task AddPlayList(CandyPlayListDto input)
        {
            await Db.Insertable(input.ToMapest<CandyPlayList>()).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task ClearPlayList()
        {
            await Db.Deleteable<CandyPlayList>().ExecuteCommandAsync();
        }

        public async Task<List<CandyPlayLyricsDto>> GetLyrics(string SongId, int Platform)
        {
            var result = await Db.Queryable<CandyPlayLyrics>()
                  .Where(t => t.SongId == SongId)
                  .Where(t => t.Platform == Platform)
                  .FirstAsync();
            if (result == null)
                return await Task.FromResult<List<CandyPlayLyricsDto>>(null);
            else
            {
                List<CandyPlayLyricsDto> dto = new List<CandyPlayLyricsDto>();
                result.Lyric.Split("_").ForArrayEach<string>(item =>
                {
                    var data = item.Split("|");
                    dto.Add(new CandyPlayLyricsDto
                    {
                        Lyric = data.LastOrDefault(),
                        Time = data.FirstOrDefault()
                    });
                });
                return dto;
            }
        }

        public async Task<List<CandyPlayListDto>> GetPlayList()
        {
            return (await Db.Queryable<CandyPlayList>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync()).ToMapest<List<CandyPlayListDto>>();
        }

        public async Task RemovePlayList(Guid input)
        {
            await Db.Deleteable<CandyPlayList>(t => t.PId == input).ExecuteCommandAsync();
        }
    }
}
