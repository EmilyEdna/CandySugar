using CandySugar.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Core.Service
{
    public interface IYinYue
    {
        Task AddPlayList(CandyPlayListDto input);
        Task<List<CandyPlayListDto>> GetPlayList();
        Task ClearPlayList();
        Task RemovePlayList(Guid input);
        Task<List<CandyPlayLyricsDto>> GetLyrics(string SongId, int Platform);
        Task AddLyric(string SongId, int Platform, string Lyric);
    }
}
