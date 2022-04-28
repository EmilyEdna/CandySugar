using CandySugar.Web.Core.Commom;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using SDKColloction.MusicSDK;
using SDKColloction.MusicSDK.ViewModel;
using SDKColloction.MusicSDK.ViewModel.Enums;
using SDKColloction.MusicSDK.ViewModel.Request;
using XExten.Advance.StaticFramework;

namespace CandySugar.Web.Application
{
    /// <summary>
    /// 音乐
    /// </summary>
    [ApiDescriptionSettings("Music", Tag = "音乐", SplitCamelCase = false), NonUnify, Route("Api/Music")]
    public class MusicApplication : IDynamicApiController
    {
        /// <summary>
        /// 查询单曲
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchSingle(string input, Platform SearchType, int Page = 1)
        {
            return await SyncStatic.TryCatch(async () =>
            {
                var Music = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        MusicPlatformType = (MusicPlatformEnum)(int)SearchType,
                        MusicType = MusicTypeEnum.MusicSongItem,
                        Search = new MusicSearch
                        {
                            Page = Page,
                            KeyWord = input
                        }
                    };
                }).RunsAsync();

                return new JsonResult(new Result { Response = Music.SongItemResult });
            }, ex => throw ex);
        }
        /// <summary>
        /// 歌单查询
        /// </summary>
        /// <param name="input"></param>
        /// <param name="SearchType"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchPlayList(string input, Platform SearchType, int Page = 1)
        {
            return await SyncStatic.TryCatch(async () =>
            {
                var Music = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new MusicRequestInput
                {
                    MusicPlatformType = (MusicPlatformEnum)(int)SearchType,
                    MusicType = MusicTypeEnum.MusicSongSheet,
                    Search = new MusicSearch
                    {
                        Page = Page,
                        KeyWord = input
                    }
                };
            }).RunsAsync();

                return new JsonResult(new Result { Response = Music.SongSheetResult });
            }, ex => throw ex);
        }
        /// <summary>
        /// 歌单详情
        /// </summary>
        /// <param name="SongId"></param>
        /// <param name="SearchType"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchPlayListDetail(string SongId, Platform SearchType, int Page = 1)
        {
            return await SyncStatic.TryCatch(async () =>
            {
                //歌单详情
                var Music = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new MusicRequestInput
                {
                    MusicPlatformType = (MusicPlatformEnum)(int)SearchType,
                    MusicType = MusicTypeEnum.MusicSheetDetail,
                    SheetSearch = new MusicSheetSearch
                    {
                        Page = Page,
                        Id = SongId
                    }
                };
            }).RunsAsync();

                return new JsonResult(new Result { Response = Music.SongSheetDetailResult });
            }, ex => throw ex);
        }
        /// <summary>
        /// 关联专辑
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchLinkAlbum(string AlbumId, Platform SearchType)
        {
            return await SyncStatic.TryCatch(async () =>
            {
                var Music = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        MusicPlatformType = (MusicPlatformEnum)(int)SearchType,
                        MusicType = MusicTypeEnum.MusicAlbumDetail,
                        AlbumSearch = new MusicAlbumSearch
                        {
                            AlbumId = AlbumId
                        }
                    };
                }).RunsAsync();

                return new JsonResult(new Result { Response = Music.SongAlbumDetailResult });
            }, ex => throw ex);
        }
        /// <summary>
        /// 播放地址
        /// </summary>
        /// <param name="SongId"></param>
        /// <param name="AlbumId"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Play(string SongId, string AlbumId, Platform SearchType)
        {
            return await SyncStatic.TryCatch(async () =>
            {
                var Music = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        MusicPlatformType = (MusicPlatformEnum)(int)SearchType,
                        MusicType = MusicTypeEnum.MusicPlayAddress,
                        AddressSearch = new MusicPlaySearch
                        {
                            KuGouAlbumId = AlbumId,
                            Dynamic = SongId
                        }
                    };
                }).RunsAsync();
                return new JsonResult(new Result { Response = Music.SongPlayAddressResult });
            }, ex => throw ex);
        }
        /// <summary>
        /// 歌词
        /// </summary>
        /// <param name="SongId"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Lyric(string SongId, Platform SearchType)
        {
            return await SyncStatic.TryCatch(async () =>
            {
                var Music = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        MusicPlatformType = (MusicPlatformEnum)(int)SearchType,
                        MusicType = MusicTypeEnum.MusicLyric,
                        LyricSearch = new MusicLyricSearch
                        {
                            Dynamic = SongId
                        }
                    };
                }).RunsAsync();

                return new JsonResult(new Result { Response = Music.SongLyricResult });
            }, ex => throw ex);
        }
    }
}