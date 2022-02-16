﻿using CandySugar.Controls.UserControls;
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

namespace CandySugar.CandyWindows
{
    /// <summary>
    /// CandyLyricWin.xaml 的交互逻辑
    /// </summary>
    public partial class CandyLyricWin : CandyWindow
    {
        public CandyLyricWin()
        {
            InitializeComponent();
        }

        private void LyricMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
