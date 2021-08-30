﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeacherPlanner.ToDo.Views
{
    /// <summary>
    /// Interaction logic for TodoItemView.xaml
    /// </summary>
    public partial class TodoItemView : UserControl
    {
        public TodoItemView()
        {
            InitializeComponent();
        }

        private void Add_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Foreground = Brushes.LightGreen;
        }

        private void Add_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Foreground = Brushes.Green;
        }
        
         private void Delete_Button_MouseEnter(object sender, MouseEventArgs e)
         {
            Button button = sender as Button;
            button.Foreground = Brushes.Pink;
        }
        private void Delete_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Foreground = Brushes.Red;
        }
    }
}
