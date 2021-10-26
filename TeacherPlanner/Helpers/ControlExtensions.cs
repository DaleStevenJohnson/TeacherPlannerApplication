using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TeacherPlanner.Helpers
{
    public static class ControlExtensions
    {

        public static string GetHeaderOfPreviouslySelectedTab(SelectionChangedEventArgs e)
        {
            TabItem[] tabItems = new TabItem[e.RemovedItems.Count];
            e.RemovedItems.CopyTo(tabItems, 0);
            if (tabItems.Any())
            {
                return tabItems[0].Header.ToString();
            }
            return string.Empty;
        }
    }
}
