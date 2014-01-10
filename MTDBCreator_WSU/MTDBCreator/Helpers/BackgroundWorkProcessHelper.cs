using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.Database;

namespace MTDBCreator.Helpers
{
    internal static class BackgroundWorkProcessHelper
    {
        internal static object Process(IBackgroundWorkHelper backgroundWorkHelper)
        {
            ProcessWindow processWindow = new ProcessWindow(backgroundWorkHelper, Application.Current.MainWindow);

            if (processWindow.ShowDialog() == true)
            {
                return backgroundWorkHelper.Result;
            }

            return null;
        }
    }
}