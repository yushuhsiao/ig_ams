using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class PagingModel
    {
        public const int PageSize_Min = 1;
        public const int PageSize_Max = 1000;

        [JsonProperty]
        public int PageIndex
        {
            get
            {
                if (_PageIndex > 0)
                    return _PageIndex;
                else
                    return 0;
            }
            set
            {
                if (value > 0)
                    _PageIndex = value;
                else
                    _PageIndex = 0;
            }
        }
        private int _PageIndex;

        [JsonProperty]
        public int PageSize
        {
            get
            {
                if (_PageSize < PageSize_Min)
                    return PageSize_Min;
                else if (_PageSize > PageSize_Max)
                    return PageSize_Max;
                else
                    return _PageSize;
            }
            set
            {
                if (value < PageSize_Min)
                    value = PageSize_Min;
                else if (value > PageSize_Max)
                    value = PageSize_Max;
                else
                    _PageSize = value;
            }
        }
        private int _PageSize;

        [JsonProperty]
        public virtual string SortKey { get; set; }

        public int Offset => this.PageSize * this.PageIndex;

        public string ToSql(string orderBy = null) => $"order by {SqlCmd.magic_quote(orderBy ?? this.SortKey)} offset {this.Offset} rows fetch next {this.PageSize} rows only";
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PagingModel<T> : PagingModel
    {
        [JsonProperty]
        public override string SortKey
        {
            get => base.SortKey ?? TableName<T>._.SortKey;
            set => base.SortKey = value;
        }

        //public static PagingModel<T> Instance = new PagingModel<T>();


        //public static PagingModel<T> operator +(PagingModel<T> src, int max)
        //{
        //    if (src == null)
        //        return PagingModel<T>.Instance;
        //    //if (max == 0) max = PageSize_Max;
        //    //src.PageSize = src.PageSize.Max(PageSize_Max);
        //    //src.Offset = src.PageSize * src.PageIndex;
        //    return src;
        //}
        //public static PagingModel<T> operator ++(PagingModel<T> src) => src + 0;
    }

    //partial class DataModel_Extensions
    //{
    //    public const int PageSize_Max = 1000;
    //    public static PagingModel<T> Normalize<T>(this PagingModel<T> src)
    //    {
    //        if (src != null)
    //        {
    //            src.PageSize = src.PageSize.Max(PageSize_Max);
    //            src.Offset = src.PageSize * src.PageIndex;
    //        }
    //        else
    //            return PagingModel<T>.Instance;
    //        return src;
    //    }
    //}
}