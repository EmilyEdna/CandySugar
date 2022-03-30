using CandySugar.Xam.Common;
using CandySugar.Xam.Common.AppDTO;
using GalActor.SDK;
using GalActor.SDK.ViewModel;
using GalActor.SDK.ViewModel.Eunms;
using GalActor.SDK.ViewModel.Request;
using GalActor.SDK.ViewModel.Response;
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

namespace CandySugar.App.Controls.ViewModels.AxgleModel
{
    public class CandyAxgleCateViewModel : ViewModelNavigatBase
    {
        private readonly GalActorProxy Proxy;
        public CandyAxgleCateViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new GalActorProxy
            {
                IP = Soft.ProxyIP,
                PassWord = Soft.ProxyPwd,
                Port = Soft.ProxyPort,
                UserName = Soft.ProxyAccount
            };
            Desc = GalActorDescEnum.Latest;
            this.PageIndex = 1;
            this.Refresh = false;
            this.IsBusy = false;
        }

        #region Field
        private int AId;
        private GalActorDescEnum Desc;
        private string KeyWord;
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            AId = Convert.ToInt32(parameters.GetValue<string>("AId"));
            Category();
        }
        protected override void OnViewLaunch()
        {
            Combo = new ObservableCollection<GalComboDto>
            {
                new GalComboDto
                {
                    Description = 0,
                    Name = "上次观看"
                },
                new GalComboDto
                {
                    Description = 1,
                    Name = "最新的"
                },
                new GalComboDto
                {
                    Description = 2,
                    Name = "最多观看"
                },
                new GalComboDto
                {
                    Description = 3,
                    Name = "高评分"
                },
                new GalComboDto
                {
                    Description = 4,
                    Name = "最多收藏"
                },
                new GalComboDto{
                   Description = 5,
                   Name="长时常"
                }
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

        private ObservableCollection<GalComboDto> _Combo;
        public ObservableCollection<GalComboDto> Combo
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
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast, nameof(CandyAxgleCateViewModel), Method);
        }
        public async void Category(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) Refresh = true; else IsBusy = true;
                await Task.Delay(Soft.WaitSpan);
                var Cate = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.Category,
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
                    Refresh = false;
                    this.CategoryList = new ObservableCollection<CalActorCategoryList>(Cate.CategoryListsResults.CategaryList);
                }
                else
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
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("Category")))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Search(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) Refresh = true; else IsBusy = true;
                await Task.Delay(Soft.WaitSpan);
                var Seach = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.Search,
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
                    Refresh = false;
                    this.CategoryList = new ObservableCollection<CalActorCategoryList>(Seach.SearchResult.SearchList.ToMapest<List<CalActorCategoryList>>());
                }
                else
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
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("Category")))
                {
                    await Task.Delay(3000);
                }
            }
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
            var box = (GalComboDto)input;
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
        public ICommand PlayCommand => new DelegateCommand<string>(input => { });
        public ICommand LikeCommand => new DelegateCommand<CalActorCategoryList>(input => { });
        #endregion
    }
}
