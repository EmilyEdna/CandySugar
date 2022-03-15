﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CandySugar.Droid.Render
{
    /// <summary>
    /// Event arguments for a WebView request to go full-screen.
    /// </summary>
    public class EnterFullScreenRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnterFullScreenRequestedEventArgs"/> class.
        /// </summary>
        /// <param name="view">The Android view that should be displayed in full-screen.</param>
        public EnterFullScreenRequestedEventArgs(View view)
        {
            View = view;
        }

        /// <summary>
        /// Gets the Android view that is to be displayed in full-screen.
        /// </summary>
        public View View { get; }
    }
}