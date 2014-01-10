#region Namespaces

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;

#endregion

namespace MTDBCreator.Helpers
{
    public static class WindowHelper
    {
        public static int GetPercentage(int current, int total)
        {
            return Convert.ToInt32(100 * ((double)current / (double)total));
        }

        // Remove
        public static bool SyncBindingExpression(Control control, BindingExpressionSyncDirection direction)
        {
            bool isChanged = false;
            BindingExpression bindingExpression = null;

            if (control is TextBox)
            {
                bindingExpression = control.GetBindingExpression(TextBox.TextProperty);
            }
            else if (control is CheckBox)
            {
                bindingExpression = control.GetBindingExpression(CheckBox.IsCheckedProperty);
            }
            else if (control is RadioButton)
            {
                bindingExpression = control.GetBindingExpression(RadioButton.IsCheckedProperty);
            }

            if (bindingExpression != null)
            {
                switch (direction)
                {
                    case BindingExpressionSyncDirection.Source:
                        {
                            bindingExpression.UpdateSource();

                            isChanged = true;

                            break;
                        }
                    case BindingExpressionSyncDirection.Target:
                        {
                            bindingExpression.UpdateTarget();

                            break;
                        }
                }
            }

            return isChanged;
        }
    }

    // Remove
    public enum BindingExpressionSyncDirection
    {
        Source,
        Target
    }
}
