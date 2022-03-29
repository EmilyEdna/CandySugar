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
        }

        #region Field
        public int AId;
        private GalActorDescEnum Desc;
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            AId = Convert.ToInt32(parameters.GetValue<string>("AId"));
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
        #endregion

        #region Method
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast, nameof(CandyAxgleCateViewModel), Method);
        }
        public async void Category()
        {
            try
            {
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
                this.CategoryList = new ObservableCollection<CalActorCategoryList>(Cate.CategoryListsResults.CategaryList);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("Category")))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void Search()
        { 
        
        }
        #endregion

        #region Command
        public ICommand SearchCommand => new DelegateCommand<string>(input =>
        {

        });
        public ICommand ComboSelectCommand => new DelegateCommand<dynamic>(input =>
        {
            var x = input;
        });

        
        #endregion
    }
}
