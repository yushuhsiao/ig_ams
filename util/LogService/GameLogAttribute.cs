using ams;
using ams.Data;
using GeniusBull;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace LogService
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    class GameLogAttribute : Attribute
    {
        public string FieldName { get; set; }
        public bool ApplyExchangeRate { get; set; }
        public GameLogAttribute(string fieldName) { this.FieldName = fieldName; }

        public PropertyInfo p_src;
        public FieldInfo f_src;

        public PropertyInfo p_dst;
        public FieldInfo f_dst;

        static Dictionary<Type, List<GameLogAttribute>> dict = new Dictionary<Type, List<GameLogAttribute>>();
        public static void MapValue(SqlBuilder sql, object obj, decimal exchange_rate)
        {
            if (obj == null) return;
            List<GameLogAttribute> list;
            lock (dict)
            {
                if (!dict.TryGetValue(obj.GetType(), out list))
                {
                    dict[obj.GetType()] = list = new List<GameLogAttribute>();
                    for (Type t = obj.GetType(); t != null; t = t.BaseType)
                    {
                        foreach (MemberInfo m in t.GetMembers(_TypeExtensions.BindingFlags4))
                        {
                            GameLogAttribute a = m.GetCustomAttribute<GameLogAttribute>();
                            if (a == null) continue;
                            a.p_src = m as PropertyInfo;
                            a.f_src = m as FieldInfo;
                            a.p_dst = typeof(GameLog).GetProperty(a.FieldName, _TypeExtensions.BindingFlags0);
                            a.f_dst = typeof(GameLog).GetField(a.FieldName, _TypeExtensions.BindingFlags0);
                            list.Add(a);
                        }
                    }
                }
            }
            foreach (GameLogAttribute a in list)
            {
                object value;
                if (a.p_src != null) value = a.p_src.GetValue(obj, null);
                else if (a.f_src != null) value = a.f_src.GetValue(obj);
                else continue;
                string name = a.p_dst?.Name ?? a.f_dst?.Name;
                if (name == null) continue;
                if (a.ApplyExchangeRate && exchange_rate != 1 && (value is decimal))
                    value = ((decimal)value) * exchange_rate;
                if (value is UserName)
                    sql["n", name] = value;
                else
                    sql[" ", name] = value;
            }
        }

        public static bool CreateGameLog(_Config.Item config, GeniusBull._LogBase item, GeniusBull._LogBase group, IG01PlatformInfo platform, GameInfo gameInfo, out MemberData member, out string sql_string)
        {
            AgentData agent;
            if (item.GetMember(config, out member, out agent))
            {
                decimal cx = Currency.QueryExchangeRate(member.CorpInfo.Currency, platform.Currency);
                //if (member.CorpInfo.ID == 2)
                //    Debugger.Break();
                SqlBuilder sql = new SqlBuilder();
                sql[" ", nameof(GameLog.UserID), "          "] = member.ID;
                sql["n", nameof(GameLog.UserName), "        "] = member.UserName;
                sql[" ", nameof(GameLog.Depth), "           "] = member.Depth;
                sql[" ", nameof(GameLog.CurrencyA), "       "] = member.CorpInfo.Currency;
                sql[" ", nameof(GameLog.CurrencyB), "       "] = platform.Currency;
                sql[" ", nameof(GameLog.CurrencyX), "       "] = cx;
                sql[" ", nameof(GameLog.CorpID), "          "] = member.CorpInfo.ID;
                sql["n", nameof(GameLog.CorpName), "        "] = member.CorpInfo.UserName;
                sql[" ", nameof(GameLog.ParentID), "        "] = agent.ID;
                sql["n", nameof(GameLog.ParentName), "      "] = agent.UserName;
                sql[" ", nameof(GameLog.PlatformID), "      "] = platform.ID;
                sql["n", nameof(GameLog.PlatformName), "    "] = platform.PlatformName;
                sql[" ", nameof(GameLog.PlatformType), "    "] = platform.PlatformType;
                sql[" ", nameof(GameLog.GameClass), "       "] = gameInfo.GameClass;
                sql[" ", nameof(GameLog.GameID), "          "] = gameInfo.ID;
                sql["n", nameof(GameLog.GameName), "        "] = gameInfo.Name;
                GameLogAttribute.MapValue(sql, item, 1 / cx);
                GameLogAttribute.MapValue(sql, group, 1 / cx);
                sql_string = sql._insert(TableName<GameLog>.Value);
                return true;
            }
            else return _null.noop(false, out sql_string);
        }
    }
}
