using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Models
{
    public class PeriodRowModel
    {
        public PeriodRowModel(string leftText, string centerText, string rightText) {
            RowText = new string[3];
            RowText[0] = leftText;
            RowText[1] = centerText;
            RowText[2] = rightText;
        }
        internal string[] RowText;
        public string LeftText 
        {
            get { return RowText[0]; }
            set => RowText[0] = value.Trim().Replace("`", ""); 
        }
        public string CenterText
        {
            get { return RowText[1]; }
            set => RowText[1] = value.Trim().Replace("`", "");
        }
        public string RightText
        {
            get { return RowText[2]; }
            set => RowText[2] = value.Trim().Replace("`", "");
        }
    }

}
