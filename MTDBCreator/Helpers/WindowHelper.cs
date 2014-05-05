#region Namespaces

using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

#endregion

namespace MTDBCreator.Helpers
{
    public static class WindowHelper
    {
        public static int GetPercentage(int current, int total)
        {
            return Convert.ToInt32(100 * (current / (double)total));
        }

        // Remove
        public static bool SyncBindingExpression(Control control, BindingExpressionSyncDirection direction)
        {
            var isChanged = false;
            BindingExpression bindingExpression = null;

            if (control is TextBox)
            {
                bindingExpression = control.GetBindingExpression(TextBox.TextProperty);
            }
            else if (control is CheckBox)
            {
                bindingExpression = control.GetBindingExpression(ToggleButton.IsCheckedProperty);
            }
            else if (control is RadioButton)
            {
                bindingExpression = control.GetBindingExpression(ToggleButton.IsCheckedProperty);
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
