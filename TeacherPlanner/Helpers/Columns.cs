using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Helpers
{
    public class Columns
    {
        private readonly Dictionary<string, SortingColumn> _allColumns;
        private SortingColumn _currentSort;

        private readonly Dictionary<string, Func<ServerAgentModel, string?>> _serverPropertyExtractors =
           new Dictionary<string, Func<ServerAgentModel, string?>>
           {
            {HostName, s => s.HostName},
            {Tag, s => s.Tag},
            {Status, s => s.Status},
            {JtrBuildId, s => s.JtrBuildId},
            {JdVersion, s => s.AggregationServerVersion}
           };

        private readonly Dictionary<string, Func<RunnerAgentModel, string?>> _runnerPropertyExtractors =
           new Dictionary<string, Func<RunnerAgentModel, string?>>
           {
            {HostName, r => r.HostName},
            {Tag, r => r.Tag},
            {Status, r => r.Status},
            {JtrBuildId, r => r.JtrBuildId}
           };

        private const string HostName = "Host Name";
        private const string Tag = "Tag";
        private const string Status = "Status";
        private const string JtrBuildId = "JTR Build ID";
        private const string JdVersion = "JD Version";

        public SortingColumn HostNameColumnHeader { get; set; } =
           new SortingColumn(HostName, SortDirection.NotSorting);
        public SortingColumn TagColumnHeader { get; set; } =
           new SortingColumn(Tag, SortDirection.Descending);
        public SortingColumn StatusColumnHeader { get; set; } =
           new SortingColumn(Status, SortDirection.NotSorting);
        public SortingColumn JtrBuildIdColumnHeader { get; set; } =
           new SortingColumn(JtrBuildId, SortDirection.NotSorting);
        public SortingColumn JdVersionColumnHeader { get; set; } =
           new SortingColumn(JdVersion, SortDirection.NotSorting);

        public ICommand SortByColumnCommand { get; set; }

        public event EventHandler? SortingChanged;

        public Columns()
        {
            SortByColumnCommand = new SimpleCommand(SetSortingColumn);
            _allColumns = new Dictionary<string, SortingColumn>
         {
            {HostName, HostNameColumnHeader},
            {Tag, TagColumnHeader},
            {Status, StatusColumnHeader},
            {JtrBuildId, JtrBuildIdColumnHeader},
            {JdVersion, JdVersionColumnHeader}
         };
            _currentSort = TagColumnHeader;
        }

        public IEnumerable<RunnerAgentModel> Sort(IEnumerable<RunnerAgentModel> runners)
        {
            Func<RunnerAgentModel, string?> propertyExtractor = s => s.Tag;
            if (_runnerPropertyExtractors.TryGetValue(_currentSort.ColumnName, out var foundPropertyExtractor))
                propertyExtractor = foundPropertyExtractor;

            var orderedRunners = _currentSort.SortDirection.Equals(SortDirection.Descending)
               ? runners.OrderByDescending(propertyExtractor)
               : runners.OrderBy(propertyExtractor);

            return orderedRunners;
        }

        public IEnumerable<ServerAgentModel> Sort(IEnumerable<ServerAgentModel> servers)
        {
            Func<ServerAgentModel, string?> propertyExtractor = s => s.Tag;
            if (_serverPropertyExtractors.TryGetValue(_currentSort.ColumnName, out var foundPropertyExtractor))
                propertyExtractor = foundPropertyExtractor;

            var orderedServers = _currentSort.SortDirection.Equals(SortDirection.Descending)
               ? servers.OrderByDescending(propertyExtractor)
               : servers.OrderBy(propertyExtractor);

            return orderedServers;
        }

        private void SetSortingColumn(object columnObject)
        {
            var sortingColumn = (SortingColumn)columnObject;
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
                columnOfRelevance.ReverseSort();
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
