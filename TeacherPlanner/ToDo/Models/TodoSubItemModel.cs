using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.ToDo.Models
{
    public class TodoSubItemModel : ObservableObject
    {
        private bool _isChecked;
        private string _content;

        public event EventHandler ValuesUpdatedEvent;

        public TodoSubItemModel(string content, bool isChecked, int id)
        {
            ID = id;
            Content = content;
            IsChecked = isChecked;
        }
        public int ID { get; }
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (RaiseAndSetIfChanged(ref _isChecked, value) && ValuesUpdatedEvent != null)
                    ValuesUpdatedEvent.Invoke(null, EventArgs.Empty);
            }
        }
        public string Content
        {
            get => _content;
            set
            {
                if (RaiseAndSetIfChanged(ref _content, value) && ValuesUpdatedEvent != null)
                    ValuesUpdatedEvent.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
