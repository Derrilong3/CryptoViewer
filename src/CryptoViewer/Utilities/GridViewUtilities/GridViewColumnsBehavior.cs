using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CryptoViewer.Utilities.GridViewUtilities
{
    internal class GridViewColumnsBehavior
    {
        public static readonly DependencyProperty ColumnsProperty =
    DependencyProperty.RegisterAttached(
        "Columns",
        typeof(ICollection<GridViewColumn>),
        typeof(GridViewColumnsBehavior),
        new UIPropertyMetadata(null, OnColumnsChanged));

        public static ObservableCollection<GridViewColumn> GetColumns(DependencyObject obj)
        {
            return (ObservableCollection<GridViewColumn>)obj.GetValue(ColumnsProperty);
        }

        public static void SetColumns(DependencyObject obj, ObservableCollection<GridViewColumn> value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridView gridView)
            {
                gridView.Columns.Clear();

                if (e.NewValue is ICollection<GridViewColumn> columns)
                {
                    foreach (var column in columns)
                    {
                        gridView.Columns.Add(column);
                    }
                }
            }
        }
    }
}
