using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameControl.Models
{
    public class PCDetail
    {
        public int Enable { get; set; }
        public string Name { get; set; }
        public float Odds { get; set; }
        public float Water { get; set; }
        public float WaterBack { get; set; }
        public int BetMin { get; set; }
        public int BetMax { get; set; }
        public int SingelMax { get; set; }
        public int DishMax { get; set; }
    }

    public class PCControl
    {
        public int SID { get; set; }
        public string Dish { get; set; }
        public PCDetail BallSmall { get; set; }
        public PCDetail BallMid { get; set; }
        public PCDetail BallBig { get; set; }
        public PCDetail BallOdd { get; set; }
        public PCDetail BallSam { get; set; }
        public PCDetail BallEven { get; set; }
        public PCDetail SumBig { get; set; }
        public PCDetail SumSmall { get; set; }
        public PCDetail SumOdd { get; set; }
        public PCDetail SumEven { get; set; }
        public PCDetail SumBigOdd { get; set; }
        public PCDetail SumSmallOdd { get; set; }
        public PCDetail SumBigEven { get; set; }
        public PCDetail SumSmallEven { get; set; }
        public PCDetail Range_1 { get; set; }
        public PCDetail Range_2 { get; set; }
        public PCDetail Range_3 { get; set; }
        public PCDetail Range_4 { get; set; }
        public PCDetail Range_5 { get; set; }
        public int BallMaxMonney { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateUser { get; set; }
        public int CompanyID { get; set; }
    }

}