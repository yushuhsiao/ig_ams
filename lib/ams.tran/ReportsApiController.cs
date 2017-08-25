using ams.Data;
using System;
using System.Data;
using System.Web.Http;

namespace ams.Controllers
{
    public partial class ReportsApiController : _ApiController
    {
        [HttpPost, Route("~/reports/tranlog/list")]
        public ListResponse<TranLog> GetTranLog(ListRequest<TranLog> args)
        {
            return this.Null(args).Validate(this, true).GetResponse(
                get_sqlcmd: () => args.CorpInfo.DB_Log01R(),
                onBuild: (string name, FilterableAttribute item) =>
            {
                switch (name)
                {
                    case nameof(TranLog.UserName):
                    case nameof(TranLog.ParentName): /*     */ args.AddFilter_StringContains(name, item); break;
                    case nameof(TranLog.PlatformName): /*   */ args.AddFilter_PlatformNames(name, item); break;
                    case nameof(TranLog.LogType): /*        */ args.AddFilter_Enum<LogType>(name, item); break;
                }
            });
        }
        //public ListResponse<TranLog> TranLog(ListRequest_2<TranLog> args)
        //{
        //    ListRequest<TranLog>.Valid(ModelState, args);
        //    args.TimeFilter("RequestTime");
        //    var ret = args.GetResponse(args.CorpInfo.DB_Log01R(), onBuild: (SqlBuilder sql, string name, Type valueType, jqx_filter_list value1, string[] value2, object value) =>
        //    {
        //        if (name == null) { }
        //        else if (name == "UserName")
        //        {
        //            if (value is string) sql["w", name] = value;
        //            else if (value1 != null) sql["w", name, null, " like "] = $"%{ value1[0].GetValue().Replace("%", "")}%";
        //            else if (value2 != null) { }
        //        }
        //        else if (name == "PlatformID")
        //            args.AddPlatforms(sql, name, valueType, value1, value2);
        //    });
        //    return args.GetResponse(args.CorpInfo.DB_Log01R(), create: () => new TranLog());
        //}

        [HttpPost, Route("~/reports/gamelog/list")]
        public ListResponse<GameLog> GetGameLog(ListRequest<GameLog> args)
        {
            var ret = this.Null(args).Validate(this, true).GetResponse(
                get_sqlcmd: () => args.CorpInfo.DB_Log01R(),
                onBuild: (string name, FilterableAttribute item) =>
                {
                    switch (name)
                    {
                        case nameof(GameLog.UserName):
                        case nameof(GameLog.SerialNumber):
                        case nameof(GameLog.ParentName): /*     */ args.AddFilter_StringContains(name, item); break;
                        case nameof(GameLog.PlayStartTime):
                        case nameof(GameLog.PlayEndTime):
                        case nameof(GameLog.CreateTime): /*     */ args.AddFilter_TimeRange(name, item); break;
                        case nameof(GameLog.PlatformID):
                        case nameof(GameLog.PlatformName): /*   */ args.AddFilter_PlatformNames("PlatformID", item); break;
                        case nameof(GameLog.PlatformType): /*   */ args.AddFilter_Enum<PlatformType>(name, item); break;
                        case nameof(GameLog.GameClass): /*      */ args.AddFilter_Enum<GameClass>(name, item); break;
                        case nameof(GameLog.GameID):
                        case nameof(GameLog.GameName): /*       */ args.AddFilter_GameIDs("GameID", item); break;
                    }
                });
            foreach (var row in ret.Rows)
            {
                row.Platform = PlatformInfo.GetPlatformInfo(row.PlatformID, false, false);
                row.GameInfo = GameInfo.GetGameInfo(row.GameClass, row.GameID);
            }
            return ret;
        }
        //public ListResponse<GameLog> GameLog(ListRequest_2<GameLog> args)
        //{
        //    ListRequest<GameLog>.Valid(ModelState, args);
        //    args.TimeFilter("PlayEndTime");
        //    var ret = args.GetResponse(args.CorpInfo.DB_Log01R(), onBuild: (SqlBuilder sql, string name, Type valueType, jqx_filter_list value1, string[] value2, object value) =>
        //    {
        //        switch (name)
        //        {
        //            case "UserName":
        //                if (value is string) sql["w", name] = value;
        //                else if (value1 != null) sql["w", name, null, " like "] = $"%{ value1[0].GetValue().Replace("%", "")}%";
        //                else if (value2 != null) { }
        //                break;
        //            case nameof(ams.Data.GameLog.PlayStartTime):
        //            case nameof(ams.Data.GameLog.PlayEndTime):
        //            case nameof(ams.Data.GameLog.CreateTime): args.AddTimeRange(sql, name, valueType, value1, value2); break;
        //            case nameof(ams.Data.GameLog.PlatformID): args.AddPlatforms(sql, name, valueType, value1, value2); break;
        //            case nameof(ams.Data.GameLog.GameID):
        //            case nameof(ams.Data.GameLog.GameName): args.AddGameIDs(sql, "GameID", valueType, value1, value2); break;
        //        }
        //    });
        //    foreach (var row in ret.Rows)
        //    {
        //        row.Platform = PlatformInfo.GetPlatformInfo(row.PlatformID, false, false);
        //        row.GameInfo = GameInfo.GetGameInfo(row.GameClass, row.GameID);
        //    }
        //    return ret;
        //}
    }
}
