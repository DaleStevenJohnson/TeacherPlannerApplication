using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeacherPlanner.Constants
{
    public enum KeyDateTypes
    {
        CPD = 10,
        Deadline = 20,
        Event = 30, 
        Meeting = 40,
        ParentsEvening = 50,
        Report = 60,
        Other = 100,
        
    }

    public static class KeyDateTypeDefinitions
    {
        private static readonly Dictionary<KeyDateTypes, string> KeyDateTypeNames = new Dictionary<KeyDateTypes, string>
        {
            { KeyDateTypes.CPD, "CPD"},
            { KeyDateTypes.Deadline, "Deadline"},
            { KeyDateTypes.Event, "Event"},
            { KeyDateTypes.Meeting, "Meeting"},
            { KeyDateTypes.ParentsEvening, "Parent's Evening"},
            { KeyDateTypes.Report, "Report"},
            { KeyDateTypes.Other, "Other"},
        };

        public static string GetKeyDateTypeName(this KeyDateTypes keyDateType)
        {
            return KeyDateTypeNames[keyDateType];
        }

        public static KeyDateTypes GetKeyDateTypeEnum(string keyDateType)
        {
            return KeyDateTypeNames.FirstOrDefault(x => x.Value == keyDateType).Key;
        }

    }
}
