﻿using CandySugar.Controls.UserControls;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandySugar.Controls.PopupControl
{
    /// <summary>
    /// PINPopupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PINPopupWindow : CandyWindow
    {
        public PINPopupWindow()
        {
            InitializeComponent();
        }

        private void PinCompleted(object sender, RoutedEventArgs e)
        {
            if (((PinBox)sender).Password.ToUpper().Equals("EMILY")) this.DialogResult = true;
            else this.DialogResult = false;
            this.Close();
        }
    }
}
