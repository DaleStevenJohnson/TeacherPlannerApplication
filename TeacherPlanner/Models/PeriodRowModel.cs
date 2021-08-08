using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Models
{
    public class PeriodRowModel
    {
        public PeriodRowModel(string leftText, string centerText, string rightText) {
            LeftText = leftText;
            CenterText = centerText;
            RightText = rightText;
        }

        public string LeftText { 
            get; 
            set; 
        }
        public string CenterText { get; set; }
        public string RightText { get; set; }
    }

}
