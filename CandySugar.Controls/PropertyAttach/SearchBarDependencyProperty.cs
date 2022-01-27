using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CandySugar.Controls.PropertyAttach
{
    public class SearchBarDependencyProperty
    {

        #region Fields

        /// <summary>
        /// 文本框和Visual画刷对应的字典
        /// </summary>
        private static readonly Dictionary<SearchBar, VisualBrush> TxtBrushes = new Dictionary<SearchBar, VisualBrush>();

        #endregion Fields

        #region Attached DependencyProperty
        /// <summary>
        /// 获取占位符
        /// </summary>
        /// <param name="obj">占位符所在的对象</param>
        /// <returns>占位符</returns>
        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceHolderProperty);
        }

        /// <summary>
        /// 设置占位符
        /// </summary>
        /// <param name="obj">占位符所在的对象</param>
        /// <param name="value">占位符</param>
        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceHolderProperty, value);
        }

        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.RegisterAttached("PlaceHolder", typeof(string), typeof(TextBoxDependencyProperty), new PropertyMetadata("", OnPlaceholderChanged));

        #endregion

        #region Events Handling

        /// <summary>
        /// 占位符改变的响应
        /// </summary>
        /// <param name="d">来源</param>
        /// <param name="e">改变信息</param>
        public static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var txt = d as SearchBar;
            if ((txt != null) && (!TxtBrushes.ContainsKey(txt)))
            {
                var placeholderTextBlock = new TextBlock();
                var binding = new Binding
                {
                    Source = txt,
                    //绑定到附加属性
                    Path = new PropertyPath("(0)", PlaceHolderProperty)
                };
                placeholderTextBlock.SetBinding(TextBlock.TextProperty, binding);
                placeholderTextBlock.FontStyle = FontStyles.Normal;
                placeholderTextBlock.Foreground = new SolidColorBrush(Colors.DeepPink);

                var placeholderVisualBrush = new VisualBrush
                {
                    AlignmentX = AlignmentX.Left,
                    Stretch = Stretch.None,
                    Visual = placeholderTextBlock,
                    Opacity = 0.3d
                };

                txt.Background = placeholderVisualBrush;

                txt.TextChanged += PlaceholderTextBox_TextChanged;
                txt.Unloaded += PlaceholderTextBox_Unloaded;

                TxtBrushes.Add(txt, placeholderVisualBrush);
            }
        }

        /// <summary>
        /// 文本变化的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PlaceholderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = sender as SearchBar;
            if ((txt != null) && (TxtBrushes.ContainsKey(txt)))
            {
                var placeholderVisualBrush = TxtBrushes[txt];
                txt.Background = string.IsNullOrEmpty(txt.Text) ? placeholderVisualBrush : null;
            }
        }

        /// <summary>
        /// 文本框卸载的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PlaceholderTextBox_Unloaded(object sender, RoutedEventArgs e)
        {
            var txt = sender as SearchBar;
            if ((txt != null) && (TxtBrushes.ContainsKey(txt)))
            {
                TxtBrushes.Remove(txt);

                txt.TextChanged -= PlaceholderTextBox_TextChanged;
                txt.Unloaded -= PlaceholderTextBox_Unloaded;
            }
        }

        #endregion Events Handling
    }
}
