using System;
using System.Collections.Generic;

using System.Text;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Helpers
{
    public class SortingColumn : ObservableObject
    {
        private SortDirection _sortDirection;
        public string ColumnName { get; private set; }

        public SortDirection SortDirection
        {
            get => _sortDirection;
            set => RaiseAndSetIfChanged(ref _sortDirection, value);
        }

        public SortingColumn(string columnName, SortDirection direction)
        {
            ColumnName = columnName;
            SortDirection = direction;
        }

        public void ReverseSort()
        {
            SortDirection = SortDirection.Equals(SortDirection.Ascending)
               ? SortDirection.Descending
               : SortDirection.Ascending;
        }
    }
}
