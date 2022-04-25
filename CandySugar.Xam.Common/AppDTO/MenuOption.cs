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
                CommandParam = MenuFuncEnum.Novel
            });
            data.Add(new MenuOption
            {
                FuncName = "轻小说",
                CommandParam = MenuFuncEnum.LightNovel
            });
            data.Add(new MenuOption
            {
                FuncName = "动漫",
                CommandParam = MenuFuncEnum.Anime
            });
            data.Add(new MenuOption
            {
                FuncName = "漫画",
                CommandParam = MenuFuncEnum.Manga
            });
            data.Add(new MenuOption
            {
                FuncName = "壁纸",
                CommandParam = MenuFuncEnum.Wallpaper
            });
            data.Add(new MenuOption
            {
                FuncName = "茶杯",
                CommandParam = MenuFuncEnum.Axgle
            });
            data.Add(new MenuOption
            {
                FuncName = "ACG",
                CommandParam = MenuFuncEnum.HAnime
            });
            data.Add(new MenuOption
            {
                FuncName = "音乐",
                CommandParam = MenuFuncEnum.Music
            });
            data.Add(new MenuOption
            {
                FuncName = "设置",
                CommandParam = MenuFuncEnum.Setting
            });
            data.Add(new MenuOption
            {
                FuncName = "日志",
                CommandParam = MenuFuncEnum.Loger
            });
            data.Add(new MenuOption
            {
                FuncName = "关于",
                CommandParam = MenuFuncEnum.About
            });
            return data;
        }
    }
}
