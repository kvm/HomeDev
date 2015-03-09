using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Flashlight.Managers
{
    public static class ExceptionHandler
    {

        public static void LogException(Exception ex)
        {
            // e.g. MarkedUp.AnalyticClient.Error(ex.Message, ex);
        }

        /// <summary>
        /// Handles failure for application exception on UI thread (or initiated from UI thread via async void handler)
        /// </summary>
        public static void HandleException(Exception ex)
        {

            LogException(ex);

            var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var dialog = new MessageDialog(GetDisplayMessage(ex), "Unknown Error");
                await dialog.ShowAsync();
            });

        }

        /// <summary>
        /// Gets the error message to display from an exception
        /// </summary>
        public static string GetDisplayMessage(Exception ex)
        {
            string errorMessage;
#if DEBUG
            errorMessage = (ex.Message + " " + ex.StackTrace);
#else
                errorMessage = "An unknown error has occurred, please try again";
#endif
            return errorMessage;
        }

    }
}
