using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls
{
    public class AuthorizeHelper
    {
        public static AuthorizeHelper Instance => new AuthorizeHelper();

        /// <summary>
        /// 存储权限
        /// </summary>
        /// <param name="action"></param>
        public async void ApplyPermission(Action action)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (status != PermissionStatus.Granted)
            {
                var Role = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage);
                if (!Role)
                {
                    using (await MaterialDialog.Instance.LoadingSnackbarAsync("请允许访问本机存储"))
                    {
                        await Task.Delay(3000);
                    }
                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }
            }

            if (status == PermissionStatus.Granted)
            {
                action?.Invoke();
            }
            else if (status != PermissionStatus.Unknown)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("未知的权限请求"))
                {
                    await Task.Delay(3000);
                }
            }
        }
    }
}
