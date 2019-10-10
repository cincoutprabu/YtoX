//DataGridHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;

namespace YtoX.DataGrid
{
    public class DataGridHelper
    {
        #region Methods

        public static void Bind(ref System.Windows.Controls.DataGrid dataGrid, StringTable table)
        {
            dataGrid.ItemsSource = table.Values;
            dataGrid.AutoGenerateColumns = false;

            dataGrid.Columns.Clear();
            foreach (string columnName in table.ColumnNames)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = columnName;
                textColumn.Binding = new Binding();
                ((Binding)textColumn.Binding).Converter = new ColumnValueSelector();
                ((Binding)textColumn.Binding).ConverterParameter = columnName;
                dataGrid.Columns.Add(textColumn);
            }
        }

        #endregion
    }
}
