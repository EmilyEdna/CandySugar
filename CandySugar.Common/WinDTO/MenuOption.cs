using CandySugar.Common.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.WinDTO
{
    public class MenuOption
    {
        public string FuncName { get; set; }
        public string ImageRoute { get; set; }
        public MenuFuncEunm CommandParam { get; set; }
        public static ObservableCollection<MenuOption> InitMenu()
        {
            var data = new ObservableCollection<MenuOption>();
            data.Add(new MenuOption
            {
                FuncName = "小说(Ctrl+N)",
                ImageRoute = "https://konachan.com/data/preview/0a/44/0a4469bedd333d4ca9273510fc283b49.jpg",
                CommandParam = MenuFuncEunm.Novel
            });
            data.Add(new MenuOption
            {
                FuncName = "轻小说(Ctrl+L)",
                ImageRoute = "https://konachan.com/data/preview/f0/61/f0619919b99f30c1dfcf585522a03193.jpg",
                CommandParam = MenuFuncEunm.LightNovel
            });
            data.Add(new MenuOption
            {
                FuncName = "动漫(Ctrl+A)",
                ImageRoute = "https://konachan.com/data/preview/84/2a/842aa5d4262f6620616596fe86c92e3b.jpg",
                CommandParam = MenuFuncEunm.Anime
            });
            data.Add(new MenuOption
            {
                FuncName = "漫画(Ctrl+M)",
                ImageRoute = "https://konachan.com/data/preview/fe/09/fe098505111314d02eefc20940767492.jpg",
                CommandParam = MenuFuncEunm.Manga
            });
            data.Add(new MenuOption
            {
                FuncName = "壁纸(Ctrl+W)",
                ImageRoute = "https://konachan.com/data/preview/f8/71/f8712a1f3919afbfa7a228bcc0a320c8.jpg",
                CommandParam = MenuFuncEunm.Wallpaper
            });
            data.Add(new MenuOption
            {
                FuncName = "音乐(Ctrl+Y)",
                ImageRoute = "https://konachan.com/data/preview/63/38/6338577be643049d4b37882a25aec477.jpg",
                CommandParam = MenuFuncEunm.Music
            });
            data.Add(new MenuOption
            {
                FuncName = "茶杯(Ctrl+D)",
                ImageRoute = "https://konachan.com/data/preview/d5/21/d521a54a37b255b44c63ebb3803e7710.jpg",
                CommandParam = MenuFuncEunm.Avgle
            });
            return data;
        }
    }
}
