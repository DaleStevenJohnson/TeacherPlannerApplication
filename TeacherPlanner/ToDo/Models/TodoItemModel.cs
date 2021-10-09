using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner.ToDo.Models
{
    public class TodoItemModel : ObservableObject
    {
        private bool _isChecked;
        private string _content;

        public event EventHandler ValuesUpdatedEvent;
        public event EventHandler CompletedStatusChangedEvent;
        public TodoItemModel(string content, bool isChecked, int id, int listID)
        {
            
            Content = content;
            IsChecked = isChecked;
            ID = id;
            ListID = listID;
            SubItems = new ObservableCollection<TodoSubItemViewModel>();
        }

        public int ID { get; }
        public int ListID { get; }
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (RaiseAndSetIfChanged(ref _isChecked, value) && ValuesUpdatedEvent != null)
                {
                    ValuesUpdatedEvent.Invoke(null, EventArgs.Empty);
                    CompletedStatusChangedEvent.Invoke(null, EventArgs.Empty);
                }
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

        public ObservableCollection<TodoSubItemViewModel> SubItems { get; set; }
    }
}
