using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Constants;
using TeacherPlanner.Planner.ViewModels;

namespace TeacherPlanner.Helpers
{
    public class ColumnManager
    {
        private readonly Dictionary<string, SortingColumn> _allColumns;
        private SortingColumn _currentSort;

        private readonly Dictionary<string, Func<KeyDateItemViewModel, string?>> _objectPropertyExtractors =
           new Dictionary<string, Func<KeyDateItemViewModel, string?>>();

        private string[] _columnHeaders;

        public List<SortingColumn> ColumnHeaders { get; set; }

        public ICommand SortByColumnCommand { get; set; }

        public event EventHandler? SortingChanged;

        public ColumnManager(string[] columnHeaders, int defaultSortColumnNumber = 0)
        {
            _columnHeaders = columnHeaders;

            _allColumns = new Dictionary<string, SortingColumn>();
            ColumnHeaders = new List<SortingColumn>();

            foreach (string header in _columnHeaders)
            {
                var sortingColumn = new SortingColumn(header, SortDirection.NotSorting);
                ColumnHeaders.Add(sortingColumn);
                _allColumns.Add(header, sortingColumn);
                _objectPropertyExtractors.Add(header, obj => obj.GetProperty(header));
            }

            SortByColumnCommand = new SimpleCommand(SetSortingColumn);

            ColumnHeaders[defaultSortColumnNumber].SortDirection = SortDirection.Descending;
            _currentSort = ColumnHeaders[defaultSortColumnNumber];
        }

        public IEnumerable<KeyDateItemViewModel> Sort(IEnumerable<KeyDateItemViewModel> keyDates)
        {
            Func<KeyDateItemViewModel, string?> propertyExtractor = s => s.Date.ToString("yyyy/MM/dd");

            if (_objectPropertyExtractors.TryGetValue(_currentSort.ColumnName, out var foundPropertyExtractor))
                propertyExtractor = foundPropertyExtractor;

            var orderedKeyDates = _currentSort.SortDirection.Equals(SortDirection.Descending)
               ? keyDates.OrderByDescending(propertyExtractor)
               : keyDates.OrderBy(propertyExtractor);

            return orderedKeyDates;
        }


        private void SetSortingColumn(object columnObject)
        {
            var sortingColumn = columnObject as SortingColumn;
            if (sortingColumn == null)
                return;

            var columnName = sortingColumn.ColumnName;
            if (columnName == null)
                return;

            var columnOfRelevance = _allColumns.GetValueOrDefault(columnName);
            if (columnOfRelevance == null)
                return;

            if (columnOfRelevance.SortDirection != SortDirection.NotSorting)
            {
                columnOfRelevance.SwapSortDirection();
            }
            else
            {
                _currentSort.SortDirection = SortDirection.NotSorting;
                columnOfRelevance.SortDirection = SortDirection.Descending;
                _currentSort = columnOfRelevance;
            }

            SortingChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
