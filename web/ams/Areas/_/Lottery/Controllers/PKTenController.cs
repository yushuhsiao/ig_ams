using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;
using PKTen.Models;
using System.Configuration;

namespace ams.Areas.PKTen.Controllers
{
    [RouteArea(_url.areas.Lottery)]
    [Route("~/lottery/{action}")]
    public class PKTenController : _Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GameDB"].ToString());

        public ActionResult PKTenPcIndex()
        {
            string sql = "Select SID, Dish, UpdateTime, UpdateUser From Pc_PK10_Sample";
            SqlCmd PKIndex = _HttpContext.GetSqlCmd(DB.LTDB01R);
            //conn.Open();
            //SqlCommand PKIndex = new SqlCommand(sql, conn);
            using (SqlDataReader dr = PKIndex.ExecuteReader(sql))
            {
                List<PK10_PC> results = new List<PK10_PC>();
                while (dr.Read())
                {
                    PK10_PC Item = new PK10_PC();
                    Item.SID = dr.GetInt32(0);
                    Item.Dish = dr.GetString(1);
                    Item.UpdateTime = dr.GetDateTime(2);
                    Item.UpdateUser = dr.GetString(3);
                    results.Add(Item);
                }
                dr.Close();
                //conn.Close();
                return View(results);
            }               
        }

