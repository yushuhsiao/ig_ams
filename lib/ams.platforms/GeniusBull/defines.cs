using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeniusBull
{
    public enum MemberRole : byte
    {
        Root = 255,
        Admin = 254,
        Vendor = 0,
        Webmaster = 1,
        Agent = 2, // *
        Subagent = 3,
        Player = 4, // *
        Robot = 5
    }

    public enum MemberStatus : byte
    {
        Delete = 0,
        Active = 1,
        Blocked = 2,
        Disable = 3
    }

    public enum MemberType : byte
    {
        Cash = 0,
        Credit = 1,
        Both = 2
    }

    public enum EntryLobby { LiveCasino, TabletopGames, VideoArcade }

    public enum MahjongWindPosition : int
    {
        東風東 = 00, 東風南 = 01, 東風西 = 02, 東風北 = 03,
        南風東 = 04, 南風南 = 05, 南風西 = 06, 南風北 = 07,
        西風東 = 08, 西風南 = 09, 西風西 = 10, 西風北 = 11,
        北風東 = 12, 北風南 = 13, 北風西 = 14, 北風北 = 15,
    }
}