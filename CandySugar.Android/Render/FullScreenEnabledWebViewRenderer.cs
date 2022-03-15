using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CandySugar.App.Controls.Controls;
using CandySugar.Droid.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FullScreenEnabledWebView), typeof(FullScreenEnabledWebViewRenderer))]
namespace CandySugar.Droid.Render
{
    public class FullScreenEnabledWebViewRenderer : WebViewRenderer
    {
     
            private FullScreenEnabledWebView _webView;

            /// <summary>
            /// Initializes a new instance of the <see cref="FullScreenEnabledWebViewRenderer"/> class.
            /// </summary>
            /// <param name="context">An Android context.</param>
            public FullScreenEnabledWebViewRenderer(Context context) : base(context)
            {
            }

            /// <inheritdoc/>
            protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
            {
                base.OnElementChanged(e);
                _webView = (FullScreenEnabledWebView)e.NewElement;
            }

            /// <summary>
            /// Creates a <see cref="FormsWebChromeClient"/> that implements the necessary callbacks to support
            /// full-screen operation.
            /// </summary>
            /// <returns>A <see cref="FullScreenEnabledWebChromeClient"/>.</returns>
            protected override FormsWebChromeClient GetFormsWebChromeClient()
            {
                var client = new FullScreenEnabledWebChromeClient();
                client.EnterFullscreenRequested += OnEnterFullscreenRequested;
                client.ExitFullscreenRequested += OnExitFullscreenRequested;
                return client;
            }

            /// <summary>
            /// Executes the full-screen command on the <see cref="FullScreenEnabledWebView"/> if available. The
            /// Xamarin view to display in full-screen is sent as a command parameter.
            /// </summary>
            /// <param name="sender">The event sender.</param>
            /// <param name="eventArgs">The event arguments.</param>
            private void OnEnterFullscreenRequested(
                object sender,
                EnterFullScreenRequestedEventArgs eventArgs)
            {
                if (_webView.EnterFullScreenCommand != null && _webView.EnterFullScreenCommand.CanExecute(null))
                {
                    _webView.EnterFullScreenCommand.Execute(eventArgs.View.ToView());
                }
            }

            /// <summary>
            /// Executes the exit full-screen command on th e <see cref="FullScreenEnabledWebView"/> if available.
            /// The command is passed no parameters.
            /// </summary>
            /// <param name="sender">The event sender.</param>
            /// <param name="eventArgs">The event arguments.</param>
            private void OnExitFullscreenRequested(object sender, EventArgs eventArgs)
            {
                if (_webView.ExitFullScreenCommand != null && _webView.ExitFullScreenCommand.CanExecute(null))
                {
                    _webView.ExitFullScreenCommand.Execute(null);
                }
            }
        }
}