        public ActionResult PKTenPcWaterEdit(int SID)
        {
            string sqlWater = "select SID, Dish, Sum3, Sum4, Sum5, Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15, "
                 + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4, "
                 + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                 + " from Pc_PK10_Sample where SID = " + SID;

            //conn.Open();
            //SqlCommand scmd = new SqlCommand(sqlWater, conn);
            SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = scmd.ExecuteReader(sqlWater);
            PK10_PC PcCon = new PK10_PC();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    PcCon = (new PK10_PC
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        Sum3 = Sum3,
                        Sum4 = Sum4,
                        Sum5 = Sum5,
                        Sum6 = Sum6,
                        Sum7 = Sum7,
                        Sum8 = Sum8,
                        Sum9 = Sum9,
                        Sum10 = Sum10,
                        Sum11 = Sum11,
                        Sum12 = Sum12,
                        Sum13 = Sum13,
                        Sum14 = Sum14,
                        Sum15 = Sum15,
                        Sum16 = Sum16,
                        Sum17 = Sum17,
                        Sum18 = Sum18,
                        Sum19 = Sum19,
                        SumSmall = SumSmall,
                        SumBig = SumBig,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        ChampionBuild = ChampionBuild,
                        Car1 = Car1,
                        Car2 = Car2,
                        Car3 = Car3,
                        Car4 = Car4,
                        Car5 = Car5,
                        Car6 = Car6,
                        Car7 = Car7,
                        Car8 = Car8,
                        Car9 = Car9,
                        Car10 = Car10,
                        CarSmall = CarSmall,
                        CarBig = CarBig,
                        CarOdd = CarOdd,
                        CarEven = CarEven,
                        CarDra = CarDra,
                        CarTig = CarTig
                    });
                    return View(PcCon);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult PKTenPcWaterEdit(PK10_PC postback)
        {
            string SqlEdit = "update Pc_PK10_Sample SET Dish = @Dish,"
                        + " Sum3 = @Sum3, Sum4 = @Sum4, Sum5 = @Sum5, Sum6 = @Sum6, Sum7 = @Sum7, "
                        + " Sum8 = @Sum8, Sum9 = @Sum9, Sum10= @Sum10, Sum11 = @Sum11, Sum12 = @Sum12, "
                        + " Sum13 = @Sum13, Sum14 = @Sum14, Sum15 = @Sum15, Sum16 = @Sum16, Sum17 = @Sum17, "
                        + " Sum18 = @Sum18, Sum19 = @Sum19, SumSmall = @SumSmall, SumBig = @SumBig, "
                        + " SumOdd = @SumOdd, SumEven = @SumEven, ChampionBuild = @ChampionBuild, "
                        + " Car1 = @Car1, Car2 = @Car2, Car3 = @Car3, Car4 = @Car5, Car5 = @Car5, Car6 = @Car6, "
                        + " Car7 = @Car7, Car8 = @Car8, Car9 = @Car9, Car10 = @Car10, CarSmall = @CarSmall, "
                        + " CarBig = @CarBig, CarOdd = @CarOdd, CarEven = @CarEven, CarDra = @CarDra, "
                        + " CarTig = @CarTig ,UpdateUser = @UpdateUser, UpdateTime = @UpdateTime, "
                        + " CompanyID = @CompanyID WHERE SID = @SID";
            string sum3 = "", sum4 = "", sum5 = "", sum6 = "", sum7 = "", sum8 = "", sum9 = "", sum10 = "", sum11 = "", sum12 = "";
            string sum13 = "", sum14 = "", sum15 = "", sum16 = "", sum17 = "", sum18 = "", sum19 = "", sumsmall = "", sumbig = "", sumodd = "", sumeven = "", championbuild = "";
            string car1 = "", car2 = "", car3 = "", car4 = "", car5 = "", car6 = "", car7 = "", car8 = "", car9 = "", car10 = "";
            string carsmall = "", carbig = "", carodd = "", careven = "", cardra = "", cartig = "";
            string Dish = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "select SID, Dish, Sum3, Sum4, Sum5 , Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15,"
                + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4,"
                + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                + " from Pc_PK10_Sample where SID =" + postback.SID;
                //conn.Open();
                //SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlDataReader dr = scmd.ExecuteReader(SqlSelect);
                PK10_PC PcCon = new PK10_PC();
                //Json
                while (dr.Read())
                {
                    Dish = dr[1].ToString();
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    //塞入資料
                    //總和
                    Sum3.Water = postback.Sum3.Water;
                    Sum3.WaterBack = postback.Sum3.WaterBack;
                    Sum4.Water = postback.Sum4.Water;
                    Sum4.WaterBack = postback.Sum4.WaterBack;
                    Sum5.Water = postback.Sum5.Water;
                    Sum5.WaterBack = postback.Sum5.WaterBack;
                    Sum6.Water = postback.Sum6.Water;
                    Sum6.WaterBack = postback.Sum6.WaterBack;
                    Sum7.Water = postback.Sum7.Water;
                    Sum7.WaterBack = postback.Sum7.WaterBack;
                    Sum8.Water = postback.Sum8.Water;
                    Sum8.WaterBack = postback.Sum8.WaterBack;
                    Sum9.Water = postback.Sum9.Water;
                    Sum9.WaterBack = postback.Sum9.WaterBack;
                    Sum10.Water = postback.Sum10.Water;
                    Sum10.WaterBack = postback.Sum10.WaterBack;
                    Sum11.Water = postback.Sum11.Water;
                    Sum11.WaterBack = postback.Sum11.WaterBack;
                    Sum12.Water = postback.Sum12.Water;
                    Sum12.WaterBack = postback.Sum12.WaterBack;
                    Sum13.Water = postback.Sum13.Water;
                    Sum13.WaterBack = postback.Sum13.WaterBack;
                    Sum14.Water = postback.Sum14.Water;
                    Sum14.WaterBack = postback.Sum14.WaterBack;
                    Sum15.Water = postback.Sum15.Water;
                    Sum15.WaterBack = postback.Sum15.WaterBack;
                    Sum16.Water = postback.Sum16.Water;
                    Sum16.WaterBack = postback.Sum16.WaterBack;
                    Sum17.Water = postback.Sum17.Water;
                    Sum17.WaterBack = postback.Sum17.WaterBack;
                    Sum18.Water = postback.Sum18.Water;
                    Sum18.WaterBack = postback.Sum18.WaterBack;
                    Sum19.Water = postback.Sum19.Water;
                    Sum19.WaterBack = postback.Sum19.WaterBack;
                    //總和大小單雙
                    SumSmall.Water = postback.SumSmall.Water;
                    SumSmall.WaterBack = postback.SumSmall.WaterBack;
                    SumBig.Water = postback.SumBig.Water;
                    SumBig.WaterBack = postback.SumBig.WaterBack;
                    SumOdd.Water = postback.SumOdd.Water;
                    SumOdd.WaterBack = postback.SumOdd.WaterBack;
                    SumEven.Water = postback.SumEven.Water;
                    SumEven.WaterBack = postback.SumEven.WaterBack;
                    //冠亞組合
                    ChampionBuild.Water = postback.ChampionBuild.Water;
                    ChampionBuild.WaterBack = postback.ChampionBuild.WaterBack;
                    //車                  
                    Car1.Water = postback.Car1.Water;
                    Car1.WaterBack = postback.Car1.WaterBack;
                    Car2.Water = postback.Car2.Water;
                    Car2.WaterBack = postback.Car2.WaterBack;
                    Car3.Water = postback.Car3.Water;
                    Car3.WaterBack = postback.Car3.WaterBack;
                    Car4.Water = postback.Car4.Water;
                    Car4.WaterBack = postback.Car4.WaterBack;
                    Car5.Water = postback.Car5.Water;
                    Car5.WaterBack = postback.Car5.WaterBack;
                    Car6.Water = postback.Car6.Water;
                    Car6.WaterBack = postback.Car6.WaterBack;
                    Car7.Water = postback.Car7.Water;
                    Car7.WaterBack = postback.Car7.WaterBack;
                    Car8.Water = postback.Car8.Water;
                    Car8.WaterBack = postback.Car8.WaterBack;
                    Car9.Water = postback.Car9.Water;
                    Car9.WaterBack = postback.Car9.WaterBack;
                    Car10.Water = postback.Car10.Water;
                    Car10.WaterBack = postback.Car10.WaterBack;

                    CarSmall.Water = postback.CarSmall.Water;
                    CarSmall.WaterBack = postback.CarSmall.WaterBack;
                    CarBig.Water = postback.CarBig.Water;
                    CarBig.WaterBack = postback.CarBig.WaterBack;
                    CarOdd.Water = postback.CarOdd.Water;
                    CarOdd.WaterBack = postback.CarOdd.WaterBack;
                    CarEven.Water = postback.CarEven.Water;
                    CarEven.WaterBack = postback.CarEven.WaterBack;
                    CarDra.Water = postback.CarDra.Water;
                    CarDra.WaterBack = postback.CarDra.WaterBack;
                    CarTig.Water = postback.CarTig.Water;
                    CarTig.WaterBack = postback.CarTig.WaterBack;

                    //組回Json
                    //組回Json
                    sum3 = JsonConvert.SerializeObject(Sum3);
                    sum4 = JsonConvert.SerializeObject(Sum4);
                    sum5 = JsonConvert.SerializeObject(Sum5);
                    sum6 = JsonConvert.SerializeObject(Sum6);
                    sum7 = JsonConvert.SerializeObject(Sum7);
                    sum8 = JsonConvert.SerializeObject(Sum8);
                    sum9 = JsonConvert.SerializeObject(Sum9);
                    sum10 = JsonConvert.SerializeObject(Sum10);
                    sum11 = JsonConvert.SerializeObject(Sum11);
                    sum12 = JsonConvert.SerializeObject(Sum12);
                    sum13 = JsonConvert.SerializeObject(Sum13);
                    sum14 = JsonConvert.SerializeObject(Sum14);
                    sum15 = JsonConvert.SerializeObject(Sum15);
                    sum16 = JsonConvert.SerializeObject(Sum16);
                    sum17 = JsonConvert.SerializeObject(Sum17);
                    sum18 = JsonConvert.SerializeObject(Sum18);
                    sum19 = JsonConvert.SerializeObject(Sum19);
                    sumsmall = JsonConvert.SerializeObject(SumSmall);
                    sumbig = JsonConvert.SerializeObject(SumBig);
                    sumodd = JsonConvert.SerializeObject(SumOdd);
                    sumeven = JsonConvert.SerializeObject(SumEven);
                    championbuild = JsonConvert.SerializeObject(ChampionBuild);
                    car1 = JsonConvert.SerializeObject(Car1);
                    car2 = JsonConvert.SerializeObject(Car2);
                    car3 = JsonConvert.SerializeObject(Car3);
                    car4 = JsonConvert.SerializeObject(Car4);
                    car5 = JsonConvert.SerializeObject(Car5);
                    car6 = JsonConvert.SerializeObject(Car6);
                    car7 = JsonConvert.SerializeObject(Car7);
                    car8 = JsonConvert.SerializeObject(Car8);
                    car9 = JsonConvert.SerializeObject(Car9);
                    car10 = JsonConvert.SerializeObject(Car10);
                    carsmall = JsonConvert.SerializeObject(CarSmall);
                    carbig = JsonConvert.SerializeObject(CarBig);
                    carodd = JsonConvert.SerializeObject(CarOdd);
                    careven = JsonConvert.SerializeObject(CarEven);
                    cardra = JsonConvert.SerializeObject(CarDra);
                    cartig = JsonConvert.SerializeObject(CarTig);
                }
                dr.Close();
                SqlCmd Update = _HttpContext.GetSqlCmd(DB.LTDB01R);
                
                //SqlCommand Update = new SqlCommand(SqlEdit, conn);
                Update.Parameters.AddWithValue("@Sum3", sum3);
                Update.Parameters.AddWithValue("@Sum4", sum4);
                Update.Parameters.AddWithValue("@Sum5", sum5);
                Update.Parameters.AddWithValue("@Sum6", sum6);
                Update.Parameters.AddWithValue("@Sum7", sum7);
                Update.Parameters.AddWithValue("@Sum8", sum8);
                Update.Parameters.AddWithValue("@Sum9", sum9);
                Update.Parameters.AddWithValue("@Sum10", sum10);
                Update.Parameters.AddWithValue("@Sum11", sum11);
                Update.Parameters.AddWithValue("@Sum12", sum12);
                Update.Parameters.AddWithValue("@Sum13", sum13);
                Update.Parameters.AddWithValue("@Sum14", sum14);
                Update.Parameters.AddWithValue("@Sum15", sum15);
                Update.Parameters.AddWithValue("@Sum16", sum16);
                Update.Parameters.AddWithValue("@Sum17", sum17);
                Update.Parameters.AddWithValue("@Sum18", sum18);
                Update.Parameters.AddWithValue("@Sum19", sum19);
                Update.Parameters.AddWithValue("@SumSmall", sumsmall);
                Update.Parameters.AddWithValue("@SumBig", sumbig);
                Update.Parameters.AddWithValue("@SumOdd", sumodd);
                Update.Parameters.AddWithValue("@SumEven", sumeven);
                Update.Parameters.AddWithValue("@ChampionBuild", championbuild);
                Update.Parameters.AddWithValue("@Car1", car1);
                Update.Parameters.AddWithValue("@Car2", car2);
                Update.Parameters.AddWithValue("@Car3", car3);
                Update.Parameters.AddWithValue("@Car4", car4);
                Update.Parameters.AddWithValue("@Car5", car5);
                Update.Parameters.AddWithValue("@Car6", car6);
                Update.Parameters.AddWithValue("@Car7", car7);
                Update.Parameters.AddWithValue("@Car8", car8);
                Update.Parameters.AddWithValue("@Car9", car9);
                Update.Parameters.AddWithValue("@Car10", car10);
                Update.Parameters.AddWithValue("@CarSmall", carsmall);
                Update.Parameters.AddWithValue("@CarBig", carbig);
                Update.Parameters.AddWithValue("@CarOdd", carodd);
                Update.Parameters.AddWithValue("@CarEven", careven);
                Update.Parameters.AddWithValue("@CarDra", cardra);
                Update.Parameters.AddWithValue("@CarTig", cartig);
                Update.Parameters.AddWithValue("@UpdateUser", "Danny");
                Update.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                Update.Parameters.AddWithValue("@CompanyID", 1);
                Update.Parameters.AddWithValue("@SID", postback.SID);
                Update.Parameters.AddWithValue("@Dish", Dish);
                Update.ExecuteNonQuery(SqlEdit);

                return RedirectToAction("PKTenIndex");
            }
            else
            {
                return View(postback);
            }
        }

        public ActionResult PKTenPCDetailEdit(int SID)
        {
            string sqlWater = "select SID, Dish, Sum3, Sum4, Sum5, Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15, "
                 + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4, "
                 + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                 + " from Pc_PK10_Sample where SID = " + SID;

            //conn.Open();
            //SqlCommand scmd = new SqlCommand(sqlWater, conn);
            SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = scmd.ExecuteReader();
            PK10_PC PcCon = new PK10_PC();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    PcCon = (new PK10_PC
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        Sum3 = Sum3,
                        Sum4 = Sum4,
                        Sum5 = Sum5,
                        Sum6 = Sum6,
                        Sum7 = Sum7,
                        Sum8 = Sum8,
                        Sum9 = Sum9,
                        Sum10 = Sum10,
                        Sum11 = Sum11,
                        Sum12 = Sum12,
                        Sum13 = Sum13,
                        Sum14 = Sum14,
                        Sum15 = Sum15,
                        Sum16 = Sum16,
                        Sum17 = Sum17,
                        Sum18 = Sum18,
                        Sum19 = Sum19,
                        SumSmall = SumSmall,
                        SumBig = SumBig,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        ChampionBuild = ChampionBuild,
                        Car1 = Car1,
                        Car2 = Car2,
                        Car3 = Car3,
                        Car4 = Car4,
                        Car5 = Car5,
                        Car6 = Car6,
                        Car7 = Car7,
                        Car8 = Car8,
                        Car9 = Car9,
                        Car10 = Car10,
                        CarSmall = CarSmall,
                        CarBig = CarBig,
                        CarOdd = CarOdd,
                        CarEven = CarEven,
                        CarDra = CarDra,
                        CarTig = CarTig
                    });
                    return View(PcCon);
                }
            }
            //conn.Close();
            return View();
        }
        [HttpPost]
        public ActionResult PKTenPCDetailEdit(PK10_PC postback)
        {
            string SqlEdit = "update Pc_PK10_Sample SET Dish = @Dish,"
                        + " Sum3 = @Sum3, Sum4 = @Sum4, Sum5 = @Sum5, Sum6 = @Sum6, Sum7 = @Sum7, "
                        + " Sum8 = @Sum8, Sum9 = @Sum9, Sum10= @Sum10, Sum11 = @Sum11, Sum12 = @Sum12, "
                        + " Sum13 = @Sum13, Sum14 = @Sum14, Sum15 = @Sum15, Sum16 = @Sum16, Sum17 = @Sum17, "
                        + " Sum18 = @Sum18, Sum19 = @Sum19, SumSmall = @SumSmall, SumBig = @SumBig, "
                        + " SumOdd = @SumOdd, SumEven = @SumEven, ChampionBuild = @ChampionBuild, "
                        + " Car1 = @Car1, Car2 = @Car2, Car3 = @Car3, Car4 = @Car5, Car5 = @Car5, Car6 = @Car6, "
                        + " Car7 = @Car7, Car8 = @Car8, Car9 = @Car9, Car10 = @Car10, CarSmall = @CarSmall, "
                        + " CarBig = @CarBig, CarOdd = @CarOdd, CarEven = @CarEven, CarDra = @CarDra, "
                        + " CarTig = @CarTig ,UpdateUser = @UpdateUser, UpdateTime = @UpdateTime, "
                        + " CompanyID = @CompanyID WHERE SID = @SID";
            string sum3 = "", sum4 = "", sum5 = "", sum6 = "", sum7 = "", sum8 = "", sum9 = "", sum10 = "", sum11 = "", sum12 = "";
            string sum13 = "", sum14 = "", sum15 = "", sum16 = "", sum17 = "", sum18 = "", sum19 = "", sumsmall = "", sumbig = "", sumodd = "", sumeven = "", championbuild = "";
            string car1 = "", car2 = "", car3 = "", car4 = "", car5 = "", car6 = "", car7 = "", car8 = "", car9 = "", car10 = "";
            string carsmall = "", carbig = "", carodd = "", careven = "", cardra = "", cartig = "";
            string Dish = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "select SID, Dish, Sum3, Sum4, Sum5 , Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15,"
                + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4,"
                + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                + "from Pc_PK10_Sample where SID =" + postback.SID;
                conn.Open();
                //SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlDataReader dr = scmd.ExecuteReader(SqlSelect);
                
                //Json
                while (dr.Read())
                {
                    Dish = dr[1].ToString();
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    //塞入資料
                    //總和
                    Sum3.BetMin = postback.Sum3.BetMin;
                    Sum3.BetMax = postback.Sum3.BetMax;
                    Sum3.SingelMax = postback.Sum3.SingelMax;
                    Sum3.DishMax = postback.Sum3.DishMax;

                    Sum4.BetMin = postback.Sum4.BetMin;
                    Sum4.BetMax = postback.Sum4.BetMax;
                    Sum4.SingelMax = postback.Sum4.SingelMax;
                    Sum4.DishMax = postback.Sum3.DishMax;

                    Sum5.BetMin = postback.Sum5.BetMin;
                    Sum5.BetMax = postback.Sum5.BetMax;
                    Sum5.SingelMax = postback.Sum5.SingelMax;
                    Sum5.DishMax = postback.Sum3.DishMax;

                    Sum6.BetMin = postback.Sum6.BetMin;
                    Sum6.BetMax = postback.Sum6.BetMax;
                    Sum6.SingelMax = postback.Sum6.SingelMax;
                    Sum6.DishMax = postback.Sum3.DishMax;

                    Sum7.BetMin = postback.Sum7.BetMin;
                    Sum7.BetMax = postback.Sum7.BetMax;
                    Sum7.SingelMax = postback.Sum7.SingelMax;
                    Sum7.DishMax = postback.Sum3.DishMax;

                    Sum8.BetMin = postback.Sum8.BetMin;
                    Sum8.BetMax = postback.Sum8.BetMax;
                    Sum8.SingelMax = postback.Sum8.SingelMax;
                    Sum8.DishMax = postback.Sum3.DishMax;

                    Sum9.BetMin = postback.Sum9.BetMin;
                    Sum9.BetMax = postback.Sum9.BetMax;
                    Sum9.SingelMax = postback.Sum9.SingelMax;
                    Sum9.DishMax = postback.Sum3.DishMax;

                    Sum10.BetMin = postback.Sum10.BetMin;
                    Sum10.BetMax = postback.Sum10.BetMax;
                    Sum10.SingelMax = postback.Sum10.SingelMax;
                    Sum10.DishMax = postback.Sum3.DishMax;

                    Sum11.BetMin = postback.Sum11.BetMin;
                    Sum11.BetMax = postback.Sum11.BetMax;
                    Sum11.SingelMax = postback.Sum11.SingelMax;
                    Sum11.DishMax = postback.Sum3.DishMax;

                    Sum12.BetMin = postback.Sum12.BetMin;
                    Sum12.BetMax = postback.Sum12.BetMax;
                    Sum12.SingelMax = postback.Sum12.SingelMax;
                    Sum12.DishMax = postback.Sum3.DishMax;

                    Sum13.BetMin = postback.Sum13.BetMin;
                    Sum13.BetMax = postback.Sum13.BetMax;
                    Sum13.SingelMax = postback.Sum13.SingelMax;
                    Sum13.DishMax = postback.Sum3.DishMax;

                    Sum14.BetMin = postback.Sum14.BetMin;
                    Sum14.BetMax = postback.Sum14.BetMax;
                    Sum14.SingelMax = postback.Sum14.SingelMax;
                    Sum14.DishMax = postback.Sum3.DishMax;

                    Sum15.BetMin = postback.Sum15.BetMin;
                    Sum15.BetMax = postback.Sum15.BetMax;
                    Sum15.SingelMax = postback.Sum15.SingelMax;
                    Sum15.DishMax = postback.Sum3.DishMax;

                    Sum16.BetMin = postback.Sum16.BetMin;
                    Sum16.BetMax = postback.Sum16.BetMax;
                    Sum16.SingelMax = postback.Sum16.SingelMax;
                    Sum16.DishMax = postback.Sum3.DishMax;

                    Sum17.BetMin = postback.Sum17.BetMin;
                    Sum17.BetMax = postback.Sum17.BetMax;
                    Sum17.SingelMax = postback.Sum17.SingelMax;
                    Sum17.DishMax = postback.Sum3.DishMax;

                    Sum18.BetMin = postback.Sum18.BetMin;
                    Sum18.BetMax = postback.Sum18.BetMax;
                    Sum18.SingelMax = postback.Sum18.SingelMax;
                    Sum18.DishMax = postback.Sum3.DishMax;

                    Sum19.BetMin = postback.Sum19.BetMin;
                    Sum19.BetMax = postback.Sum19.BetMax;
                    Sum19.SingelMax = postback.Sum19.SingelMax;
                    Sum19.DishMax = postback.Sum3.DishMax;
                    //總和大小單雙
                    SumSmall.BetMin = postback.SumSmall.BetMin;
                    SumSmall.BetMax = postback.SumSmall.BetMax;
                    SumSmall.SingelMax = postback.SumSmall.SingelMax;
                    SumSmall.DishMax = postback.SumSmall.DishMax;

                    SumBig.BetMin = postback.SumBig.BetMin;
                    SumBig.BetMax = postback.SumBig.BetMax;
                    SumBig.SingelMax = postback.SumBig.SingelMax;
                    SumBig.DishMax = postback.SumSmall.DishMax;

                    SumOdd.BetMin = postback.SumOdd.BetMin;
                    SumOdd.BetMax = postback.SumOdd.BetMax;
                    SumOdd.SingelMax = postback.SumOdd.SingelMax;
                    SumOdd.DishMax = postback.SumSmall.DishMax;

                    SumEven.BetMin = postback.SumEven.BetMin;
                    SumEven.BetMax = postback.SumEven.BetMax;
                    SumEven.SingelMax = postback.SumEven.SingelMax;
                    SumEven.DishMax = postback.SumSmall.DishMax;
                    //冠亞組合
                    ChampionBuild.BetMin = postback.ChampionBuild.BetMin;
                    ChampionBuild.BetMax = postback.ChampionBuild.BetMax;
                    ChampionBuild.SingelMax = postback.ChampionBuild.SingelMax;
                    ChampionBuild.DishMax = postback.ChampionBuild.DishMax;
                    //車                  
                    Car1.BetMin = postback.Car1.BetMin;
                    Car1.BetMax = postback.Car1.BetMax;
                    Car2.BetMin = postback.Car2.BetMin;
                    Car2.BetMax = postback.Car2.BetMax;
                    Car3.BetMin = postback.Car3.BetMin;
                    Car3.BetMax = postback.Car3.BetMax;
                    Car4.BetMin = postback.Car4.BetMin;
                    Car4.BetMax = postback.Car4.BetMax;
                    Car5.BetMin = postback.Car5.BetMin;
                    Car5.BetMax = postback.Car5.BetMax;
                    Car6.BetMin = postback.Car6.BetMin;
                    Car6.BetMax = postback.Car6.BetMax;
                    Car7.BetMin = postback.Car7.BetMin;
                    Car7.BetMax = postback.Car7.BetMax;
                    Car8.BetMin = postback.Car8.BetMin;
                    Car8.BetMax = postback.Car8.BetMax;
                    Car9.BetMin = postback.Car9.BetMin;
                    Car9.BetMax = postback.Car9.BetMax;
                    Car10.BetMin = postback.Car10.BetMin;
                    Car10.BetMax = postback.Car10.BetMax;
                    //CarSingleMax
                    Car1.SingelMax = postback.Car1.SingelMax;
                    Car2.SingelMax = postback.Car2.SingelMax;
                    Car3.SingelMax = postback.Car3.SingelMax;
                    Car4.SingelMax = postback.Car4.SingelMax;
                    Car5.SingelMax = postback.Car5.SingelMax;
                    Car6.SingelMax = postback.Car6.SingelMax;
                    Car7.SingelMax = postback.Car7.SingelMax;
                    Car8.SingelMax = postback.Car8.SingelMax;
                    Car9.SingelMax = postback.Car9.SingelMax;
                    Car10.SingelMax = postback.Car10.SingelMax;
                    //CarDishMax
                    Car1.DishMax = postback.Car1.DishMax;
                    Car2.DishMax = postback.Car1.DishMax;
                    Car3.DishMax = postback.Car1.DishMax;
                    Car4.DishMax = postback.Car1.DishMax;
                    Car5.DishMax = postback.Car1.DishMax;
                    Car6.DishMax = postback.Car1.DishMax;
                    Car7.DishMax = postback.Car1.DishMax;
                    Car8.DishMax = postback.Car1.DishMax;
                    Car9.DishMax = postback.Car1.DishMax;
                    Car10.DishMax = postback.Car1.DishMax;

                    CarSmall.BetMin = postback.CarSmall.BetMin;
                    CarSmall.BetMax = postback.CarSmall.BetMax;
                    CarSmall.SingelMax = postback.CarSmall.SingelMax;
                    CarSmall.DishMax = postback.CarSmall.DishMax;

                    CarBig.BetMin = postback.CarBig.BetMin;
                    CarBig.BetMax = postback.CarBig.BetMax;
                    CarBig.SingelMax = postback.CarBig.SingelMax;
                    CarBig.DishMax = postback.CarSmall.DishMax;

                    CarOdd.BetMin = postback.CarOdd.BetMin;
                    CarOdd.BetMax = postback.CarOdd.BetMax;
                    CarOdd.SingelMax = postback.CarOdd.SingelMax;
                    CarOdd.DishMax = postback.CarSmall.DishMax;

                    CarEven.BetMin = postback.CarEven.BetMin;
                    CarEven.BetMax = postback.CarEven.BetMax;
                    CarEven.SingelMax = postback.CarEven.SingelMax;
                    CarEven.DishMax = postback.CarSmall.DishMax;

                    CarDra.BetMin = postback.CarDra.BetMin;
                    CarDra.BetMax = postback.CarDra.BetMax;
                    CarDra.SingelMax = postback.CarDra.SingelMax;
                    CarDra.DishMax = postback.CarSmall.DishMax;

                    CarTig.BetMin = postback.CarTig.BetMin;
                    CarTig.BetMax = postback.CarTig.BetMax;
                    CarTig.SingelMax = postback.CarTig.SingelMax;
                    CarTig.DishMax = postback.CarSmall.DishMax;
                    //組回Json
                    sum3 = JsonConvert.SerializeObject(Sum3);
                    sum4 = JsonConvert.SerializeObject(Sum4);
                    sum5 = JsonConvert.SerializeObject(Sum5);
                    sum6 = JsonConvert.SerializeObject(Sum6);
                    sum7 = JsonConvert.SerializeObject(Sum7);
                    sum8 = JsonConvert.SerializeObject(Sum8);
                    sum9 = JsonConvert.SerializeObject(Sum9);
                    sum10 = JsonConvert.SerializeObject(Sum10);
                    sum11 = JsonConvert.SerializeObject(Sum11);
                    sum12 = JsonConvert.SerializeObject(Sum12);
                    sum13 = JsonConvert.SerializeObject(Sum13);
                    sum14 = JsonConvert.SerializeObject(Sum14);
                    sum15 = JsonConvert.SerializeObject(Sum15);
                    sum16 = JsonConvert.SerializeObject(Sum16);
                    sum17 = JsonConvert.SerializeObject(Sum17);
                    sum18 = JsonConvert.SerializeObject(Sum18);
                    sum19 = JsonConvert.SerializeObject(Sum19);
                    sumsmall = JsonConvert.SerializeObject(SumSmall);
                    sumbig = JsonConvert.SerializeObject(SumBig);
                    sumodd = JsonConvert.SerializeObject(SumOdd);
                    sumeven = JsonConvert.SerializeObject(SumEven);
                    championbuild = JsonConvert.SerializeObject(ChampionBuild);
                    car1 = JsonConvert.SerializeObject(Car1);
                    car2 = JsonConvert.SerializeObject(Car2);
                    car3 = JsonConvert.SerializeObject(Car3);
                    car4 = JsonConvert.SerializeObject(Car4);
                    car5 = JsonConvert.SerializeObject(Car5);
                    car6 = JsonConvert.SerializeObject(Car6);
                    car7 = JsonConvert.SerializeObject(Car7);
                    car8 = JsonConvert.SerializeObject(Car8);
                    car9 = JsonConvert.SerializeObject(Car9);
                    car10 = JsonConvert.SerializeObject(Car10);
                    carsmall = JsonConvert.SerializeObject(CarSmall);
                    carbig = JsonConvert.SerializeObject(CarBig);
                    carodd = JsonConvert.SerializeObject(CarOdd);
                    careven = JsonConvert.SerializeObject(CarEven);
                    cardra = JsonConvert.SerializeObject(CarDra);
                    cartig = JsonConvert.SerializeObject(CarTig);
                }
                dr.Close();
                SqlCmd PCDedtailEdit = _HttpContext.GetSqlCmd(DB.LTDB01R);
                //SqlCommand PCDedalEdit = new SqlCommand(SqlEdit, conn);
                PCDedtailEdit.Parameters.AddWithValue("@Sum3", sum3);
                PCDedtailEdit.Parameters.AddWithValue("@Sum4", sum4);
                PCDedtailEdit.Parameters.AddWithValue("@Sum5", sum5);
                PCDedtailEdit.Parameters.AddWithValue("@Sum6", sum6);
                PCDedtailEdit.Parameters.AddWithValue("@Sum7", sum7);
                PCDedtailEdit.Parameters.AddWithValue("@Sum8", sum8);
                PCDedtailEdit.Parameters.AddWithValue("@Sum9", sum9);
                PCDedtailEdit.Parameters.AddWithValue("@Sum10", sum10);
                PCDedtailEdit.Parameters.AddWithValue("@Sum11", sum11);
                PCDedtailEdit.Parameters.AddWithValue("@Sum12", sum12);
                PCDedtailEdit.Parameters.AddWithValue("@Sum13", sum13);
                PCDedtailEdit.Parameters.AddWithValue("@Sum14", sum14);
                PCDedtailEdit.Parameters.AddWithValue("@Sum15", sum15);
                PCDedtailEdit.Parameters.AddWithValue("@Sum16", sum16);
                PCDedtailEdit.Parameters.AddWithValue("@Sum17", sum17);
                PCDedtailEdit.Parameters.AddWithValue("@Sum18", sum18);
                PCDedtailEdit.Parameters.AddWithValue("@Sum19", sum19);
                PCDedtailEdit.Parameters.AddWithValue("@SumSmall", sumsmall);
                PCDedtailEdit.Parameters.AddWithValue("@SumBig", sumbig);
                PCDedtailEdit.Parameters.AddWithValue("@SumOdd", sumodd);
                PCDedtailEdit.Parameters.AddWithValue("@SumEven", sumeven);
                PCDedtailEdit.Parameters.AddWithValue("@ChampionBuild", championbuild);
                PCDedtailEdit.Parameters.AddWithValue("@Car1", car1);
                PCDedtailEdit.Parameters.AddWithValue("@Car2", car2);
                PCDedtailEdit.Parameters.AddWithValue("@Car3", car3);
                PCDedtailEdit.Parameters.AddWithValue("@Car4", car4);
                PCDedtailEdit.Parameters.AddWithValue("@Car5", car5);
                PCDedtailEdit.Parameters.AddWithValue("@Car6", car6);
                PCDedtailEdit.Parameters.AddWithValue("@Car7", car7);
                PCDedtailEdit.Parameters.AddWithValue("@Car8", car8);
                PCDedtailEdit.Parameters.AddWithValue("@Car9", car9);
                PCDedtailEdit.Parameters.AddWithValue("@Car10", car10);
                PCDedtailEdit.Parameters.AddWithValue("@CarSmall", carsmall);
                PCDedtailEdit.Parameters.AddWithValue("@CarBig", carbig);
                PCDedtailEdit.Parameters.AddWithValue("@CarOdd", carodd);
                PCDedtailEdit.Parameters.AddWithValue("@CarEven", careven);
                PCDedtailEdit.Parameters.AddWithValue("@CarDra", cardra);
                PCDedtailEdit.Parameters.AddWithValue("@CarTig", cartig);
                PCDedtailEdit.Parameters.AddWithValue("@UpdateUser", "Danny");
                PCDedtailEdit.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                PCDedtailEdit.Parameters.AddWithValue("@CompanyID", 1);
                PCDedtailEdit.Parameters.AddWithValue("@SID", postback.SID);
                PCDedtailEdit.Parameters.AddWithValue("@Dish", Dish);

                PCDedtailEdit.ExecuteNonQuery(SqlEdit);

                //conn.Close();
                return RedirectToAction("PKTenIndex");
            }
            else
            {
                return View(postback);
            }
        }

        public ActionResult PKTenPCEnableEdit(int SID)
        {
            string sqlWater = "select SID, Dish, Sum3, Sum4, Sum5, Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15, "
                 + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4, "
                 + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                 + " from Pc_PK10_Sample where SID = " + SID;

            //conn.Open();
            //SqlCommand scmd = new SqlCommand(sqlWater, conn);
            SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = scmd.ExecuteReader(sqlWater);
            PK10_PC PcCon = new PK10_PC();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    PcCon = (new PK10_PC
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        Sum3 = Sum3,
                        Sum4 = Sum4,
                        Sum5 = Sum5,
                        Sum6 = Sum6,
                        Sum7 = Sum7,
                        Sum8 = Sum8,
                        Sum9 = Sum9,
                        Sum10 = Sum10,
                        Sum11 = Sum11,
                        Sum12 = Sum12,
                        Sum13 = Sum13,
                        Sum14 = Sum14,
                        Sum15 = Sum15,
                        Sum16 = Sum16,
                        Sum17 = Sum17,
                        Sum18 = Sum18,
                        Sum19 = Sum19,
                        SumSmall = SumSmall,
                        SumBig = SumBig,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        ChampionBuild = ChampionBuild,
                        Car1 = Car1,
                        Car2 = Car2,
                        Car3 = Car3,
                        Car4 = Car4,
                        Car5 = Car5,
                        Car6 = Car6,
                        Car7 = Car7,
                        Car8 = Car8,
                        Car9 = Car9,
                        Car10 = Car10,
                        CarSmall = CarSmall,
                        CarBig = CarBig,
                        CarOdd = CarOdd,
                        CarEven = CarEven,
                        CarDra = CarDra,
                        CarTig = CarTig
                    });
                    return View(PcCon);
                }
            }
            //conn.Close();
            return View();
        }
        [HttpPost]
        public ActionResult PKTenPCEnableEdit(PK10_PC postback)
        {
            string SqlEdit = " update Pc_PK10_Sample SET Dish=@Dish,"
                        + " Sum3 = @Sum3, Sum4 = @Sum4, Sum5 = @Sum5, Sum6 = @Sum6, Sum7 = @Sum7, "
                        + " Sum8 = @Sum8, Sum9 = @Sum9, Sum10= @Sum10, Sum11 = @Sum11, Sum12 = @Sum12, "
                        + " Sum13 = @Sum13, Sum14 = @Sum14, Sum15 = @Sum15, Sum16 = @Sum16, Sum17 = @Sum17, "
                        + " Sum18 = @Sum18, Sum19 = @Sum19, SumSmall = @SumSmall, SumBig = @SumBig, "
                        + " SumOdd = @SumOdd, SumEven = @SumEven, ChampionBuild = @ChampionBuild, "
                        + " Car1 = @Car1, Car2 = @Car2, Car3 = @Car3, Car4 = @Car5, Car5 = @Car5, Car6 = @Car6, "
                        + " Car7 = @Car7, Car8 = @Car8, Car9 = @Car9, Car10 = @Car10, CarSmall = @CarSmall, "
                        + " CarBig = @CarBig, CarOdd = @CarOdd, CarEven = @CarEven, CarDra = @CarDra, "
                        + " CarTig = @CarTig ,UpdateUser = @UpdateUser, UpdateTime = @UpdateTime, "
                        + " CompanyID = @CompanyID WHERE SID = @SID"; ;
            string Dish = "";
            string sum3 = "", sum4 = "", sum5 = "", sum6 = "", sum7 = "", sum8 = "", sum9 = "", sum10 = "", sum11 = "", sum12 = "";
            string sum13 = "", sum14 = "", sum15 = "", sum16 = "", sum17 = "", sum18 = "", sum19 = "", sumsmall = "", sumbig = "", sumodd = "", sumeven = "", championbuild = "";
            string car1 = "", car2 = "", car3 = "", car4 = "", car5 = "", car6 = "", car7 = "", car8 = "", car9 = "", car10 = "";
            string carsmall = "", carbig = "", carodd = "", careven = "", cardra = "", cartig = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "select SID, Dish, Sum3, Sum4, Sum5 , Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15,"
                + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4,"
                + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                + "from Pc_PK10_Sample where SID =" + postback.SID;
                //conn.Open();
                //SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlDataReader dr = scmd.ExecuteReader(SqlSelect);

                //Json
                while (dr.Read())
                {
                    Dish = dr[1].ToString();
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    //塞入資料
                    //總和
                    Sum3.Enable = postback.Sum3.Enable;
                    Sum4.Enable = postback.Sum3.Enable;
                    Sum5.Enable = postback.Sum3.Enable;
                    Sum6.Enable = postback.Sum3.Enable;
                    Sum7.Enable = postback.Sum3.Enable;
                    Sum8.Enable = postback.Sum3.Enable;
                    Sum9.Enable = postback.Sum3.Enable;
                    Sum10.Enable = postback.Sum3.Enable;
                    Sum11.Enable = postback.Sum3.Enable;
                    Sum12.Enable = postback.Sum3.Enable;
                    Sum13.Enable = postback.Sum3.Enable;
                    Sum14.Enable = postback.Sum3.Enable;
                    Sum15.Enable = postback.Sum3.Enable;
                    Sum16.Enable = postback.Sum3.Enable;
                    Sum17.Enable = postback.Sum3.Enable;
                    Sum18.Enable = postback.Sum3.Enable;
                    Sum19.Enable = postback.Sum3.Enable;
                    //總和大小單雙
                    SumSmall.Enable = postback.SumSmall.Enable;
                    SumBig.Enable = postback.SumSmall.Enable;
                    SumOdd.Enable = postback.SumSmall.Enable;
                    SumEven.Enable = postback.SumSmall.Enable;

                    //冠亞組合
                    ChampionBuild.Enable = postback.ChampionBuild.Enable;

                    //車                  
                    Car1.Enable = postback.Car1.Enable;
                    Car2.Enable = postback.Car1.Enable;
                    Car3.Enable = postback.Car1.Enable;
                    Car4.Enable = postback.Car1.Enable;
                    Car5.Enable = postback.Car1.Enable;
                    Car6.Enable = postback.Car1.Enable;
                    Car7.Enable = postback.Car1.Enable;
                    Car8.Enable = postback.Car1.Enable;
                    Car9.Enable = postback.Car1.Enable;
                    Car10.Enable = postback.Car1.Enable;

                    CarSmall.Enable = postback.CarSmall.Enable;

                    CarBig.BetMin = postback.CarSmall.Enable;

                    CarOdd.Enable = postback.CarSmall.Enable;

                    CarEven.Enable = postback.CarSmall.Enable;

                    CarDra.Enable = postback.CarSmall.Enable;

                    CarTig.Enable = postback.CarSmall.Enable;

                    //組回Json
                    sum3 = JsonConvert.SerializeObject(Sum3);
                    sum4 = JsonConvert.SerializeObject(Sum4);
                    sum5 = JsonConvert.SerializeObject(Sum5);
                    sum6 = JsonConvert.SerializeObject(Sum6);
                    sum7 = JsonConvert.SerializeObject(Sum7);
                    sum8 = JsonConvert.SerializeObject(Sum8);
                    sum9 = JsonConvert.SerializeObject(Sum9);
                    sum10 = JsonConvert.SerializeObject(Sum10);
                    sum11 = JsonConvert.SerializeObject(Sum11);
                    sum12 = JsonConvert.SerializeObject(Sum12);
                    sum13 = JsonConvert.SerializeObject(Sum13);
                    sum14 = JsonConvert.SerializeObject(Sum14);
                    sum15 = JsonConvert.SerializeObject(Sum15);
                    sum16 = JsonConvert.SerializeObject(Sum16);
                    sum17 = JsonConvert.SerializeObject(Sum17);
                    sum18 = JsonConvert.SerializeObject(Sum18);
                    sum19 = JsonConvert.SerializeObject(Sum19);
                    sumsmall = JsonConvert.SerializeObject(SumSmall);
                    sumbig = JsonConvert.SerializeObject(SumBig);
                    sumodd = JsonConvert.SerializeObject(SumOdd);
                    sumeven = JsonConvert.SerializeObject(SumEven);
                    championbuild = JsonConvert.SerializeObject(ChampionBuild);
                    car1 = JsonConvert.SerializeObject(Car1);
                    car2 = JsonConvert.SerializeObject(Car2);
                    car3 = JsonConvert.SerializeObject(Car3);
                    car4 = JsonConvert.SerializeObject(Car4);
                    car5 = JsonConvert.SerializeObject(Car5);
                    car6 = JsonConvert.SerializeObject(Car6);
                    car7 = JsonConvert.SerializeObject(Car7);
                    car8 = JsonConvert.SerializeObject(Car8);
                    car9 = JsonConvert.SerializeObject(Car9);
                    car10 = JsonConvert.SerializeObject(Car10);
                    carsmall = JsonConvert.SerializeObject(CarSmall);
                    carbig = JsonConvert.SerializeObject(CarBig);
                    carodd = JsonConvert.SerializeObject(CarOdd);
                    careven = JsonConvert.SerializeObject(CarEven);
                    cardra = JsonConvert.SerializeObject(CarDra);
                    cartig = JsonConvert.SerializeObject(CarTig);


                }
                dr.Close();
                SqlCmd EnableEdit = _HttpContext.GetSqlCmd(DB.LTDB01R);
                //SqlCommand PCDedalEdit = new SqlCommand(SqlEdit, conn);
                EnableEdit.Parameters.AddWithValue("@Sum3", sum3);
                EnableEdit.Parameters.AddWithValue("@Sum4", sum4);
                EnableEdit.Parameters.AddWithValue("@Sum5", sum5);
                EnableEdit.Parameters.AddWithValue("@Sum6", sum6);
                EnableEdit.Parameters.AddWithValue("@Sum7", sum7);
                EnableEdit.Parameters.AddWithValue("@Sum8", sum8);
                EnableEdit.Parameters.AddWithValue("@Sum9", sum9);
                EnableEdit.Parameters.AddWithValue("@Sum10", sum10);
                EnableEdit.Parameters.AddWithValue("@Sum11", sum11);
                EnableEdit.Parameters.AddWithValue("@Sum12", sum12);
                EnableEdit.Parameters.AddWithValue("@Sum13", sum13);
                EnableEdit.Parameters.AddWithValue("@Sum14", sum14);
                EnableEdit.Parameters.AddWithValue("@Sum15", sum15);
                EnableEdit.Parameters.AddWithValue("@Sum16", sum16);
                EnableEdit.Parameters.AddWithValue("@Sum17", sum17);
                EnableEdit.Parameters.AddWithValue("@Sum18", sum18);
                EnableEdit.Parameters.AddWithValue("@Sum19", sum19);
                EnableEdit.Parameters.AddWithValue("@SumSmall", sumsmall);
                EnableEdit.Parameters.AddWithValue("@SumBig", sumbig);
                EnableEdit.Parameters.AddWithValue("@SumOdd", sumodd);
                EnableEdit.Parameters.AddWithValue("@SumEven", sumeven);
                EnableEdit.Parameters.AddWithValue("@ChampionBuild", championbuild);
                EnableEdit.Parameters.AddWithValue("@Car1", car1);
                EnableEdit.Parameters.AddWithValue("@Car2", car2);
                EnableEdit.Parameters.AddWithValue("@Car3", car3);
                EnableEdit.Parameters.AddWithValue("@Car4", car4);
                EnableEdit.Parameters.AddWithValue("@Car5", car5);
                EnableEdit.Parameters.AddWithValue("@Car6", car6);
                EnableEdit.Parameters.AddWithValue("@Car7", car7);
                EnableEdit.Parameters.AddWithValue("@Car8", car8);
                EnableEdit.Parameters.AddWithValue("@Car9", car9);
                EnableEdit.Parameters.AddWithValue("@Car10", car10);
                EnableEdit.Parameters.AddWithValue("@CarSmall", carsmall);
                EnableEdit.Parameters.AddWithValue("@CarBig", carbig);
                EnableEdit.Parameters.AddWithValue("@CarOdd", carodd);
                EnableEdit.Parameters.AddWithValue("@CarEven", careven);
                EnableEdit.Parameters.AddWithValue("@CarDra", cardra);
                EnableEdit.Parameters.AddWithValue("@CarTig", cartig);
                EnableEdit.Parameters.AddWithValue("@UpdateUser", "Danny");
                EnableEdit.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                EnableEdit.Parameters.AddWithValue("@CompanyID", 1);
                EnableEdit.Parameters.AddWithValue("@SID", postback.SID);
                EnableEdit.Parameters.AddWithValue("@Dish", Dish);
                EnableEdit.ExecuteNonQuery(SqlEdit);
                //conn.Close();
                return RedirectToAction("PKTenIndex");
            }
            else
            {
                return View(postback);
            }
        }

        public ActionResult PKTenPCNameEdit(int SID)
        {
            //int SID = 1;
            string sqlWater = "select SID, Dish, Sum3, Sum4, Sum5, Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15, "
                 + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4, "
                 + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                 + " from Pc_PK10_Sample where SID = " + SID;

            //conn.Open();
            SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            //SqlCommand scmd = new SqlCommand(sqlWater, conn);
            SqlDataReader dr = scmd.ExecuteReader(sqlWater);
            PK10_PC PcCon = new PK10_PC();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    PcCon = (new PK10_PC
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        Sum3 = Sum3,
                        Sum4 = Sum4,
                        Sum5 = Sum5,
                        Sum6 = Sum6,
                        Sum7 = Sum7,
                        Sum8 = Sum8,
                        Sum9 = Sum9,
                        Sum10 = Sum10,
                        Sum11 = Sum11,
                        Sum12 = Sum12,
                        Sum13 = Sum13,
                        Sum14 = Sum14,
                        Sum15 = Sum15,
                        Sum16 = Sum16,
                        Sum17 = Sum17,
                        Sum18 = Sum18,
                        Sum19 = Sum19,
                        SumSmall = SumSmall,
                        SumBig = SumBig,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        ChampionBuild = ChampionBuild,
                        Car1 = Car1,
                        Car2 = Car2,
                        Car3 = Car3,
                        Car4 = Car4,
                        Car5 = Car5,
                        Car6 = Car6,
                        Car7 = Car7,
                        Car8 = Car8,
                        Car9 = Car9,
                        Car10 = Car10,
                        CarSmall = CarSmall,
                        CarBig = CarBig,
                        CarOdd = CarOdd,
                        CarEven = CarEven,
                        CarDra = CarDra,
                        CarTig = CarTig
                    });
                    return View(PcCon);
                }
            }
            //conn.Close();
            return View();
        }
        [HttpPost]
        public ActionResult PKTenPCNameEdit(PK10_PC postback)
        {
            string SqlEdit = "update Pc_PK10_Sample SET Dish = @Dish, "
                        + " Sum3 = @Sum3, Sum4 = @Sum4, Sum5 = @Sum5, Sum6 = @Sum6, Sum7 = @Sum7, "
                        + " Sum8 = @Sum8, Sum9 = @Sum9, Sum10= @Sum10, Sum11 = @Sum11, Sum12 = @Sum12, "
                        + " Sum13 = @Sum13, Sum14 = @Sum14, Sum15 = @Sum15, Sum16 = @Sum16, Sum17 = @Sum17, "
                        + " Sum18 = @Sum18, Sum19 = @Sum19, SumSmall = @SumSmall, SumBig = @SumBig, "
                        + " SumOdd = @SumOdd, SumEven = @SumEven, ChampionBuild = @ChampionBuild, "
                        + " Car1 = @Car1, Car2 = @Car2, Car3 = @Car3, Car4 = @Car5, Car5 = @Car5, Car6 = @Car6, "
                        + " Car7 = @Car7, Car8 = @Car8, Car9 = @Car9, Car10 = @Car10, CarSmall = @CarSmall, "
                        + " CarBig = @CarBig, CarOdd = @CarOdd, CarEven = @CarEven, CarDra = @CarDra, "
                        + " CarTig = @CarTig ,UpdateUser = @UpdateUser, UpdateTime = @UpdateTime, "
                        + " CompanyID = @CompanyID WHERE SID = " + postback.SID;

            string sum3 = "", sum4 = "", sum5 = "", sum6 = "", sum7 = "", sum8 = "", sum9 = "", sum10 = "", sum11 = "", sum12 = "";
            string sum13 = "", sum14 = "", sum15 = "", sum16 = "", sum17 = "", sum18 = "", sum19 = "", sumsmall = "", sumbig = "", sumodd = "", sumeven = "", championbuild = "";
            string car1 = "", car2 = "", car3 = "", car4 = "", car5 = "", car6 = "", car7 = "", car8 = "", car9 = "", car10 = "";
            string carsmall = "", carbig = "", carodd = "", careven = "", cardra = "", cartig = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "select SID, Dish, Sum3, Sum4, Sum5 , Sum6, Sum7, Sum8, Sum9, Sum10, Sum11, Sum12, Sum13, Sum14, Sum15,"
                + " Sum16, Sum17, Sum18, Sum19, SumSmall, SumBig, SumOdd, SumEven, ChampionBuild, Car1, Car2, Car3, Car4,"
                + " Car5, Car6, Car7, Car8, Car9, Car10, CarSmall, CarBig, CarOdd, CarEven, CarDra, CarTig, BallMaxMoney "
                + "from Pc_PK10_Sample where SID =" + postback.SID;
                //conn.Open();
                SqlCmd scmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                //SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlDataReader dr = scmd.ExecuteReader(SqlSelect);
                //Json
                while (dr.Read())
                {
                    PCDetail Sum3 = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail Sum4 = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail Sum5 = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail Sum6 = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail Sum7 = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail Sum8 = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail Sum9 = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail Sum10 = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail Sum11 = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail Sum12 = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail Sum13 = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail Sum14 = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail Sum15 = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail Sum16 = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Sum17 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Sum18 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Sum19 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[21].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[22].ToString());
                    PCDetail ChampionBuild = JsonConvert.DeserializeObject<PCDetail>(dr[23].ToString());
                    PCDetail Car1 = JsonConvert.DeserializeObject<PCDetail>(dr[24].ToString());
                    PCDetail Car2 = JsonConvert.DeserializeObject<PCDetail>(dr[25].ToString());
                    PCDetail Car3 = JsonConvert.DeserializeObject<PCDetail>(dr[26].ToString());
                    PCDetail Car4 = JsonConvert.DeserializeObject<PCDetail>(dr[27].ToString());
                    PCDetail Car5 = JsonConvert.DeserializeObject<PCDetail>(dr[28].ToString());
                    PCDetail Car6 = JsonConvert.DeserializeObject<PCDetail>(dr[29].ToString());
                    PCDetail Car7 = JsonConvert.DeserializeObject<PCDetail>(dr[30].ToString());
                    PCDetail Car8 = JsonConvert.DeserializeObject<PCDetail>(dr[31].ToString());
                    PCDetail Car9 = JsonConvert.DeserializeObject<PCDetail>(dr[32].ToString());
                    PCDetail Car10 = JsonConvert.DeserializeObject<PCDetail>(dr[33].ToString());
                    PCDetail CarSmall = JsonConvert.DeserializeObject<PCDetail>(dr[34].ToString());
                    PCDetail CarBig = JsonConvert.DeserializeObject<PCDetail>(dr[35].ToString());
                    PCDetail CarOdd = JsonConvert.DeserializeObject<PCDetail>(dr[36].ToString());
                    PCDetail CarEven = JsonConvert.DeserializeObject<PCDetail>(dr[37].ToString());
                    PCDetail CarDra = JsonConvert.DeserializeObject<PCDetail>(dr[38].ToString());
                    PCDetail CarTig = JsonConvert.DeserializeObject<PCDetail>(dr[39].ToString());
                    //塞入資料
                    //總和
                    Sum3.Name = postback.Sum3.Name;
                    Sum4.Name = postback.Sum4.Name;
                    Sum5.Name = postback.Sum5.Name;
                    Sum6.Name = postback.Sum6.Name;
                    Sum7.Name = postback.Sum7.Name;
                    Sum8.Name = postback.Sum8.Name;
                    Sum9.Name = postback.Sum9.Name;
                    Sum10.Name = postback.Sum10.Name;
                    Sum11.Name = postback.Sum11.Name;
                    Sum12.Name = postback.Sum12.Name;
                    Sum13.Name = postback.Sum13.Name;
                    Sum14.Name = postback.Sum14.Name;
                    Sum15.Name = postback.Sum15.Name;
                    Sum16.Name = postback.Sum16.Name;
                    Sum17.Name = postback.Sum17.Name;
                    Sum18.Name = postback.Sum18.Name;
                    Sum19.Name = postback.Sum19.Name;
                    //總和大小單雙
                    SumSmall.Name = postback.SumSmall.Name;
                    SumBig.Name = postback.SumBig.Name;
                    SumOdd.Name = postback.SumOdd.Name;
                    SumEven.Name = postback.SumEven.Name;

                    //冠亞組合
                    ChampionBuild.Name = postback.ChampionBuild.Name;

                    //車                  
                    Car1.Name = postback.Car1.Name;
                    Car2.Name = postback.Car2.Name;
                    Car3.Name = postback.Car3.Name;
                    Car4.Name = postback.Car4.Name;
                    Car5.Name = postback.Car5.Name;
                    Car6.Name = postback.Car6.Name;
                    Car7.Name = postback.Car7.Name;
                    Car8.Name = postback.Car8.Name;
                    Car9.Name = postback.Car9.Name;
                    Car10.Name = postback.Car10.Name;

                    CarSmall.Name = postback.CarSmall.Name;

                    CarBig.Name = postback.CarBig.Name;

                    CarOdd.Name = postback.CarOdd.Name;

                    CarEven.Name = postback.CarEven.Name;

                    CarDra.Name = postback.CarDra.Name;

                    CarTig.Name = postback.CarTig.Name;

                    //組回Json
                    sum3 = JsonConvert.SerializeObject(Sum3);
                    sum4 = JsonConvert.SerializeObject(Sum4);
                    sum5 = JsonConvert.SerializeObject(Sum5);
                    sum6 = JsonConvert.SerializeObject(Sum6);
                    sum7 = JsonConvert.SerializeObject(Sum7);
                    sum8 = JsonConvert.SerializeObject(Sum8);
                    sum9 = JsonConvert.SerializeObject(Sum9);
                    sum10 = JsonConvert.SerializeObject(Sum10);
                    sum11 = JsonConvert.SerializeObject(Sum11);
                    sum12 = JsonConvert.SerializeObject(Sum12);
                    sum13 = JsonConvert.SerializeObject(Sum13);
                    sum14 = JsonConvert.SerializeObject(Sum14);
                    sum15 = JsonConvert.SerializeObject(Sum15);
                    sum16 = JsonConvert.SerializeObject(Sum16);
                    sum17 = JsonConvert.SerializeObject(Sum17);
                    sum18 = JsonConvert.SerializeObject(Sum18);
                    sum19 = JsonConvert.SerializeObject(Sum19);
                    sumsmall = JsonConvert.SerializeObject(SumSmall);
                    sumbig = JsonConvert.SerializeObject(SumBig);
                    sumodd = JsonConvert.SerializeObject(SumOdd);
                    sumeven = JsonConvert.SerializeObject(SumEven);
                    championbuild = JsonConvert.SerializeObject(ChampionBuild);
                    car1 = JsonConvert.SerializeObject(Car1);
                    car2 = JsonConvert.SerializeObject(Car2);
                    car3 = JsonConvert.SerializeObject(Car3);
                    car4 = JsonConvert.SerializeObject(Car4);
                    car5 = JsonConvert.SerializeObject(Car5);
                    car6 = JsonConvert.SerializeObject(Car6);
                    car7 = JsonConvert.SerializeObject(Car7);
                    car8 = JsonConvert.SerializeObject(Car8);
                    car9 = JsonConvert.SerializeObject(Car9);
                    car10 = JsonConvert.SerializeObject(Car10);
                    carsmall = JsonConvert.SerializeObject(CarSmall);
                    carbig = JsonConvert.SerializeObject(CarBig);
                    carodd = JsonConvert.SerializeObject(CarOdd);
                    careven = JsonConvert.SerializeObject(CarEven);
                    cardra = JsonConvert.SerializeObject(CarDra);
                    cartig = JsonConvert.SerializeObject(CarTig);
                }
                dr.Close();
                SqlCmd PCNameEdit = _HttpContext.GetSqlCmd(DB.LTDB01R);
                //SqlCommand PCDedalEdit = new SqlCommand(SqlEdit, conn);
                PCNameEdit.Parameters.AddWithValue("@Sum3", sum3);
                PCNameEdit.Parameters.AddWithValue("@Sum4", sum4);
                PCNameEdit.Parameters.AddWithValue("@Sum5", sum5);
                PCNameEdit.Parameters.AddWithValue("@Sum6", sum6);
                PCNameEdit.Parameters.AddWithValue("@Sum7", sum7);
                PCNameEdit.Parameters.AddWithValue("@Sum8", sum8);
                PCNameEdit.Parameters.AddWithValue("@Sum9", sum9);
                PCNameEdit.Parameters.AddWithValue("@Sum10", sum10);
                PCNameEdit.Parameters.AddWithValue("@Sum11", sum11);
                PCNameEdit.Parameters.AddWithValue("@Sum12", sum12);
                PCNameEdit.Parameters.AddWithValue("@Sum13", sum13);
                PCNameEdit.Parameters.AddWithValue("@Sum14", sum14);
                PCNameEdit.Parameters.AddWithValue("@Sum15", sum15);
                PCNameEdit.Parameters.AddWithValue("@Sum16", sum16);
                PCNameEdit.Parameters.AddWithValue("@Sum17", sum17);
                PCNameEdit.Parameters.AddWithValue("@Sum18", sum18);
                PCNameEdit.Parameters.AddWithValue("@Sum19", sum19);
                PCNameEdit.Parameters.AddWithValue("@SumSmall", sumsmall);
                PCNameEdit.Parameters.AddWithValue("@SumBig", sumbig);
                PCNameEdit.Parameters.AddWithValue("@SumOdd", sumodd);
                PCNameEdit.Parameters.AddWithValue("@SumEven", sumeven);
                PCNameEdit.Parameters.AddWithValue("@ChampionBuild", championbuild);
                PCNameEdit.Parameters.AddWithValue("@Car1", car1);
                PCNameEdit.Parameters.AddWithValue("@Car2", car2);
                PCNameEdit.Parameters.AddWithValue("@Car3", car3);
                PCNameEdit.Parameters.AddWithValue("@Car4", car4);
                PCNameEdit.Parameters.AddWithValue("@Car5", car5);
                PCNameEdit.Parameters.AddWithValue("@Car6", car6);
                PCNameEdit.Parameters.AddWithValue("@Car7", car7);
                PCNameEdit.Parameters.AddWithValue("@Car8", car8);
                PCNameEdit.Parameters.AddWithValue("@Car9", car9);
                PCNameEdit.Parameters.AddWithValue("@Car10", car10);
                PCNameEdit.Parameters.AddWithValue("@CarSmall", carsmall);
                PCNameEdit.Parameters.AddWithValue("@CarBig", carbig);
                PCNameEdit.Parameters.AddWithValue("@CarOdd", carodd);
                PCNameEdit.Parameters.AddWithValue("@CarEven", careven);
                PCNameEdit.Parameters.AddWithValue("@CarDra", cardra);
                PCNameEdit.Parameters.AddWithValue("@CarTig", cartig);
                PCNameEdit.Parameters.AddWithValue("@UpdateUser", "Danny");
                PCNameEdit.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                PCNameEdit.Parameters.AddWithValue("@CompanyID", 1);
                PCNameEdit.Parameters.AddWithValue("@SID", postback.SID);
                PCNameEdit.Parameters.AddWithValue("@Dish", postback.Dish);
                PCNameEdit.ExecuteNonQuery(SqlEdit);

                //conn.Close();
                //return View();
                return RedirectToAction("PKTenIndex");
            }
            else
            {
                return View(postback);
            }
        }
    }
}