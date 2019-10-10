//DataTypeMetaData.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public class DataTypeInfo
    {
        #region Properties

        public DataType DataType { get; set; }
        public int MaxLength { get; set; }

        #endregion

        #region Constructors

        public DataTypeInfo()
        {
            DataType = DataType.None;
            MaxLength = 0;
        }

        #endregion
    }

    public static class AllDataTypeInfos
    {
        public static List<DataTypeInfo> DataTypeInfoList { get; set; }

        #region Constructors

        static AllDataTypeInfos()
        {
            DataTypeInfoList = new List<DataTypeInfo>();

            DataTypeInfoList.Add(new DataTypeInfo()
            {
                DataType = DataType.Text,
                MaxLength = 256
            });

            DataTypeInfoList.Add(new DataTypeInfo()
            {
                DataType = DataType.Number,
                MaxLength = 24
            });
        }

        #endregion

        #region Methods

        public static DataTypeInfo Find(DataType dataType)
        {
            return DataTypeInfoList.FirstOrDefault(obj => obj.DataType == dataType);
        }

        #endregion
    }
}
