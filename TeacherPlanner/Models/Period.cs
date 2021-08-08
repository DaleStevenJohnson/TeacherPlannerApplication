using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Models
{
    public class Period : ObservableObject
    {
        
        
        string Date;
        //private int _number = 0;
        

        public Period(int number, string classcode, string date, int rows)
        {
            Number = number;
            ClassCode = classcode;
            Date = date;

            MarginFields = new string[rows];
            MainContentFields = new string[rows];
            SideFields = new string[rows];
            

            PopulateFieldArrays(MarginFields);
            PopulateFieldArrays(MainContentFields);
            PopulateFieldArrays(SideFields);
        }

        public int Number { get; set; }
        public string ClassCode { get; set; }

        
        public string[] MarginFields { get; set; }
        public string[] MainContentFields { get; set; }
        public string[] SideFields { get; set; }

        private void PopulateFieldArrays(string[] fieldArray)
        {
            Random r = new Random();
            for (int i = 0; i < fieldArray.Length; i++)
            {
                fieldArray[i] = $"Test {r.Next(1,100)}";
            }
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
