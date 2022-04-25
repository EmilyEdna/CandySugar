using CandySugar.Xam.Common;
using CandySugar.Xam.Common.AppDTO;
using CandySugar.Xam.Core.Service;
using SDKColloction.GalActorSDK;
using SDKColloction.GalActorSDK.ViewModel;
using SDKColloction.GalActorSDK.ViewModel.Eunms;
using SDKColloction.GalActorSDK.ViewModel.Request;
using SDKColloction.GalActorSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.Xam.Common.DTO;
using CandySugar.App.Controls.Views.Axgle;
using XF.Material.Forms.Models;

namespace CandySugar.App.Controls.ViewModels.AxgleModel
{
    public class CandyAxgleCateViewModel : ViewModelNavigatBase
    {
      
        public CandyAxgleCateViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new GalActorProxy
            {
                IP = Soft.ProxyIP,
                PassWord = Soft.ProxyPwd,
                Port = Soft.ProxyPort,
                UserName = Soft.ProxyAccount
            };
            Desc = GalActorDescEnum.MostViewed;
            this.PageIndex = 1;
            this.Refresh = false;
            this.IsBusy = false;
            CandyLog = Resolve<ILoger>();
        }

        #region Field
        private readonly GalActorProxy Proxy;
        private int AId;
        private GalActorDescEnum Desc;
        private string KeyWord;
        private readonly ILoger CandyLog;
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            AId = Convert.ToInt32(parameters.GetValue<string>("AId"));
            Category();
        }
        protected override void OnViewLaunch()
        {
            Combo = new ObservableCollection<ComboDto>
            {
                new ComboDto
                {
                    Description = 0,
                    Name = "上次观看"
                },
                new ComboDto
                {
                    Description = 1,
                    Name = "最新的"
                },
                new ComboDto
                {
                    Description = 2,
                    Name = "最多观看"
                },
                new ComboDto
                {
                    Description = 3,
                    Name = "高评分"
                },
                new ComboDto
                {
                    Description = 4,
                    Name = "最多收藏"
                },
                new ComboDto{
                   Description = 5,
                   Name="长时常"
                }
            };
            Menu = new ObservableCollection<MaterialMenuItem> {
             new MaterialMenuItem { Text ="收藏" }
            };
        }
        #endregion

        #region Property
        private ObservableCollection<CalActorCategoryList> _CategoryList;
        public ObservableCollection<CalActorCategoryList> CategoryList
        {
            get => _CategoryList;
            set => SetProperty(ref _CategoryList, value);
        }

        private ObservableCollection<MaterialMenuItem> _Menu;
        public ObservableCollection<MaterialMenuItem> Menu
        {
            get => _Menu;
            set => SetProperty(ref _Menu, value);
        }

        private ObservableCollection<ComboDto> _Combo;
        public ObservableCollection<ComboDto> Combo
        {
            get => _Combo;
            set => SetProperty(ref _Combo, value);
        }

        private int _PageIndex;
        public int PageIndex
        {
            get => _PageIndex;
            set => SetProperty(ref _PageIndex, value);
        }

        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }

        private bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyAxgleCateViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast, nameof(CandyAxgleCateViewModel), Method);
        }
        public async void Category(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore)  IsBusy = true; else Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var Cate = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.GalCategory,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Category = new CalActorCategory
                        {
                            CId = AId,
                            Desc = Desc,
                            Page = this.PageIndex,
                        }
                    };
                }).RunsAsync();
                this.Total = Cate.CategoryListsResults.Total;
                if (IsLoadMore)
                {
                    IsBusy = false;
                    if (this.CategoryList == null)
                        this.CategoryList = new ObservableCollection<CalActorCategoryList>(Cate.CategoryListsResults.CategaryList);
                    else
                    {
                        Cate.CategoryListsResults.CategaryList.ForEach(item =>
                        {
                            this.CategoryList.Add(item);
                        });
                    }
                }
                else
                {
                    Refresh = false;
                    this.CategoryList = new ObservableCollection<CalActorCategoryList>(Cate.CategoryListsResults.CategaryList);
                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Category",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Search(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) IsBusy = true; else Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var Seach = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.GalSearch,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Search = new GalActorSearch
                        {
                            Page = this.PageIndex,
                            KeyWord = KeyWord
                        }
                    };
                }).RunsAsync();
                this.Total = Seach.SearchResult.Total;
                if (IsLoadMore)
                {
                    IsBusy = false;
                    if (this.CategoryList == null)
                        this.CategoryList = new ObservableCollection<CalActorCategoryList>(Seach.SearchResult.SearchList.ToMapest<List<CalActorCategoryList>>());
                    else
                    {
                        Seach.SearchResult.SearchList.ToMapest<List<CalActorCategoryList>>().ForEach(item =>
                        {
                            this.CategoryList.Add(item);
                        });
                    }
                }
                else
                {
                    Refresh = false;
                    this.CategoryList = new ObservableCollection<CalActorCategoryList>(Seach.SearchResult.SearchList.ToMapest<List<CalActorCategoryList>>());
                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Search",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Like(CalActorCategoryList input) 
        {
           await Resolve<IAXLiShi>().Insert(input.ToMapest<CandyAXLiShiDto>());
        }
        public async void Navigation(string input)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Route", input);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyAxglePlayView), UriKind.Relative), param);
        }
        #endregion

        #region Command
        public ICommand SearchCommand => new DelegateCommand<string>(input =>
        {
            KeyWord = input;
            this.PageIndex = 1;
            Search();
        });
        public ICommand ComboSelectCommand => new DelegateCommand<dynamic>(input =>
        {
            var box = (ComboDto)input;
            if (box != null && !KeyWord.IsNullOrEmpty())
            {
                Desc = (GalActorDescEnum)box.Description;
                KeyWord=String.Empty;
                PageIndex = 1;
                Category();
            }
        });
        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            this.PageIndex = 1;
            if (KeyWord.IsNullOrEmpty()) Category();
            else Search();
        });
        public ICommand ShowMoreCommand => new DelegateCommand(() => {

            PageIndex += 1;
            if (PageIndex <= Total)
            {
                if (!KeyWord.IsNullOrEmpty()) Search(true);
                else Category(true);
            }
        });
        public ICommand PlayCommand => new DelegateCommand<string>(input => {
            if (!input.IsNullOrEmpty())
                Navigation(input);
        });
        public ICommand LikeCommand => new DelegateCommand<CalActorCategoryList>(input => 
        {
            if (input != null)
                Like(input);
        });
        public ICommand GotoLikeCommand => new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync(new Uri(nameof(CandyAxgleLikeView), UriKind.Relative));
        });
        #endregion
    }
}
