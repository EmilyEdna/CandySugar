using HandyControl.Controls;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CandySugar.Controls.Behaviors
{
    public class SliderBehavior: Behavior<PreviewSlider>
    {
        /// <summary>
        /// Keys down.
        /// </summary>
        private int keysDown;

        #region Dependency property Value

        /// <summary>
        /// 
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(SliderBehavior),
            new PropertyMetadata(default(double), OnValuePropertyChanged));

        #endregion

        #region Dependency property Value

        /// <summary>
        /// DataBindable Command
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(SliderBehavior),
            new PropertyMetadata(null));

        #endregion

        /// <summary>
        /// On behavior attached.
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.KeyUp += this.OnKeyUp;
            this.AssociatedObject.KeyDown += this.OnKeyDown;
            this.AssociatedObject.ValueChanged += this.OnValueChanged;

            base.OnAttached();
        }

        /// <summary>
        /// On behavior detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.KeyUp -= this.OnKeyUp;
            this.AssociatedObject.KeyDown -= this.OnKeyDown;
            this.AssociatedObject.ValueChanged -= this.OnValueChanged;
        }

        /// <summary>
        /// On Value dependency property change.
        /// </summary>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (SliderBehavior)d;
            if (me.AssociatedObject != null)
                me.Value = (double)e.NewValue;
        }

        /// <summary>
        /// Occurs when the slider's value change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Mouse.Captured != null)
            {
                this.AssociatedObject.LostMouseCapture += this.OnLostMouseCapture;
            }
            else
            {
                this.ApplyValue();
            }
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            this.AssociatedObject.LostMouseCapture -= this.OnLostMouseCapture;
            this.ApplyValue();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (this.keysDown-- != 0)
            {
                this.ApplyValue();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            this.keysDown++;
        }

        /// <summary>
        /// Applies the current value in the Value dependency property and raises the command.
        /// </summary>
        private void ApplyValue()
        {
            this.Value = this.AssociatedObject.Value;

            if (this.Command != null)
                this.Command.Execute(this.Value);
        }
    }
}
