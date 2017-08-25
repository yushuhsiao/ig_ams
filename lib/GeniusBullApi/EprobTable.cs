using ams;
using ams.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace GeniusBull
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn), ams.TableName("EprobTable")]
    public class EprobTable
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Group
        {
            [DbImport, JsonProperty]
            public virtual int GameId { get; set; }
            [DbImport, JsonProperty]
            public virtual int Eprob { get; set; }
            [DbImport, JsonProperty]
            public virtual bool Selected { get; set; }
        }

        [DbImport]
        public long Id;
        [DbImport]
        public int GameId;
        [DbImport]
        public int Eprob;
        [DbImport]
        public bool Selected;
        [DbImport]
        public int Symbol;
        [DbImport]
        public string SymbolName;
        [DbImport]
        public int Reel_1 { get { return Reel[0]; } set { Reel[0] = value; } }
        [DbImport]
        public int Reel_2 { get { return Reel[1]; } set { Reel[1] = value; } }
        [DbImport]
        public int Reel_3 { get { return Reel[2]; } set { Reel[2] = value; } }
        [DbImport]
        public int Reel_4 { get { return Reel[3]; } set { Reel[3] = value; } }
        [DbImport]
        public int Reel_5 { get { return Reel[4]; } set { Reel[4] = value; } }

        [JsonProperty]
        public int[] Reel = new int[5];

        [JsonProperty]
        public Dictionary<string, decimal> PayRate = new Dictionary<string, decimal>();
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn), ams.TableName("EprobSymbol")]
    public class EprobSymbol
    {
        public static RedisVer<List<EprobSymbol>>.Dict Cache = new RedisVer<List<EprobSymbol>>.Dict("EprobSymbol")
        {
            ReadData = (sqlcmd, index) =>
            {
                IG01PlatformInfo p = PlatformInfo.GetPlatformInfo(index) as IG01PlatformInfo;
                if (p == null) return null;
                return p.GameDB().ToList<EprobSymbol>($"select * from {ams.TableName<EprobSymbol>.Value} nolock");
            }
        };

        [DbImport, JsonProperty]
        public int Symbol;
        [DbImport, JsonProperty]
        public string Name;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn), ams.TableName("GeniusBull_EprobTableLimit")]
    public class EprobTableLimit
    {
        public static RedisVer<List<EprobTableLimit>>.Dict Cache = new RedisVer<List<EprobTableLimit>>.Dict("EprobTableLimit")
        {
            ReadData = (sqlcmd, index) =>
            {
                var ret = sqlcmd.ToList<EprobTableLimit>($"select * from {ams.TableName<EprobTableLimit>.Value} nolock");
                foreach (var n in ret)
                    n.SymbolCode = (n._Symbol = EprobSymbol.Cache[index].Value.Find((n2) => n.Symbol == n2.Name))?.Symbol ?? 0;
                return ret;
            }
        };

        EprobSymbol _Symbol;
        public int[] Max = new int[5];
        public int[] Min = new int[5];

        [DbImport("X2")]
        public decimal X2_Ratio;
        [DbImport("X3")]
        public decimal X3_Ratio;
        [DbImport("X4")]
        public decimal X4_Ratio;
        [DbImport("X5")]
        public decimal X5_Ratio;

        [DbImport]
        public int GameId;
        public int SymbolCode;
        [DbImport]
        public string Symbol;
        [DbImport]
        public int Max1 { get { return Max[0]; } set { Max[0] = value; } }
        [DbImport]
        public int Max2 { get { return Max[1]; } set { Max[1] = value; } }
        [DbImport]
        public int Max3 { get { return Max[2]; } set { Max[2] = value; } }
        [DbImport]
        public int Max4 { get { return Max[3]; } set { Max[3] = value; } }
        [DbImport]
        public int Max5 { get { return Max[4]; } set { Max[4] = value; } }
        [DbImport]
        public int Min1 { get { return Min[0]; } set { Min[0] = value; } }
        [DbImport]
        public int Min2 { get { return Min[1]; } set { Min[1] = value; } }
        [DbImport]
        public int Min3 { get { return Min[2]; } set { Min[2] = value; } }
        [DbImport]
        public int Min4 { get { return Min[3]; } set { Min[3] = value; } }
        [DbImport]
        public int Min5 { get { return Min[4]; } set { Min[4] = value; } }
    }
}