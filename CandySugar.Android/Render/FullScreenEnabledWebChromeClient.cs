using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Android;

namespace CandySugar.Droid.Render
{
    public class FullScreenEnabledWebChromeClient : FormsWebChromeClient
    {
        /// <summary>
        /// Triggered when the content requests full-screen.
        /// </summary>
        public event EventHandler<EnterFullScreenRequestedEventArgs> EnterFullscreenRequested;

        /// <summary>
        /// Triggered when the content requests exiting full-screen.
        /// </summary>
        public event EventHandler ExitFullscreenRequested;

        /// <inheritdoc />
        public override void OnHideCustomView()
        {
            ExitFullscreenRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public override void OnShowCustomView(View view, ICustomViewCallback callback)
        {
            EnterFullscreenRequested?.Invoke(this, new EnterFullScreenRequestedEventArgs(view));
        }
    }
}