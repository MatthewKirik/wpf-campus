using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Models
{
    public class NotifiedCellValue
    {
        public event Action<int, int, string> OnCellChanged;

        private int row;
        private int col;

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnCellChanged?.Invoke(row, col, value);
            }
        }

        public NotifiedCellValue(int row, int col, string value)
        {
            Value = value;
            this.row = row;
            this.col = col;
        }
    }

    //public class NotifiedMatrix<T>
    //{
    //    public event Action<int, int, T> CellValueChanged;

    //    public List<List<NotifiedCellValue<T>>> Cells { get; private set; }

    //    public NotifiedMatrix(T[][] arr)
    //    {
    //        Cells = new List<List<NotifiedCellValue<T>>>();
    //        for (int i = 0; i < arr.Length; i++)
    //        {
    //            var rowValue = new List<NotifiedCellValue<T>>();
    //            for (int j = 0; j < arr[i].Length; j++)
    //            {
    //                var cellValue = new NotifiedCellValue<T>(i, j, arr[i][j]);
    //                cellValue.OnCellChanged += CellValue_OnCellChanged;
    //                rowValue.Add(cellValue);
    //            }
    //            Cells.Add(rowValue);
    //        }
    //    }

    //    private void CellValue_OnCellChanged(int arg1, int arg2, T arg3)
    //    {
    //        CellValueChanged?.Invoke(arg1, arg2, arg3);
    //    }
    //}
}
