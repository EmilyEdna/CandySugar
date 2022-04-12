using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CandySugar.Xam.Common.CrossDownManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CandySugar.Droid.CrossDownImpl
{
    [BroadcastReceiver(Enabled = true, Exported = true), IntentFilter(new[] { DownloadManager.ActionDownloadComplete })]
    public class DownloadCompletedBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var reference = intent.GetLongExtra(DownloadManager.ExtraDownloadId, -1);

            var downloadFile = CrossDownloadManager.Current.Queue.Cast<DownloadFileImplementation>().FirstOrDefault(f => f.Id == reference);
            if (downloadFile == null) return;

            var query = new DownloadManager.Query();
            query.SetFilterById(downloadFile.Id);

            try
            {
                using (var cursor = ((DownloadManager)context.GetSystemService(Context.DownloadService)).InvokeQuery(query))
                {
                    while (cursor != null && cursor.MoveToNext())
                    {
                        ((DownloadManagerImplementation)CrossDownloadManager.Current).UpdateFileProperties(cursor, downloadFile);
                    }
                    cursor?.Close();
                }
            }
            catch (Android.Database.Sqlite.SQLiteException)
            {
                // I lately got an exception that the database was unaccessible ...
            }
        }
    }
}