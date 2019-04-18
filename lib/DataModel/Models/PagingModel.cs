using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace InnateGlory.Models
{
    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //public abstract class PagingModel
    //{
    //}

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PagingModel<T> //: PagingModel
    {
        [JsonProperty]
        public int PageIndex
        {
            get
            {
                if (_PageIndex < 0)
                    return 0;
                return _PageIndex;
            }
            set => _PageIndex = value;
        } private int _PageIndex;

        [JsonProperty]
        public int PageSize
        {
            get
            {
                if (_PageSize <= 0)
                    return PageSize_Max;
                if (_PageSize > PageSize_Max)
                    return PageSize_Max;
                return _PageSize;
            }
            set => _PageSize = value;
        } private int _PageSize;

        [JsonProperty]
        public string SortKey
        {
            get => _SortKey ?? TableName<T>._.SortKey;
            set => _SortKey = value;
        } private string _SortKey;

        public int Offset => this.PageSize * this.PageIndex;

        public string ToSql(string orderBy = null) => $"order by {SqlCmd.magic_quote(orderBy ?? this.SortKey)} offset {this.Offset} rows fetch next {this.PageSize} rows only";

        public static PagingModel<T> Instance = new PagingModel<T>();

        public const int PageSize_Max = 1000;

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