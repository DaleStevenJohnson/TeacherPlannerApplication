using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class PeriodModel
    {
        private readonly string _delimiter = "`";
        public PeriodModel(int number, string classcode)
        {
            Number = number;
            ClassCode = classcode;
            //Date = date;
            Rows = new PeriodRowModel[7];
        }

        public string Date;
        private PeriodRowModel[] Rows;
        public PeriodRowModel Row1
        {
            get { return Rows[0]; }
            set => Rows[0] = value;
        }
        public PeriodRowModel Row2
        {
            get { return Rows[1]; }
            set => Rows[1] = value;
        }
        public PeriodRowModel Row3
        {
            get { return Rows[2]; }
            set => Rows[2] = value;
        }
        public PeriodRowModel Row4
        {
            get { return Rows[3]; }
            set => Rows[3] = value;
        }
        public PeriodRowModel Row5
        {
            get { return Rows[4]; }
            set => Rows[4] = value;
        }
        public PeriodRowModel Row6
        {
            get { return Rows[5]; }
            set => Rows[5] = value;
        }
        public PeriodRowModel Row7
        {
            get { return Rows[6]; }
            set => Rows[6] = value;
        }

        public int Number { get; set; }
        public string ClassCode { get; set; }

        public void LoadData(string[] periodData)
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                string[] rowData = periodData[i].Split(_delimiter);
                Rows[i] = new PeriodRowModel(rowData[0], rowData[1], rowData[2]);
            }
        }
        internal string PackageSaveData()
        {
            string saveData = $"{ClassCode}";
            for (int i = 0; i < Rows.Length; i++)
            {
                string[] content = FileHandlingHelper.SanitiseStrings(Rows[i].RowText);
                string row = $"\n{content[0]}{_delimiter}{content[1]}{_delimiter}{content[2]}";
                saveData += row;
            }
            return saveData;
        }
    }
}
