using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Models
{
    public class PeriodModel : ObservableObject
    {
        
        
        

        //private int _number = 0;


        public PeriodModel(int number, string classcode, string date)
        {
            Number = number;
            ClassCode = classcode;
            Date = date;

            Row1 = new PeriodRowModel("", "", "");
            Row2 = new PeriodRowModel("", "", "");
            Row3 = new PeriodRowModel("", "", "");
            Row4 = new PeriodRowModel("", "", "");
            Row5 = new PeriodRowModel("", "", "");
            Row6 = new PeriodRowModel("", "", "");
            Row7 = new PeriodRowModel("", "", "");


            PopulateRowModel(Row1);
        }

        public string Date;
        public PeriodRowModel Row1 { get; set; }
        public PeriodRowModel Row2 { get; set; }
        public PeriodRowModel Row3 { get; set; }
        public PeriodRowModel Row4 { get; set; }
        public PeriodRowModel Row5 { get; set; }
        public PeriodRowModel Row6 { get; set; }
        public PeriodRowModel Row7 { get; set; }

        public int Number { get; set; }
        public string ClassCode { get; set; }

        
        public string[] MarginFields { 
            get; 
            set; }
        public string[] MainContentFields { get; set; }
        public string[] SideFields { get; set; }

        private void PopulateRowModel(PeriodRowModel rowModel)
        {
            Random r = new Random();
            rowModel.LeftText = $"{r.Next(1, 100)}";
            rowModel.CenterText = $"{r.Next(1, 100)}";
            rowModel.RightText = $"{r.Next(1, 100)}";
        }

        //public bool HasData { get => Number != 0; }

        //public int Number
        //{
        //    get
        //    {
        //        return _number;
        //    }
        //    set
        //    {
        //        if (RaiseAndSetIfChanged(ref _number, value))
        //        {
        //            _hasData = value != 0;
        //            OnPropertyChanged(nameof(HasData));
        //        }
        //    }
        //}

        //private void DoTheThing()
        //{
        //    Number = 5;
        //}


        //    public bool UseSharedBuildCheckboxIsChecked
        //    {
        //        get => _useSharedBuildCheckboxIsChecked;
        //        set
        //        {
        //            if (RaiseAndSetIfChanged(ref _useSharedBuildCheckboxIsChecked, value))
        //                OnPropertyChanged(nameof(ALocalBuildRadioIsChecked));
        //        }
        //    }

        //    public bool JouleLocalBuildRadioIsChecked
        //    {
        //        get => _jouleLocalBuildRadioIsChecked;
        //        set => RaiseAndSetIfChanged(ref _jouleLocalBuildRadioIsChecked, value, nameof(ALocalBuildRadioIsChecked));
        //    }

        //    public bool JouleTeamCityBuildRadioIsChecked
        //    {
        //        get => _jouleTeamCityBuildRadioIsChecked;
        //        set => RaiseAndSetIfChanged(ref _jouleTeamCityBuildRadioIsChecked, value);
        //    }

        //    public bool ALocalBuildRadioIsChecked => (UseSharedBuildCheckboxIsChecked && SharedLocalBuildRadioIsChecked) ||
        //                                           (!UseSharedBuildCheckboxIsChecked && (JouleLocalBuildRadioIsChecked || AatLocalBuildRadioIsChecked));


    }


   
}
