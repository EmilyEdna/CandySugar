using CandySugar.Xam.Common.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Xam.Common.AppDTO
{
    public class MenuOption
    {
        public string FuncName { get; set; }
        public string ImageRoute { get; set; }
        public MenuFuncEnum CommandParam { get; set; }
        public static ObservableCollection<MenuOption> InitMenu()
        {
            var data = new ObservableCollection<MenuOption>();
            data.Add(new MenuOption
            {
                FuncName = "小说",
                ImageRoute = "https://konachan.com/data/preview/0a/44/0a4469bedd333d4ca9273510fc283b49.jpg",
                CommandParam = MenuFuncEnum.Novel
            });
            data.Add(new MenuOption
            {
                FuncName = "轻小说",
                ImageRoute = "https://konachan.com/data/preview/f0/61/f0619919b99f30c1dfcf585522a03193.jpg",
                CommandParam = MenuFuncEnum.LightNovel
            });
            data.Add(new MenuOption
            {
                FuncName = "动漫",
                ImageRoute = "https://konachan.com/data/preview/84/2a/842aa5d4262f6620616596fe86c92e3b.jpg",
                CommandParam = MenuFuncEnum.Anime
            });
            data.Add(new MenuOption
            {
                FuncName = "漫画",
                ImageRoute = "https://konachan.com/data/preview/fe/09/fe098505111314d02eefc20940767492.jpg",
                CommandParam = MenuFuncEnum.Manga
            });
            data.Add(new MenuOption
            {
                FuncName = "壁纸",
                ImageRoute = "https://konachan.com/data/preview/f8/71/f8712a1f3919afbfa7a228bcc0a320c8.jpg",
                CommandParam = MenuFuncEnum.Wallpaper
            });
            data.Add(new MenuOption
            {
                FuncName = "茶杯",
                ImageRoute = "https://konachan.com/data/preview/05/77/057714f9929ac269a233a3ad843893a2.jpg",
                CommandParam = MenuFuncEnum.Axgle
            });
            data.Add(new MenuOption
            {
                FuncName = "音乐",
                ImageRoute = "https://konachan.com/data/preview/63/38/6338577be643049d4b37882a25aec477.jpg",
                CommandParam = MenuFuncEnum.Music
            });
            data.Add(new MenuOption
            {
                FuncName = "设置",
                ImageRoute = "https://konachan.com/data/preview/36/12/36125de26ea53a393ff00c48fbe2f626.jpg",
                CommandParam = MenuFuncEnum.Setting
            });
            data.Add(new MenuOption
            {
                FuncName = "关于",
                ImageRoute = "https://konachan.com/data/preview/7e/c7/7ec7a41ae560f3fe70a8f5676e10d8b0.jpg",
                CommandParam = MenuFuncEnum.About
            });
            return data;
        }
    }
}
