using Caliburn.Micro;
using CryptoViewer.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace CryptoViewer.Utilities.GridViewUtilities
{
    internal class GridViewHandler : PropertyChangedBase
    {
        private GridViewColumnHeader _lastHeaderClicked = null;
        private SortAdorner _sortAdorner = null;

        public ICollectionView View { get; set; }
        public List<GridViewColumn> Columns { get; private set; }

        public void CreateGridColumns(params (string propertyPath, Type type)[] columnTypes)
        {
            Columns = new List<GridViewColumn>();

            Create(columnTypes);

            _lastHeaderClicked = null;
            _sortAdorner = null;

            NotifyOfPropertyChange(nameof(Columns));
        }

        private void Create(params (string propertyPath, Type type)[] columnTypes)
        {
            foreach (var columnType in columnTypes)
            {
                PropertyInfo[] itemProperties = columnType.type.GetProperties();

                foreach (PropertyInfo property in itemProperties)
                {
                    ItemColumnDataAttribute attribute = property.GetCustomAttribute(typeof(ItemColumnDataAttribute)) as ItemColumnDataAttribute;

                    if (attribute == null)
                    {
                        continue;
                    }

                    GridViewColumnHeader header = new GridViewColumnHeader();

                    if (attribute.Group != string.Empty)
                    {
                        var headerStackPanel = new StackPanel();
                        headerStackPanel.Children.Add(new TextBlock
                        {
                            Text = attribute.Group,
                            FontSize = 14,
                            FontWeight = System.Windows.FontWeights.Bold
                        });
                        headerStackPanel.Children.Add(new TextBlock
                        {
                            Text = attribute.Name,
                        });

                        header.Content = headerStackPanel;
                    }
                    else
                    {
                        header.Content = attribute.Name;
                    }

                    header.Command = new RelayCommand(obj => Sort(header));

                    GridViewColumn column = new GridViewColumn()
                    {
                        Header = header,
                        Width = attribute.Width
                    };

                    string path = columnType.propertyPath == string.Empty ? property.Name : $"{columnType.propertyPath}.{property.Name}";

                    Binding binding = new Binding(path);

                    if (attribute.StringFormat != string.Empty)
                    {
                        binding.StringFormat = attribute.StringFormat;
                    }

                    column.DisplayMemberBinding = binding;

                    Columns.Add(column);
                }
            }
        }

        private void Sort(GridViewColumnHeader header)
        {
            if (header == null)
                return;

            if (header.Role == GridViewColumnHeaderRole.Padding)
                return;

            var sortBy = (header.Column.DisplayMemberBinding as Binding)?.Path?.Path;
            if (sortBy == null)
                return;

            if (_lastHeaderClicked != null)
            {
                AdornerLayer.GetAdornerLayer(_lastHeaderClicked).Remove(_sortAdorner);
                View.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Descending;
            if (_lastHeaderClicked == header && _sortAdorner.Direction == newDir)
                newDir = ListSortDirection.Ascending;

            _lastHeaderClicked = header;
            _sortAdorner = new SortAdorner(header, newDir);
            AdornerLayer.GetAdornerLayer(_lastHeaderClicked).Add(_sortAdorner);

            View.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }
}
