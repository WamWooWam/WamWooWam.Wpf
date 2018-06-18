using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WamWooWam.Wpf
{
    public static class Extensions
    {
        public static T FindVisualParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            if (parent == null)
            {
                return null;
            }

            if (parent is T p)
            {
                return p;
            }
            else
            {
                return FindVisualParent<T>(parent);
            }
        }

        public static T FindLogicalParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            DependencyObject parent = LogicalTreeHelper.GetParent(obj);

            if (parent == null)
            {
                return null;
            }

            if (parent is T p)
            {
                return p;
            }
            else
            {
                return FindLogicalParent<T>(parent);
            }
        }
    }
}
