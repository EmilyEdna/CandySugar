using CandySugar.Controls.ControlViewModel;
using CandySugar.Controls.UserControls;
using CandySugar.Core.CandyUtily;
using System;
using System.Windows.Input;
using HTag = HandyControl.Controls.Tag;
using System.Linq;
using XExten.Advance.LinqFramework;
using CandySugar.Controls.UIElementHelper;
using System.Collections.Generic;
using CandySugar.Common.WinDTO;

namespace CandySugar.WPF.CandyWindows
{
    /// <summary>
    /// CandyAcgSearchProvider.xaml 的交互逻辑
    /// </summary>
    public partial class CandyAcgProviderWin : CandyWindow
    {
        public CandyAcgProviderWin()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<NormalHeaderViewModel>().Basic();
        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void TypeSelected(object sender, EventArgs e)
        {
            HTag ht = (HTag)sender;
            if (!ht.IsSelected) return;
            UlHelper.FindVisualChild<HTag>(TypeList).ForEnumerEach(item =>
            {
                if (item.Content.Equals(ht.Content))
                {
                    item.IsSelected = true;
                    HAcgOption.Type = item.Content.ToString();
                }
                else item.IsSelected = false;
            });
        }

        private void BrandSelected(object sender, EventArgs e)
        {
            HTag ht = (HTag)sender;
            if (ht.IsSelected)
            {
                if (!HAcgOption.Brands.Contains(ht.Content.ToString()))
                    HAcgOption.Brands.Add(ht.Content.ToString());
            }
            else
            {
                HAcgOption.Brands.Remove(ht.Content.ToString());
            }
        }

        private void TagSelected(object sender, EventArgs e)
        {
            HTag ht = (HTag)sender;
            if (ht.IsSelected)
            {
                if (!HAcgOption.Tags.Contains(ht.Content.ToString()))
                    HAcgOption.Tags.Add(ht.Content.ToString());
            }
            else
            {
                HAcgOption.Tags.Remove(ht.Content.ToString());
            }
        }
    }
}
