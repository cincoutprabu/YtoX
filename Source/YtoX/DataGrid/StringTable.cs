//StringTable.cs

using System;
using System.Collections.Generic;

namespace YtoX.DataGrid
{
    public class StringTable : Dictionary<int, StringRow>
    {
        public List<string> ColumnNames { get; set; }

        public StringTable()
        {
            ColumnNames = new List<string>();
        }
    }
}
