using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameControl.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using GameControl.ViewModels;
namespace ams.Areas.GameControl.Controllers
{
    [RouteArea(_url.areas.Lottery)]
    [Route("~/lottery/{action}")]
    public class URLController : _Controller
    {
        const string MSSQL_DBconnect = "MSSQL_DBconnect";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[MSSQL_DBconnect].ToString());

        public void ListContent(string select)
        {
            List<Category> GameType = new List<Category>();
            List<Category> Location = new List<Category>();
            GameType.Add(new Category() { Text = "基諾彩", Value = "01" });
            GameType.Add(new Category() { Text = "時時彩", Value = "02" });
            GameType.Add(new Category() { Text = "快三", Value = "03" });

            Location.Add(new Category() { Text = "北京快樂8", Value = "01" });
            Location.Add(new Category() { Text = "台灣賓果賓果", Value = "02" });
            Location.Add(new Category() { Text = "加拿大", Value = "03" });
            Location.Add(new Category() { Text = "加拿大西部", Value = "04" });
            Location.Add(new Category() { Text = "斯洛伐克", Value = "05" });
            Location.Add(new Category() { Text = "俄亥俄", Value = "06" });
            SelectList GameTypeSelectlist = new SelectList(GameType, "Value", "Text");
            SelectList LocationSelectlist = new SelectList(Location, "Value", "Text", select);
            ViewData["GameType"] = new SelectList(GameType, "Value", "Text");
            ViewData["Location"] = new SelectList(Location, "Value", "Text", select);
            //ViewBag.GameType = GameTypeSelectlist;
            //ViewBag.Location = LocationSelectlist;
        }

        #region -- URL --
        // GET: URL
        public ActionResult Index()
        {
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            List<URL> results = new List<URL>();
            foreach (SqlDataReader dr in sqlcmd.ExecuteReader("SELECT SID, GameType, Location, Url as Url1, Status, Sort, UpdateTime, UpdateUser, LoadTime, FailCount FROM URL"))
                results.Add(dr.ToObject<URL>());
            return View(results);

        }
        /*
        // GET: URL
        public ActionResult Index()
        {
            using (SqlCmd sqlcmd = new SqlCmd(DB.LTDB01R))
            {
                List<URL> results = new List<URL>();
                foreach (SqlDataReader dr in sqlcmd.ExecuteReader("SELECT SID, GameType, Location, Url as Url1, Status, Sort, UpdateTime, UpdateUser, LoadTime, FailCount FROM URL"))
                    results.Add(dr.ToObject<URL>());
                return View(results);
            }
        }
        */

        //public ActionResult Index()
        //{
        //    string sql = "SELECT SID, GameType, Location, Url, Status, Sort, UpdateTime, UpdateUser, LoadTime, FailCount FROM URL";
        //    //SqlCommand scmd = new SqlCommand(sql, conn);
        //    SqlCmd scm = new SqlCmd(sql);
        //    Connect.Connection.Open();
        //    // conn.Open();
        //    using (SqlCmd dr = scm.ExecuteReader())
        //    {
        //        List<URL> results = new List<URL>();

        //        while (dr.Read())
        //        {
        //            URL Item = new URL();

        //            Item.SID = dr.GetInt32(0);
        //            Item.GameType = dr.GetString(1);
        //            Item.Location = dr.GetString(2);
        //            Item.Url1 = dr.GetString(3);
        //            Item.Status = dr.GetInt32(4);
        //            Item.Sort = dr.GetInt32(5);
        //            Item.UpdateTime = dr.GetDateTime(6);
        //            Item.UpdateUser = dr.GetString(7);
        //            Item.LoadTime = dr.GetInt32(8);
        //            Item.FailCount = dr.GetInt32(9);
        //            results.Add(Item);
        //        }
        //        return View(results);
        //    }
        //}

        //Create URL Set
        public ActionResult Create()
        {
            ListContent("0");
            return View();
        }

        [HttpPost]
        public ActionResult Create(URL postback)
        {

            if (this.ModelState.IsValid)
            {
                string create = "Insert URL (GameType, Location, Url, Status, Sort, UpdateUser, UpdateTime, LoadTime, FailCount)Values("
                    + "@GameType, @Location, @Url, @Status, @Sort, @UpdateUser, @UpdateTime, @LoadTime, @FailCount) ";
                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sqlcmd.Parameters.AddWithValue("@GameType", postback.GameType);
                sqlcmd.Parameters.AddWithValue("@Location", postback.Location);
                sqlcmd.Parameters.AddWithValue("@Url", postback.Url1);
                sqlcmd.Parameters.AddWithValue("@Status", postback.Status);
                sqlcmd.Parameters.AddWithValue("@Sort", postback.Sort);
                sqlcmd.Parameters.AddWithValue("@UpdateUser", "Danny");
                sqlcmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                sqlcmd.Parameters.AddWithValue("@LoadTime", postback.LoadTime);
                sqlcmd.Parameters.AddWithValue("@FailCount", postback.FailCount);

                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(create);
                    sqlcmd.Commit();
                }
                catch (Exception ex)
                {
                    sqlcmd.Rollback();
                }
            }
            ViewBag.ResultMessage = "Error";
            return View(postback);
        }

        public ActionResult Edit(int SID)
        {
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);

            SqlDataReader dr = sqlcmd.ExecuteReader("Select SID, GameType, Location, Url, Status, Sort, LoadTime, FailCount from Url where SID = " + SID);
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    URL results = new URL()
                    {
                        SID = (int)dr[0],
                        GameType = dr[1].ToString(),
                        Location = dr[2].ToString(),
                        Url1 = dr[3].ToString(),
                        Status = (int)dr[4],
                        Sort = (int)dr[5],
                        LoadTime = (int)dr[6],
                        FailCount = (int)dr[7]
                    };
                    ListContent(results.Location);
                    conn.Close();
                    return View(results);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(URL postback)
        {
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "Select SID, GameType, Location, Url, Status, Sort, LoadTime, FailCount from Url where SID = " + postback.SID;
                string SortSelect = "Select Sort from Url where Location = " + postback.Location + "and GameType = " + postback.GameType + "and Sort = " + postback.Sort;
                string SqlUpdate = "UPDATE URL SET "
                + " GameType = N'" + postback.GameType + "',"
                + " Location = N'" + postback.Location + "',"
                + " Url = N'" + postback.Url1 + "',"
                + " Status = " + postback.Status + ","
                + " Sort = " + postback.Sort + ","
                + " UpdateUser = N'Danny',"
                + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "',"
                + " LoadTime = " + postback.LoadTime + ","
                + " FailCount = " + postback.FailCount
                + " WHERE SID = " + postback.SID;
                int SortTmp1 = 0;
                int SortTmp2 = 0;

                SqlCmd SqlRead = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlCmd SqlSort = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlCmd SqlEdit = _HttpContext.GetSqlCmd(DB.LTDB01W);

                SqlDataReader drSelect = SqlRead.ExecuteReader();
                drSelect.Read();
                SortTmp1 = (int)drSelect[5];
                drSelect.Close();

                SqlDataReader drSort = SqlSort.ExecuteReader();
                if (drSort.Read())
                    SortTmp2 = (int)drSort[0];
                else
                    SortTmp2 = 0;
                drSort.Close();

                if (SortTmp1 == postback.Sort)
                {
                    SqlEdit.BeginTransaction();
                    try
                    {
                        SqlEdit.ExecuteNonQuery(SqlUpdate);
                        SqlEdit.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        SqlEdit.Rollback();
                        return View(postback);
                    }
                }
                else if (SortTmp2 == 0)
                {
                    SqlEdit.BeginTransaction();
                    try
                    {
                        SqlEdit.ExecuteNonQuery(SqlUpdate);
                        SqlEdit.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        SqlEdit.Rollback();
                        return View(postback);
                    }
                }
                else
                {
                    ListContent(postback.Location);
                    ViewBag.Error = "排序重複";
                    return View(postback);
                }
            }
            return View(postback);
        }

        public ActionResult Delete(int SID)
        {
            string SqlEdit = "Select SID, GameType, Location, Url, Status, Sort, LoadTime, FailCount from Url where SID = " + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(SqlEdit);
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    string GT = "";
                    string LC = "";
                    if (dr[1].ToString() == "01")
                    {
                        GT = "基諾彩";
                    }
                    if (dr[2].ToString() == "01")
                    {
                        LC = "北京快樂8";
                    }
                    else if (dr[2].ToString() == "02")
                    {
                        LC = "台灣賓果賓果";
                    }
                    else if (dr[2].ToString() == "03")
                    {
                        LC = "加拿大";
                    }
                    else if (dr[2].ToString() == "04")
                    {
                        LC = "加拿大西部";
                    }
                    else if (dr[2].ToString() == "05")
                    {
                        LC = "斯洛伐克";
                    }
                    else if (dr[2].ToString() == "06")
                    {
                        LC = "俄亥俄";
                    }
                    URL results = new URL()
                    {
                        SID = (int)dr[0],
                        GameType = GT,
                        Location = LC,
                        Url1 = dr[3].ToString(),
                        Status = (int)dr[4],
                        Sort = (int)dr[5],
                        LoadTime = (int)dr[6],
                        FailCount = (int)dr[7]
                    };
                    ListContent(results.Location);
                    return View(results);
                }
            }
            conn.Close();
            return View();
        }

        [HttpPost]
        public ActionResult Delete(URL postback)
        {
            string sql = "DELETE FROM Url WHERE SID =" + postback.SID;
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
            sqlcmd.BeginTransaction();
            try
            {
                sqlcmd.ExecuteNonQuery(sql);
                sqlcmd.Commit();
                return RedirectToAction("Index");
            }
            catch
            {
                sqlcmd.Rollback();
                return RedirectToAction("Index");
            }

        }
        #endregion
        #region -- PassAccount --
        /// <summary>
        /// PassAccount過脹
        /// </summary>
        /// <returns></returns>
        public ActionResult PassAccountIndex()
        {
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            List<PassAccount> results = new List<PassAccount>();
            foreach (SqlDataReader dr in sqlcmd.ExecuteReader("SELECT SID, UserID, GameType, Location, Status, UpdateTime, UpdateUser FROM PassAccount"))
                results.Add(dr.ToObject<PassAccount>());
            return View(results);
            /*
            string sql = "SELECT SID, UserID, GameType, Location, Status, UpdateTime, UpdateUser FROM PassAccount";
            SqlCommand scmd = new SqlCommand(sql, conn);
            using (conn)
            {
                conn.Open();
                using (SqlDataReader dr = scmd.ExecuteReader())
                {
                    List<PassAccount> results = new List<PassAccount>();
                    while (dr.Read())
                    {
                        PassAccount Item = new PassAccount();
                        Item.SID = dr.GetInt32(0);
                        Item.UserID = dr.GetString(1);
                        Item.GameType = dr.GetString(2);
                        Item.Location = dr.GetString(3);
                        Item.Status = dr.GetInt32(4);
                        Item.UpdateTime = dr.GetDateTime(5);
                        Item.UpdateUser = dr.GetString(6);
                        results.Add(Item);
                    }
                    return View(results);
                }
            }*/
        }

        //Create PassAccount Set
        public ActionResult PassAccountCreate()
        {
            ListContent("0");
            return View();
        }

        [HttpPost]
        public ActionResult PassAccountCreate(PassAccount postback)
        {
            if (this.ModelState.IsValid)
            {
                ListContent("0");
                string Create = "Insert PassAccount (UserID, GameType, Location, Status, UpdateUser, UpdateTime, CompanyID)Values( "
                    + "N'Danny',"
                    + "N'" + postback.GameType + "',"
                    + "N'" + postback.Location + "',"
                    + postback.Status + ","
                    + "N'Danny',"
                    + "N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "',"
                    + 1
                    + " )";
                SqlCommand SqlCreate = new SqlCommand(Create, conn);
                using (conn)
                {
                    conn.Open();
                    SqlCreate.ExecuteNonQuery();
                }
                return RedirectToAction("PassAccountIndex");
            }
            ViewBag.ResultMessage = "Error";
            return View(postback);

            if (ModelState.IsValid)
            {
                ListContent("0");
                string Create = "Insert PassAccount (UserID, GameType, Location, Status, UpdateUser, UpdateTime, CompanyID)Values( "
                    + "@UserID, @GameType, @Location, @Status, @UpdateUser, @UpdateTime, @CompanyID)";
                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sqlcmd.Parameters.AddWithValue("@UserID", "Danny");
                sqlcmd.Parameters.AddWithValue("@GameType", postback.GameType);
                sqlcmd.Parameters.AddWithValue("@Location", postback.Location);
                sqlcmd.Parameters.AddWithValue("@Status", postback.Status);
                sqlcmd.Parameters.AddWithValue("@UpdateUser", "Danny");
                sqlcmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                sqlcmd.Parameters.AddWithValue("@CompanyID", 1);
                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(Create);
                    sqlcmd.Commit();
                }
                catch
                {
                    sqlcmd.Rollback();
                }
            }
        }
        //Edit PassAccount Set
        public ActionResult PassAccountEdit(int SID)
        {
            string SqlEdit = "Select  SID, GameType, Location, Status  from PassAccount where SID = " + SID;
            conn.Open();
            SqlCommand scmd = new SqlCommand(SqlEdit, conn);
            SqlDataReader dr = scmd.ExecuteReader();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    PassAccount results = new PassAccount()
                    {
                        SID = (int)dr[0],
                        GameType = dr[1].ToString(),
                        Location = dr[2].ToString(),
                        Status = (int)dr[3]
                    };
                    ListContent(results.Location);
                    return View(results);
                }
            }
            conn.Close();
            return View();
        }

        [HttpPost]
        public ActionResult PassAccountEdit(PassAccount postback)
        {

            if (this.ModelState.IsValid)
            {
                string SqlEdit = "UPDATE PassAccount SET "
               + " GameType = N'" + postback.GameType + "',"
               + " Location = N'" + postback.Location + "',"
               + " Status = " + postback.Status + ","
               + " UpdateUser = N'Danny',"
               + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "'"
               + " WHERE SID = " + postback.SID;
                conn.Open();
                using (SqlCommand SqlUpdate = new SqlCommand(SqlEdit, conn))
                {
                    SqlUpdate.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("PassAccountIndex");
                }
            }
            else
            {
                return View(postback);
            }

            if (ModelState.IsValid)
            {
                string SqlEdit = "UPDATE PassAccount SET "
               + " GameType = @GameType,"
               + " Location = @Location,"
               + " Status = @Status,"
               + " UpdateUser = @UpdateUser,"
               + " UpdateTime = @UpdateTime"
               + " WHERE SID = @SID";
                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sqlcmd.Parameters.AddWithValue("@GameType", postback.GameType);
                sqlcmd.Parameters.AddWithValue("@Location", postback.Location);
                sqlcmd.Parameters.AddWithValue("@Status", postback.Status);
                sqlcmd.Parameters.AddWithValue("@UpdateUser", postback.UpdateUser);
                sqlcmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                sqlcmd.Parameters.AddWithValue("@SID", postback.SID);
                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(SqlEdit);
                    sqlcmd.Commit();
                }
                catch
                {
                    sqlcmd.Rollback();
                }
            }

        }
        //Delay PassAccount
        public ActionResult PassAccountDelete(int SID)
        {
            string sql = "SELECT SID, UserID, GameType, Location, Status FROM PassAccount Where SID=" + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(sql);
            while (dr.Read())
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    string GT = "";
                    string LC = "";
                    if (dr[2].ToString() == "01")
                    {
                        GT = "基諾彩";
                    }
                    if (dr[3].ToString() == "01")
                    {
                        LC = "北京快樂8";
                    }
                    else if (dr[3].ToString() == "02")
                    {
                        LC = "台灣賓果賓果";
                    }
                    else if (dr[3].ToString() == "03")
                    {
                        LC = "加拿大";
                    }
                    else if (dr[3].ToString() == "04")
                    {
                        LC = "加拿大西部";
                    }
                    else if (dr[3].ToString() == "05")
                    {
                        LC = "斯洛伐克";
                    }
                    else if (dr[3].ToString() == "06")
                    {
                        LC = "俄亥俄";
                    }
                    PassAccount results = new PassAccount()
                    {
                        SID = (int)dr[0],
                        UserID = dr[1].ToString(),
                        GameType = GT,
                        Location = LC,
                        Status = (int)dr[4]
                    };
                    return View(results);
                }

            }
            return View();
            /*
            SqlCommand scmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = scmd.ExecuteReader();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    string GT = "";
                    string LC = "";
                    if (dr[2].ToString() == "01")
                    {
                        GT = "基諾彩";
                    }
                    if (dr[3].ToString() == "01")
                    {
                        LC = "北京快樂8";
                    }
                    else if (dr[3].ToString() == "02")
                    {
                        LC = "台灣賓果賓果";
                    }
                    else if (dr[3].ToString() == "03")
                    {
                        LC = "加拿大";
                    }
                    else if (dr[3].ToString() == "04")
                    {
                        LC = "加拿大西部";
                    }
                    else if (dr[3].ToString() == "05")
                    {
                        LC = "斯洛伐克";
                    }
                    else if (dr[3].ToString() == "06")
                    {
                        LC = "俄亥俄";
                    }
                    PassAccount results = new PassAccount()
                    {
                        SID = (int)dr[0],
                        UserID = dr[1].ToString(),
                        GameType = GT,
                        Location = LC,
                        Status = (int)dr[4]
                    };
                    return View(results);
                }
            }
            conn.Close();
            return View();
            */
        }

        [HttpPost]
        public ActionResult PassAccountDelete(PassAccount postback)
        {

            string sql = "DELETE FROM PassAccount WHERE SID =" + postback.SID;
            /*
            conn.Open();
            using (SqlCommand sqlcmd = new SqlCommand(sql, conn))
            {
                sqlcmd.ExecuteNonQuery();
            }
            conn.Close();
            return RedirectToAction("PassAccountIndex");
            */

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
            sqlcmd.BeginTransaction();
            try
            {
                sqlcmd.ExecuteNonQuery(sql);
                sqlcmd.Commit();
                return RedirectToAction("PassAccountIndex");
            }
            catch
            {
                sqlcmd.Rollback();
                return View(postback);
            }
        }
        #endregion
        #region -- LotteryDelay(Closing) --
        //LotteryDelay Set
        public ActionResult LotteryDelayIndex()
        {
            /*
            List<Models.LotteryDelay> result = new List<Models.LotteryDelay>();
            using (Models.game_DBEntities db = new Models.game_DBEntities())
            {
                result = (from s in db.LotteryDelays select s).ToList();
                return View(result);
            }*/
            return View();
        }

        //Create LotteryDelay 
        public ActionResult LotteryDelayCreate()
        {
            //ListContent("0");
            return View();
        }
        [HttpPost]
        public ActionResult LotteryDelayCreate(LotteryDelay postback)
        {/*
            ListContent("0");
            if (this.ModelState.IsValid)
            {
                using (Models.game_DBEntities db = new Models.game_DBEntities())
                {
                    postback.UserID = "Danny";
                    postback.UpdateTime = DateTime.Now;
                    postback.UpdateUser = "Danny";
                    db.LotteryDelays.Add(postback);
                    db.SaveChanges();
                    TempData["ResultMessage"] = string.Format("{0} Create", postback.GameType);
                    return RedirectToAction("LotteryDelayIndex");
                }
            }
            ViewBag.ResultMessage = "Error";
            return View(postback);*/
            return View();
        }
        //Edit PassAccount Set
        public ActionResult LotteryDelayEdit(int SID)
        {
            /*
            using (Models.game_DBEntities db = new Models.game_DBEntities())
            {
                var result = (from s in db.LotteryDelays where s.SID == SID select s).FirstOrDefault();
                ListContent(result.Location);
                if (result != default(Models.LotteryDelay))
                {
                    return View(result);
                }
                else
                {
                    TempData["resultMessage"] = "Error";
                    return RedirectToAction("LotteryDelayIndex");
                }
            }*/
            return View();
        }

        [HttpPost]
        public ActionResult LotteryDelayEdit(LotteryDelay postback)
        {
            /*
            if (this.ModelState.IsValid)
            {
                using (Models.game_DBEntities db = new Models.game_DBEntities())
                {
                    var result = (from s in db.LotteryDelays where s.SID == postback.SID select s).FirstOrDefault();
                    result.GameType = postback.GameType;
                    result.Location = postback.Location;
                    result.Delay = postback.Delay;
                    result.UpdateTime = DateTime.Now;
                    result.UpdateUser = result.UserID;
                    db.SaveChanges();
                    TempData["ResultMessage"] = string.Format("{0}成功編輯", postback.GameType);
                    return RedirectToAction("LotteryDelayIndex");
                }
            }
            else
            {
                return View(postback);
            }
            */
            return View();
        }
        //Delete LotteryDelay
        [HttpPost]
        public ActionResult LotteryDelayDelete(int SID)
        {
            /*
            using (Models.game_DBEntities db = new Models.game_DBEntities())
            {
                var result = (from s in db.LotteryDelays where s.SID == SID select s).FirstOrDefault();
                if (result != default(Models.LotteryDelay))
                {
                    db.LotteryDelays.Remove(result);
                    db.SaveChanges();
                    TempData["ResultMessage"] = string.Format("{0} id delete", result.GameType);
                    return RedirectToAction("LotteryDelayIndex");
                }
                else
                {
                    TempData["ResultMessage"] = "Error";
                    return RedirectToAction("LotteryDelayIndex");
                }
            }*/
            return View();
        }
        #endregion
        #region GameClose
        /// <summary>
        /// GameClose
        /// </summary>
        /// <returns></returns>
        public ActionResult GameCloseIndex()
        {
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            List<GameClose> results = new List<GameClose>();
            foreach (SqlDataReader dr in sqlcmd.ExecuteReader("SELECT SID, GameType, Location, CloseTime, UpdateTime, UpdateUser  FROM GameClose"))
                results.Add(dr.ToObject<GameClose>());
            return View(results);
        }

        //Create GameClose 
        public ActionResult GameCloseCreate()
        {
            ListContent("0");
            return View();
        }
        [HttpPost]
        public ActionResult GameCloseCreate(GameClose postback)
        {

            if (ModelState.IsValid)
            {
                ListContent("0");
                string Create = "Insert GameClose(UserID, GameType, Location, CloseTime, UpdateUser, UpdateTime, CompanyID)Values("
                    + "@UserID, @GameType, @Location, @CloseTime, @UpdateUser, @UpdateTime, @CompanyID)";

                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sqlcmd.Parameters.AddWithValue("@UserID", "Danny");
                sqlcmd.Parameters.AddWithValue("@GameType", postback.GameType);
                sqlcmd.Parameters.AddWithValue("@Location", postback.Location);
                sqlcmd.Parameters.AddWithValue("@CloseTime", postback.CloseTime);
                sqlcmd.Parameters.AddWithValue("@UpdateUser", "Danny");
                sqlcmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                sqlcmd.Parameters.AddWithValue("@CompanyID", 0);
                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(Create);
                    sqlcmd.Commit();
                    return RedirectToAction("GameCloseIndex");
                }
                catch
                {
                    sqlcmd.Rollback();
                    ViewBag.ResultMessage = "Error";
                    return View(postback);
                }
            }
            return View(postback);
        }
        //Edit GameClose Set
        public ActionResult GameCloseEdit(int SID)
        {
            string SqlEdit = "Select  SID, GameType, Location, CloseTime  from GameClose where SID = " + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(SqlEdit);
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    GameClose results = new GameClose()
                    {
                        SID = (int)dr[0],
                        GameType = dr[1].ToString(),
                        Location = dr[2].ToString(),
                        CloseTime = (int)dr[3]
                    };
                    ListContent(results.Location);
                    return View(results);
                }
            }
            return View();

        }

        [HttpPost]
        public ActionResult GameCloseEdit(GameClose postback)
        {
            if (ModelState.IsValid)
            {
                string SqlEdit = "UPDATE GameClose SET "
                    + " GameType = @GameType ,"
                    + " Location = @Location ,"
                    + " CloseTime = @CloseTime ,"
                    + " UpdateUser = @UpdateUser ,"
                    + " UpdateTime = @UpdateTime ,"
                    + " CompanyID = 1"
                    + " WHERE SID = @SID";
                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sqlcmd.Parameters.AddWithValue("@GameType", postback.GameType);
                sqlcmd.Parameters.AddWithValue("@Location", postback.Location);
                sqlcmd.Parameters.AddWithValue("@CloseTime", postback.CloseTime);
                sqlcmd.Parameters.AddWithValue("@UpdateUser", "Danny");
                sqlcmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                sqlcmd.Parameters.AddWithValue("@SID", postback.SID);
                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(SqlEdit);
                    sqlcmd.Commit();
                    return RedirectToAction("GameCloseIndex");
                }
                catch
                {
                    sqlcmd.Rollback();
                    return View(postback);
                }
            }
            return View(postback);
        }
        //Delete GameClose
        public ActionResult GameCloseDelete(int SID)
        {
            string sql = "SELECT SID, UserID, GameType, Location, CloseTime FROM GameClose Where SID=" + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(sql);
            while (dr.Read())
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    string GT = "";
                    string LC = "";
                    if (dr[2].ToString() == "01")
                    {
                        GT = "基諾彩";
                    }
                    if (dr[3].ToString() == "01")
                    {
                        LC = "北京快樂8";
                    }
                    else if (dr[3].ToString() == "02")
                    {
                        LC = "台灣賓果賓果";
                    }
                    else if (dr[3].ToString() == "03")
                    {
                        LC = "加拿大";
                    }
                    else if (dr[3].ToString() == "04")
                    {
                        LC = "加拿大西部";
                    }
                    else if (dr[3].ToString() == "05")
                    {
                        LC = "斯洛伐克";
                    }
                    else if (dr[3].ToString() == "06")
                    {
                        LC = "俄亥俄";
                    }
                    GameClose results = new GameClose()
                    {
                        SID = (int)dr[0],
                        UserID = dr[1].ToString(),
                        GameType = GT,
                        Location = LC,
                        CloseTime = (int)dr[4]
                    };
                    return View(results);
                }
            }
            conn.Close();
            return View();
            /*
            SqlCommand scmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = scmd.ExecuteReader();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    string GT = "";
                    string LC = "";
                    if (dr[2].ToString() == "01")
                    {
                        GT = "基諾彩";
                    }
                    if (dr[3].ToString() == "01")
                    {
                        LC = "北京快樂8";
                    }
                    else if (dr[3].ToString() == "02")
                    {
                        LC = "台灣賓果賓果";
                    }
                    else if (dr[3].ToString() == "03")
                    {
                        LC = "加拿大";
                    }
                    else if (dr[3].ToString() == "04")
                    {
                        LC = "加拿大西部";
                    }
                    else if (dr[3].ToString() == "05")
                    {
                        LC = "斯洛伐克";
                    }
                    else if (dr[3].ToString() == "06")
                    {
                        LC = "俄亥俄";
                    }
                    GameClose results = new GameClose()
                    {
                        SID = (int)dr[0],
                        UserID = dr[1].ToString(),
                        GameType = GT,
                        Location = LC,
                        CloseTime = (int)dr[4]
                    };
                    return View(results);
                }
            }
            conn.Close();
            return View();
            */
        }

        [HttpPost]
        public ActionResult GameCloseDelete(GameClose postback)
        {

            string sql = "DELETE FROM GameClose WHERE SID =" + postback.SID;
            /*
            conn.Open();
            using (SqlCommand sqlcmd = new SqlCommand(sql, conn))
            {
                sqlcmd.ExecuteNonQuery();
            }
            conn.Close();
            return RedirectToAction("GameCloseIndex");
            */
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
            sqlcmd.BeginTransaction();
            try
            {
                sqlcmd.ExecuteNonQuery(sql);
                sqlcmd.Commit();
                return RedirectToAction("GameCloseIndex");
            }
            catch
            {
                sqlcmd.Rollback();
                return View(postback);
            }
        }
        #endregion
        /// <summary>
        /// PCIndex
        /// </summary>
        /// <param name="location"> 開獎地區</param>
        /// <returns></returns>
        public ActionResult PCIndex(string location)
        {
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            List<Pc_Keno_Sample> results = new List<Pc_Keno_Sample>();
            foreach (SqlDataReader dr in sqlcmd.ExecuteReader("SELECT SID, Dish, UpdateTime, UpdateUser FROM Pc_Keno_Sample"))
                results.Add(dr.ToObject<Pc_Keno_Sample>());
            return View(results);
            /*
            string sql = "SELECT SID, Dish, UpdateTime, UpdateUser FROM Pc_Keno_Sample";
            SqlCommand scmd = new SqlCommand(sql, conn);
            using (conn)
            {
                conn.Open();
                using (SqlDataReader dr = scmd.ExecuteReader())
                {
                    List<Pc_Keno_Sample> results = new List<Pc_Keno_Sample>();
                    while (dr.Read())
                    {
                        Pc_Keno_Sample Item = new Pc_Keno_Sample();
                        Item.SID = dr.GetInt32(0);
                        Item.Dish = dr.GetString(1);
                        Item.UpdateTime = dr.GetDateTime(2);
                        Item.UpdateUser = dr.GetString(3);
                        results.Add(Item);
                    }
                    return View(results);
                }
            }*/
        }

        //Create PC
        public ActionResult PCCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PCCreate(Pc_Keno_Sample postback)
        {
            if (ModelState.IsValid)
            {
                ListContent("0");
                string Create = "Insert Pc_Keno_Sample ( Location, Dish, UserID, UpdateUser, UpdateTime, CompanyID)Values( "
                    + " @Location, @Dish, @UserID, @UpdateUser, @UpdateTime, @CompanyID)";
                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sqlcmd.Parameters.AddWithValue("@Location", "01");
                sqlcmd.Parameters.AddWithValue("@Dish", postback.Dish);
                sqlcmd.Parameters.AddWithValue("@UserID", "Danny");
                sqlcmd.Parameters.AddWithValue("@UpdateUser", "Danny");
                sqlcmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                sqlcmd.Parameters.AddWithValue("@CompanyID", 1);
                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(Create);
                    sqlcmd.Commit();
                    return RedirectToAction("PCIndex");
                }
                catch
                {
                    sqlcmd.Rollback();
                    ViewBag.ResultMessage = "Error";
                    return View(postback);
                }
            }
            return View(postback);
        }

        //PC Enable
        public ActionResult PCEnableEdit(int SID)
        {
            string SqlEdit = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
                + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5 from Pc_Keno_Sample where SID = " + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
            SqlDataReader dr = sqlcmd.ExecuteReader(SqlEdit);

            /*
            conn.Open();
            SqlCommand scmd = new SqlCommand(SqlEdit, conn);
            SqlDataReader dr = scmd.ExecuteReader();*/
            PCControl PcCon = new PCControl();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PcCon = (new PCControl
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        BallSmall = BallSmall,
                        BallMid = BallMid,
                        BallBig = BallBig,
                        BallOdd = BallOdd,
                        BallSam = BallSam,
                        BallEven = BallEven,
                        SumBig = SumBig,
                        SumSmall = SumSmall,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        SumBigOdd = SumBigOdd,
                        SumSmallOdd = SumSmallOdd,
                        SumBigEven = SumBigEven,
                        SumSmallEven = SumSmallEven,
                        Range_1 = Range_1,
                        Range_2 = Range_2,
                        Range_3 = Range_3,
                        Range_4 = Range_4,
                        Range_5 = Range_5,
                    });
                    return View(PcCon);
                }
            }
            conn.Close();
            return View();
        }

        [HttpPost]
        public ActionResult PCEnableEdit(PCControl postback)
        {
            string SqlEdit = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
                + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5 from Pc_Keno_Sample where SID = " + postback.SID;

                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlDataReader dr = sqlcmd.ExecuteReader(SqlSelect);

                /*
                conn.Open();
                SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlDataReader dr = scmd.ExecuteReader();
                */
                PCControl PcCon = new PCControl();
                while ((dr.Read()))
                {
                    if (!dr[0].Equals(DBNull.Value))
                    {
                        //Json
                        PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                        PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                        PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                        PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                        PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                        PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                        PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                        PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                        PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                        PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                        PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                        PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                        PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                        PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                        PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                        PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                        PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                        PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                        PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());

                        //塞入資料
                        //上中下
                        BallSmall.Enable = postback.BallSmall.Enable;
                        BallMid.Enable = postback.BallSmall.Enable;
                        BallBig.Enable = postback.BallSmall.Enable;

                        //奇和偶
                        BallOdd.Enable = postback.BallOdd.Enable;
                        BallSam.Enable = postback.BallOdd.Enable;
                        BallEven.Enable = postback.BallOdd.Enable;

                        //總和大小
                        SumBig.Enable = postback.SumBig.Enable;
                        SumSmall.Enable = postback.SumBig.Enable;

                        //總和單雙
                        SumOdd.Enable = postback.SumOdd.Enable;
                        SumEven.Enable = postback.SumOdd.Enable;

                        //大單小單四面
                        SumBigOdd.Enable = postback.SumBigOdd.Enable;
                        SumSmallOdd.Enable = postback.SumBigOdd.Enable;
                        SumBigEven.Enable = postback.SumBigOdd.Enable;
                        SumSmallEven.Enable = postback.SumBigOdd.Enable;
                        //五行
                        Range_1.Enable = postback.Range_1.Enable;
                        Range_2.Enable = postback.Range_1.Enable;
                        Range_3.Enable = postback.Range_1.Enable;
                        Range_4.Enable = postback.Range_1.Enable;
                        Range_5.Enable = postback.Range_1.Enable;

                        //組回Json
                        string ballsmall = JsonConvert.SerializeObject(BallSmall);
                        string ballmid = JsonConvert.SerializeObject(BallMid);
                        string ballbig = JsonConvert.SerializeObject(BallBig);
                        string ballodd = JsonConvert.SerializeObject(BallOdd);
                        string ballsam = JsonConvert.SerializeObject(BallSam);
                        string balleven = JsonConvert.SerializeObject(BallEven);
                        string sumbig = JsonConvert.SerializeObject(SumBig);
                        string sumsmall = JsonConvert.SerializeObject(SumSmall);
                        string sumodd = JsonConvert.SerializeObject(SumOdd);
                        string sumeven = JsonConvert.SerializeObject(SumEven);
                        string sumbigodd = JsonConvert.SerializeObject(SumBigOdd);
                        string sumsmallodd = JsonConvert.SerializeObject(SumSmallOdd);
                        string sumbigeven = JsonConvert.SerializeObject(SumBigEven);
                        string sumsmalleven = JsonConvert.SerializeObject(SumSmallEven);
                        string range_1 = JsonConvert.SerializeObject(Range_1);
                        string range_2 = JsonConvert.SerializeObject(Range_2);
                        string range_3 = JsonConvert.SerializeObject(Range_3);
                        string range_4 = JsonConvert.SerializeObject(Range_4);
                        string range_5 = JsonConvert.SerializeObject(Range_5);

                        SqlEdit = "UPDATE Pc_Keno_Sample SET "
                            + " BallSmall = N'" + ballsmall + "',"
                            + " BallMid = N'" + ballmid + "',"
                            + " BallBig = N'" + ballbig + "',"
                            + " BallOdd = N'" + ballodd + "',"
                            + " BallSam = N'" + ballsam + "',"
                            + " BallEven = N'" + balleven + "',"
                            + " SumBig = N'" + sumbig + "',"
                            + " SumSmall = N'" + sumsmall + "',"
                            + " SumOdd = N'" + sumodd + "',"
                            + " SumEven = N'" + sumeven + "',"
                            + " SumBigOdd= N'" + sumbigodd + "',"
                            + " SumSmallOdd= N'" + sumsmallodd + "',"
                            + " SumBigEven= N'" + sumbigeven + "',"
                            + " SumSmallEven= N'" + sumsmalleven + "',"
                            + " Range_1= N'" + range_1 + "',"
                            + " Range_2= N'" + range_2 + "',"
                            + " Range_3= N'" + range_3 + "',"
                            + " Range_4= N'" + range_4 + "',"
                            + " Range_5= N'" + range_5 + "',"
                            + " UpdateUser = N'Danny',"
                            + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "',"
                            + " CompanyID = 1"
                            + " WHERE SID = " + postback.SID;
                    }
                }
                dr.Close();
                /*
                using (SqlCommand SqlUpdate = new SqlCommand(SqlEdit, conn))
                {
                    SqlUpdate.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("PCIndex");
                }
                */
                SqlCmd sql = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sql.BeginTransaction();
                try
                {
                    sql.ExecuteNonQuery(SqlEdit);
                    sql.Commit();
                    return RedirectToAction("PCIndex");
                }
                catch
                {
                    sql.Rollback();
                    return View(postback);
                }


            }
            else
            {
                return View(postback);
            }
        }

        //PC Name
        public ActionResult PCNameEdit(int SID)
        {
            string SqlEdit = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
                + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5 from Pc_Keno_Sample where SID = " + SID;
            /*
            conn.Open();
            SqlCommand scmd = new SqlCommand(SqlEdit, conn);
            SqlDataReader dr = scmd.ExecuteReader();
            */
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(SqlEdit);

            PCControl PcCon = new PCControl();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PcCon = (new PCControl
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        BallSmall = BallSmall,
                        BallMid = BallMid,
                        BallBig = BallBig,
                        BallOdd = BallOdd,
                        BallSam = BallSam,
                        BallEven = BallEven,
                        SumBig = SumBig,
                        SumSmall = SumSmall,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        SumBigOdd = SumBigOdd,
                        SumSmallOdd = SumSmallOdd,
                        SumBigEven = SumBigEven,
                        SumSmallEven = SumSmallEven,
                        Range_1 = Range_1,
                        Range_2 = Range_2,
                        Range_3 = Range_3,
                        Range_4 = Range_4,
                        Range_5 = Range_5,
                    });
                    return View(PcCon);
                }
            }
            conn.Close();
            return View();
        }

        [HttpPost]
        public ActionResult PCNameEdit(PCControl postback)
        {
            string SqlEdit = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
                + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5 from Pc_Keno_Sample where SID = " + postback.SID;

                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlDataReader dr = sqlcmd.ExecuteReader(SqlSelect);

                /*
                conn.Open();
                SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlDataReader dr = scmd.ExecuteReader();
                */
                PCControl PcCon = new PCControl();
                while ((dr.Read()))
                {
                    if (!dr[0].Equals(DBNull.Value))
                    {
                        //Json
                        PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                        PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                        PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                        PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                        PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                        PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                        PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                        PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                        PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                        PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                        PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                        PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                        PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                        PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                        PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                        PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                        PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                        PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                        PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());

                        //塞入資料
                        //上中下
                        BallSmall.Name = postback.BallSmall.Name;

                        BallMid.Name = postback.BallMid.Name;

                        BallBig.Name = postback.BallBig.Name;

                        //奇和偶
                        BallOdd.Name = postback.BallOdd.Name;

                        BallSam.Name = postback.BallSam.Name;

                        BallEven.Name = postback.BallEven.Name;

                        //總和大小
                        SumBig.Name = postback.SumBig.Name;

                        SumSmall.Name = postback.SumSmall.Name;

                        //總和單雙
                        SumOdd.Name = postback.SumOdd.Name;

                        SumEven.Name = postback.SumEven.Name;

                        //大單小單四面
                        SumBigOdd.Name = postback.SumBigOdd.Name;

                        SumSmallOdd.Name = postback.SumSmallOdd.Name;

                        SumBigEven.Name = postback.SumBigEven.Name;

                        SumSmallEven.Name = postback.SumSmallEven.Name;

                        //五行..
                        Range_1.Name = postback.Range_1.Name;

                        Range_2.Name = postback.Range_2.Name;

                        Range_3.Name = postback.Range_3.Name;

                        Range_4.Name = postback.Range_4.Name;

                        Range_5.Name = postback.Range_5.Name;

                        //組回Json
                        string ballsmall = JsonConvert.SerializeObject(BallSmall);
                        string ballmid = JsonConvert.SerializeObject(BallMid);
                        string ballbig = JsonConvert.SerializeObject(BallBig);
                        string ballodd = JsonConvert.SerializeObject(BallOdd);
                        string ballsam = JsonConvert.SerializeObject(BallSam);
                        string balleven = JsonConvert.SerializeObject(BallEven);
                        string sumbig = JsonConvert.SerializeObject(SumBig);
                        string sumsmall = JsonConvert.SerializeObject(SumSmall);
                        string sumodd = JsonConvert.SerializeObject(SumOdd);
                        string sumeven = JsonConvert.SerializeObject(SumEven);
                        string sumbigodd = JsonConvert.SerializeObject(SumBigOdd);
                        string sumsmallodd = JsonConvert.SerializeObject(SumSmallOdd);
                        string sumbigeven = JsonConvert.SerializeObject(SumBigEven);
                        string sumsmalleven = JsonConvert.SerializeObject(SumSmallEven);
                        string range_1 = JsonConvert.SerializeObject(Range_1);
                        string range_2 = JsonConvert.SerializeObject(Range_2);
                        string range_3 = JsonConvert.SerializeObject(Range_3);
                        string range_4 = JsonConvert.SerializeObject(Range_4);
                        string range_5 = JsonConvert.SerializeObject(Range_5);

                        SqlEdit = "UPDATE Pc_Keno_Sample SET "
                            + " BallSmall = N'" + ballsmall + "',"
                            + " BallMid = N'" + ballmid + "',"
                            + " BallBig = N'" + ballbig + "',"
                            + " BallOdd = N'" + ballodd + "',"
                            + " BallSam = N'" + ballsam + "',"
                            + " BallEven = N'" + balleven + "',"
                            + " SumBig = N'" + sumbig + "',"
                            + " SumSmall = N'" + sumsmall + "',"
                            + " SumOdd = N'" + sumodd + "',"
                            + " SumEven = N'" + sumeven + "',"
                            + " SumBigOdd= N'" + sumbigodd + "',"
                            + " SumSmallOdd= N'" + sumsmallodd + "',"
                            + " SumBigEven= N'" + sumbigeven + "',"
                            + " SumSmallEven= N'" + sumsmalleven + "',"
                            + " Range_1= N'" + range_1 + "',"
                            + " Range_2= N'" + range_2 + "',"
                            + " Range_3= N'" + range_3 + "',"
                            + " Range_4= N'" + range_4 + "',"
                            + " Range_5= N'" + range_5 + "',"
                            + " UpdateUser = N'Danny',"
                            + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "',"
                            + " CompanyID = 1"
                            + " WHERE SID = " + postback.SID;
                    }
                }
                dr.Close();
                /*
                using (SqlCommand SqlUpdate = new SqlCommand(SqlEdit, conn))
                {
                    SqlUpdate.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("PCIndex");
                }*/
                SqlCmd sql = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sql.BeginTransaction();
                try
                {
                    sql.ExecuteNonQuery(SqlEdit);
                    sql.Commit();
                    return RedirectToAction("PCIndex");
                }
                catch
                {
                    sql.Rollback();
                    return View(postback);
                }
            }
            else
            {
                return View(postback);
            }
        }

        //PC Water
        public ActionResult PCWaterEdit(int SID)
        {
            string SqlEdit = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
                + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5 from Pc_Keno_Sample where SID = " + SID;
            /*
            conn.Open();
            SqlCommand scmd = new SqlCommand(SqlEdit, conn);
            SqlDataReader dr = scmd.ExecuteReader();
            */
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(SqlEdit);
            PCControl PcCon = new PCControl();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PcCon = (new PCControl
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        BallSmall = BallSmall,
                        BallMid = BallMid,
                        BallBig = BallBig,
                        BallOdd = BallOdd,
                        BallSam = BallSam,
                        BallEven = BallEven,
                        SumBig = SumBig,
                        SumSmall = SumSmall,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        SumBigOdd = SumBigOdd,
                        SumSmallOdd = SumSmallOdd,
                        SumBigEven = SumBigEven,
                        SumSmallEven = SumSmallEven,
                        Range_1 = Range_1,
                        Range_2 = Range_2,
                        Range_3 = Range_3,
                        Range_4 = Range_4,
                        Range_5 = Range_5,
                    });
                    return View(PcCon);
                }
            }
            conn.Close();
            return View();
        }

        [HttpPost]
        public ActionResult PCWaterEdit(PCControl postback)
        {
            string SqlEdit = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
                + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5 from Pc_Keno_Sample where SID = " + postback.SID;
                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlDataReader dr = sqlcmd.ExecuteReader(SqlSelect);

                /*
                conn.Open();
                SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlDataReader dr = scmd.ExecuteReader();
                */
                PCControl PcCon = new PCControl();
                while ((dr.Read()))
                {
                    if (!dr[0].Equals(DBNull.Value))
                    {
                        //Json
                        PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                        PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                        PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                        PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                        PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                        PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                        PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                        PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                        PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                        PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                        PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                        PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                        PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                        PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                        PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                        PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                        PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                        PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                        PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                        //塞入資料
                        //上中下
                        BallSmall.Odds = postback.BallSmall.Odds;
                        BallSmall.Water = postback.BallSmall.Water;
                        BallSmall.WaterBack = postback.BallSmall.WaterBack;

                        BallMid.Odds = postback.BallMid.Odds;
                        BallMid.Water = postback.BallMid.Water;
                        BallMid.WaterBack = postback.BallMid.WaterBack;

                        BallBig.Odds = postback.BallBig.Odds;
                        BallBig.Water = postback.BallBig.Water;
                        BallBig.WaterBack = postback.BallBig.WaterBack;
                        //奇和偶
                        BallOdd.Odds = postback.BallOdd.Odds;
                        BallOdd.Water = postback.BallOdd.Water;
                        BallOdd.WaterBack = postback.BallOdd.WaterBack;

                        BallSam.Odds = postback.BallSam.Odds;
                        BallSam.Water = postback.BallSam.Water;
                        BallSam.WaterBack = postback.BallSam.WaterBack;

                        BallEven.Odds = postback.BallEven.Odds;
                        BallEven.Water = postback.BallEven.Water;
                        BallEven.WaterBack = postback.BallEven.WaterBack;

                        //總和大小
                        SumBig.Odds = postback.SumBig.Odds;
                        SumBig.Water = postback.SumBig.Water;
                        SumBig.WaterBack = postback.SumBig.WaterBack;

                        SumSmall.Odds = postback.SumSmall.Odds;
                        SumSmall.Water = postback.SumSmall.Water;
                        SumSmall.WaterBack = postback.SumSmall.WaterBack;

                        //總和單雙
                        SumOdd.Odds = postback.SumOdd.Odds;
                        SumOdd.Water = postback.SumOdd.Water;
                        SumOdd.WaterBack = postback.SumOdd.WaterBack;

                        SumEven.Odds = postback.SumEven.Odds;
                        SumEven.Water = postback.SumEven.Water;
                        SumEven.WaterBack = postback.SumEven.WaterBack;

                        //大單小單四面
                        SumBigOdd.Odds = postback.SumBigOdd.Odds;
                        SumBigOdd.Water = postback.SumBigOdd.Water;
                        SumBigOdd.WaterBack = postback.SumBigOdd.WaterBack;

                        SumSmallOdd.Odds = postback.SumSmallOdd.Odds;
                        SumSmallOdd.Water = postback.SumSmallOdd.Water;
                        SumSmallOdd.WaterBack = postback.SumSmallOdd.WaterBack;

                        SumBigEven.Odds = postback.SumBigEven.Odds;
                        SumBigEven.Water = postback.SumBigEven.Water;
                        SumBigEven.WaterBack = postback.SumBigEven.WaterBack;

                        SumSmallEven.Odds = postback.SumSmallEven.Odds;
                        SumSmallEven.Water = postback.SumSmallEven.Water;
                        SumSmallEven.WaterBack = postback.SumSmallEven.WaterBack;

                        //五行..
                        Range_1.Odds = postback.Range_1.Odds;
                        Range_1.Water = postback.Range_1.Water;
                        Range_1.WaterBack = postback.Range_1.WaterBack;

                        Range_2.Odds = postback.Range_2.Odds;
                        Range_2.Water = postback.Range_2.Water;
                        Range_2.WaterBack = postback.Range_2.WaterBack;

                        Range_3.Odds = postback.Range_3.Odds;
                        Range_3.Water = postback.Range_3.Water;
                        Range_3.WaterBack = postback.Range_3.WaterBack;

                        Range_4.Odds = postback.Range_4.Odds;
                        Range_4.Water = postback.Range_4.Water;
                        Range_4.WaterBack = postback.Range_4.WaterBack;

                        Range_5.Odds = postback.Range_5.Odds;
                        Range_5.Water = postback.Range_5.Water;
                        Range_5.WaterBack = postback.Range_5.WaterBack;

                        //組回Json
                        string ballsmall = JsonConvert.SerializeObject(BallSmall);
                        string ballmid = JsonConvert.SerializeObject(BallMid);
                        string ballbig = JsonConvert.SerializeObject(BallBig);
                        string ballodd = JsonConvert.SerializeObject(BallOdd);
                        string ballsam = JsonConvert.SerializeObject(BallSam);
                        string balleven = JsonConvert.SerializeObject(BallEven);
                        string sumbig = JsonConvert.SerializeObject(SumBig);
                        string sumsmall = JsonConvert.SerializeObject(SumSmall);
                        string sumodd = JsonConvert.SerializeObject(SumOdd);
                        string sumeven = JsonConvert.SerializeObject(SumEven);
                        string sumbigodd = JsonConvert.SerializeObject(SumBigOdd);
                        string sumsmallodd = JsonConvert.SerializeObject(SumSmallOdd);
                        string sumbigeven = JsonConvert.SerializeObject(SumBigEven);
                        string sumsmalleven = JsonConvert.SerializeObject(SumSmallEven);
                        string range_1 = JsonConvert.SerializeObject(Range_1);
                        string range_2 = JsonConvert.SerializeObject(Range_2);
                        string range_3 = JsonConvert.SerializeObject(Range_3);
                        string range_4 = JsonConvert.SerializeObject(Range_4);
                        string range_5 = JsonConvert.SerializeObject(Range_5);

                        SqlEdit = "UPDATE Pc_Keno_Sample SET "
                            + " BallSmall = N'" + ballsmall + "',"
                            + " BallMid = N'" + ballmid + "',"
                            + " BallBig = N'" + ballbig + "',"
                            + " BallOdd = N'" + ballodd + "',"
                            + " BallSam = N'" + ballsam + "',"
                            + " BallEven = N'" + balleven + "',"
                            + " SumBig = N'" + sumbig + "',"
                            + " SumSmall = N'" + sumsmall + "',"
                            + " SumOdd = N'" + sumodd + "',"
                            + " SumEven = N'" + sumeven + "',"
                            + " SumBigOdd= N'" + sumbigodd + "',"
                            + " SumSmallOdd= N'" + sumsmallodd + "',"
                            + " SumBigEven= N'" + sumbigeven + "',"
                            + " SumSmallEven= N'" + sumsmalleven + "',"
                            + " Range_1= N'" + range_1 + "',"
                            + " Range_2= N'" + range_2 + "',"
                            + " Range_3= N'" + range_3 + "',"
                            + " Range_4= N'" + range_4 + "',"
                            + " Range_5= N'" + range_5 + "',"
                            + " UpdateUser = N'Danny',"
                            + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "',"
                            + " CompanyID = 1"
                            + " WHERE SID = " + postback.SID;
                    }
                }
                dr.Close();
                /*
                using (SqlCommand SqlUpdate = new SqlCommand(SqlEdit, conn))
                {
                    SqlUpdate.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("PCIndex");
                }
                */

                SqlCmd sql = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sql.BeginTransaction();
                try
                {
                    sql.ExecuteNonQuery(SqlEdit);
                    sql.Commit();
                    return RedirectToAction("PCIndex");
                }
                catch
                {
                    sql.Rollback();
                    return View(postback);
                }
            }
            else
            {
                return View(postback);
            }
        }
        //PC Detail
        public ActionResult PCDetailEdit(int SID)
        {
            string SqlEdit = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
    + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5 ,BallMaxMonney from Pc_Keno_Sample where SID = " + SID;
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(SqlEdit);

            /*
            conn.Open();
            SqlCommand scmd = new SqlCommand(SqlEdit, conn);
            SqlDataReader dr = scmd.ExecuteReader();
            */
            PCControl PcCon = new PCControl();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    //Json
                    PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                    PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                    PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                    PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                    PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                    PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                    PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                    PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                    PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                    PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                    PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                    PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                    PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                    PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                    PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                    PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                    PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                    PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                    PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                    PcCon = (new PCControl
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString(),
                        BallSmall = BallSmall,
                        BallMid = BallMid,
                        BallBig = BallBig,
                        BallOdd = BallOdd,
                        BallSam = BallSam,
                        BallEven = BallEven,
                        SumBig = SumBig,
                        SumSmall = SumSmall,
                        SumOdd = SumOdd,
                        SumEven = SumEven,
                        SumBigOdd = SumBigOdd,
                        SumSmallOdd = SumSmallOdd,
                        SumBigEven = SumBigEven,
                        SumSmallEven = SumSmallEven,
                        Range_1 = Range_1,
                        Range_2 = Range_2,
                        Range_3 = Range_3,
                        Range_4 = Range_4,
                        Range_5 = Range_5,
                        BallMaxMonney = (int)dr[21]
                    });
                    return View(PcCon);
                }
            }
            conn.Close();
            return View();
        }

        [HttpPost]
        public ActionResult PCDetailEdit(PCControl postback)
        {
            string SqlEdit = "";
            if (this.ModelState.IsValid)
            {
                string SqlSelect = "Select SID, Dish, BallSmall, BallMid, BallBig, BallOdd, BallSam, BallEven, SumBig, SumSmall, SumOdd, SumEven,"
                + " SumBigOdd, SumSmallOdd, SumBigEven, SumSmallEven, Range_1, Range_2, Range_3, Range_4, Range_5, BallMaxMonney from Pc_Keno_Sample where SID = " + postback.SID;

                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
                SqlDataReader dr = sqlcmd.ExecuteReader(SqlSelect);
                /*
                conn.Open();
                SqlCommand scmd = new SqlCommand(SqlSelect, conn);
                SqlDataReader dr = scmd.ExecuteReader();
                */
                PCControl PcCon = new PCControl();
                while ((dr.Read()))
                {
                    if (!dr[0].Equals(DBNull.Value))
                    {
                        //Json
                        PCDetail BallSmall = JsonConvert.DeserializeObject<PCDetail>(dr[2].ToString());
                        PCDetail BallMid = JsonConvert.DeserializeObject<PCDetail>(dr[3].ToString());
                        PCDetail BallBig = JsonConvert.DeserializeObject<PCDetail>(dr[4].ToString());
                        PCDetail BallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[5].ToString());
                        PCDetail BallSam = JsonConvert.DeserializeObject<PCDetail>(dr[6].ToString());
                        PCDetail BallEven = JsonConvert.DeserializeObject<PCDetail>(dr[7].ToString());
                        PCDetail SumBig = JsonConvert.DeserializeObject<PCDetail>(dr[8].ToString());
                        PCDetail SumSmall = JsonConvert.DeserializeObject<PCDetail>(dr[9].ToString());
                        PCDetail SumOdd = JsonConvert.DeserializeObject<PCDetail>(dr[10].ToString());
                        PCDetail SumEven = JsonConvert.DeserializeObject<PCDetail>(dr[11].ToString());
                        PCDetail SumBigOdd = JsonConvert.DeserializeObject<PCDetail>(dr[12].ToString());
                        PCDetail SumSmallOdd = JsonConvert.DeserializeObject<PCDetail>(dr[13].ToString());
                        PCDetail SumBigEven = JsonConvert.DeserializeObject<PCDetail>(dr[14].ToString());
                        PCDetail SumSmallEven = JsonConvert.DeserializeObject<PCDetail>(dr[15].ToString());
                        PCDetail Range_1 = JsonConvert.DeserializeObject<PCDetail>(dr[16].ToString());
                        PCDetail Range_2 = JsonConvert.DeserializeObject<PCDetail>(dr[17].ToString());
                        PCDetail Range_3 = JsonConvert.DeserializeObject<PCDetail>(dr[18].ToString());
                        PCDetail Range_4 = JsonConvert.DeserializeObject<PCDetail>(dr[19].ToString());
                        PCDetail Range_5 = JsonConvert.DeserializeObject<PCDetail>(dr[20].ToString());
                        //塞入資料
                        //上中下
                        BallSmall.BetMin = postback.BallSmall.BetMin;
                        BallSmall.BetMax = postback.BallSmall.BetMax;
                        BallSmall.SingelMax = postback.BallSmall.SingelMax;
                        BallSmall.DishMax = postback.BallSmall.DishMax;

                        BallMid.BetMin = postback.BallMid.BetMin;
                        BallMid.BetMax = postback.BallMid.BetMax;
                        BallMid.SingelMax = postback.BallMid.SingelMax;
                        BallMid.DishMax = postback.BallSmall.DishMax;

                        BallBig.BetMin = postback.BallBig.BetMin;
                        BallBig.BetMax = postback.BallBig.BetMax;
                        BallBig.SingelMax = postback.BallBig.SingelMax;
                        BallBig.DishMax = postback.BallSmall.DishMax;

                        //奇和偶
                        BallOdd.BetMin = postback.BallOdd.BetMin;
                        BallOdd.BetMax = postback.BallOdd.BetMax;
                        BallOdd.SingelMax = postback.BallOdd.SingelMax;
                        BallOdd.DishMax = postback.BallOdd.DishMax;

                        BallSam.BetMin = postback.BallSam.BetMin;
                        BallSam.BetMax = postback.BallSam.BetMax;
                        BallSam.SingelMax = postback.BallSam.SingelMax;
                        BallSam.DishMax = postback.BallOdd.DishMax;

                        BallEven.BetMin = postback.BallEven.BetMin;
                        BallEven.BetMax = postback.BallEven.BetMax;
                        BallEven.SingelMax = postback.BallEven.SingelMax;
                        BallEven.DishMax = postback.BallOdd.DishMax;
                        //總和大小
                        SumBig.BetMin = postback.SumBig.BetMin;
                        SumBig.BetMax = postback.SumBig.BetMax;
                        SumBig.SingelMax = postback.SumBig.SingelMax;
                        SumBig.DishMax = postback.SumBig.DishMax;

                        SumSmall.BetMin = postback.SumSmall.BetMin;
                        SumSmall.BetMax = postback.SumSmall.BetMax;
                        SumSmall.SingelMax = postback.SumSmall.SingelMax;
                        SumSmall.DishMax = postback.SumBig.DishMax;
                        //總和單雙
                        SumOdd.BetMin = postback.SumOdd.BetMin;
                        SumOdd.BetMax = postback.SumOdd.BetMax;
                        SumOdd.SingelMax = postback.SumOdd.SingelMax;
                        SumOdd.DishMax = postback.SumOdd.DishMax;

                        SumEven.BetMin = postback.SumEven.BetMin;
                        SumEven.BetMax = postback.SumEven.BetMax;
                        SumEven.SingelMax = postback.SumEven.SingelMax;
                        SumEven.DishMax = postback.SumOdd.DishMax;
                        //大單小單四面
                        SumBigOdd.BetMin = postback.SumBigOdd.BetMin;
                        SumBigOdd.BetMax = postback.SumBigOdd.BetMax;
                        SumBigOdd.SingelMax = postback.SumBigOdd.SingelMax;
                        SumBigOdd.DishMax = postback.SumBigOdd.DishMax;

                        SumSmallOdd.BetMin = postback.SumSmallOdd.BetMin;
                        SumSmallOdd.BetMax = postback.SumSmallOdd.BetMax;
                        SumSmallOdd.SingelMax = postback.SumSmallOdd.SingelMax;
                        SumSmallOdd.DishMax = postback.SumBigOdd.DishMax;

                        SumBigEven.BetMin = postback.SumBigEven.BetMin;
                        SumBigEven.BetMax = postback.SumBigEven.BetMax;
                        SumBigEven.SingelMax = postback.SumBigEven.SingelMax;
                        SumBigEven.DishMax = postback.SumBigOdd.DishMax;

                        SumSmallEven.BetMin = postback.SumSmallEven.BetMin;
                        SumSmallEven.BetMax = postback.SumSmallEven.BetMax;
                        SumSmallEven.SingelMax = postback.SumSmallEven.SingelMax;
                        SumSmallEven.DishMax = postback.SumBigOdd.DishMax;
                        //五行..
                        Range_1.BetMin = postback.Range_1.BetMin;
                        Range_1.BetMax = postback.Range_1.BetMax;
                        Range_1.SingelMax = postback.Range_1.SingelMax;
                        Range_1.DishMax = postback.Range_1.DishMax;

                        Range_2.BetMin = postback.Range_2.BetMin;
                        Range_2.BetMax = postback.Range_2.BetMax;
                        Range_2.SingelMax = postback.Range_2.SingelMax;
                        Range_2.DishMax = postback.Range_1.DishMax;

                        Range_3.BetMin = postback.Range_3.BetMin;
                        Range_3.BetMax = postback.Range_3.BetMax;
                        Range_3.SingelMax = postback.Range_3.SingelMax;
                        Range_3.DishMax = postback.Range_1.DishMax;

                        Range_4.BetMin = postback.Range_4.BetMin;
                        Range_4.BetMax = postback.Range_4.BetMax;
                        Range_4.SingelMax = postback.Range_4.SingelMax;
                        Range_4.DishMax = postback.Range_1.DishMax;

                        Range_5.BetMin = postback.Range_5.BetMin;
                        Range_5.BetMax = postback.Range_5.BetMax;
                        Range_5.SingelMax = postback.Range_5.SingelMax;
                        Range_5.DishMax = postback.Range_1.DishMax;

                        //組回Json
                        string ballsmall = JsonConvert.SerializeObject(BallSmall);
                        string ballmid = JsonConvert.SerializeObject(BallMid);
                        string ballbig = JsonConvert.SerializeObject(BallBig);
                        string ballodd = JsonConvert.SerializeObject(BallOdd);
                        string ballsam = JsonConvert.SerializeObject(BallSam);
                        string balleven = JsonConvert.SerializeObject(BallEven);
                        string sumbig = JsonConvert.SerializeObject(SumBig);
                        string sumsmall = JsonConvert.SerializeObject(SumSmall);
                        string sumodd = JsonConvert.SerializeObject(SumOdd);
                        string sumeven = JsonConvert.SerializeObject(SumEven);
                        string sumbigodd = JsonConvert.SerializeObject(SumBigOdd);
                        string sumsmallodd = JsonConvert.SerializeObject(SumSmallOdd);
                        string sumbigeven = JsonConvert.SerializeObject(SumBigEven);
                        string sumsmalleven = JsonConvert.SerializeObject(SumSmallEven);
                        string range_1 = JsonConvert.SerializeObject(Range_1);
                        string range_2 = JsonConvert.SerializeObject(Range_2);
                        string range_3 = JsonConvert.SerializeObject(Range_3);
                        string range_4 = JsonConvert.SerializeObject(Range_4);
                        string range_5 = JsonConvert.SerializeObject(Range_5);

                        SqlEdit = "UPDATE Pc_Keno_Sample SET "
                            + " BallSmall = N'" + ballsmall + "',"
                            + " BallMid = N'" + ballmid + "',"
                            + " BallBig = N'" + ballbig + "',"
                            + " BallOdd = N'" + ballodd + "',"
                            + " BallSam = N'" + ballsam + "',"
                            + " BallEven = N'" + balleven + "',"
                            + " SumBig = N'" + sumbig + "',"
                            + " SumSmall = N'" + sumsmall + "',"
                            + " SumOdd = N'" + sumodd + "',"
                            + " SumEven = N'" + sumeven + "',"
                            + " SumBigOdd= N'" + sumbigodd + "',"
                            + " SumSmallOdd= N'" + sumsmallodd + "',"
                            + " SumBigEven= N'" + sumbigeven + "',"
                            + " SumSmallEven= N'" + sumsmalleven + "',"
                            + " Range_1= N'" + range_1 + "',"
                            + " Range_2= N'" + range_2 + "',"
                            + " Range_3= N'" + range_3 + "',"
                            + " Range_4= N'" + range_4 + "',"
                            + " Range_5= N'" + range_5 + "',"
                            + " BallMaxMonney= " + postback.BallMaxMonney + ","
                            + " UpdateUser = N'Danny',"
                            + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "',"
                            + " CompanyID = 1"
                            + " WHERE SID = " + postback.SID;
                    }
                }
                dr.Close();
                /*
                using (SqlCommand SqlUpdate = new SqlCommand(SqlEdit, conn))
                {
                    SqlUpdate.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("PCIndex");
                }
                */
                SqlCmd sql = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sql.BeginTransaction();
                try
                {
                    sql.ExecuteNonQuery(SqlEdit);
                    sql.Commit();
                    return RedirectToAction("PCIndex");
                }
                catch
                {
                    sql.Rollback();
                    return View(postback);
                }

            }
            else
            {
                return View(postback);
            }
        }



        //Delete PCDelete
        public ActionResult PCDelete(int SID)
        {
            string sql = "SELECT SID, Dish FROM Pc_Keno_Sample Where SID=" + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(sql);
            /*
            SqlCommand scmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = scmd.ExecuteReader();
            */
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    Pc_Keno_Sample results = new Pc_Keno_Sample()
                    {
                        SID = (int)dr[0],
                        Dish = dr[1].ToString()
                    };
                    return View(results);
                }
            }
            conn.Close();
            return View();
        }
        [HttpPost]
        public ActionResult PCDelete(Pc_Keno_Sample postback)
        {
            string sql = "DELETE FROM Pc_Keno_Sample Where SID = @SID";
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
            sqlcmd.Parameters.AddWithValue("@SID", postback.SID);
            sqlcmd.BeginTransaction();
            try
            {
                sqlcmd.ExecuteNonQuery(sql);
                sqlcmd.Commit();
                conn.Close();
                return RedirectToAction("GameCloseIndex");
            }
            catch
            {
                sqlcmd.Rollback();
                return View(postback);
            }
            /*
            conn.Open();
            SqlCommand sqlcmd = new SqlCommand(sql, conn);
            sqlcmd.Parameters.AddWithValue("@SID", postback.SID);
            sqlcmd.ExecuteNonQuery();
            conn.Close();*/
        }


        /// <summary>
        /// 球種開關
        /// </summary>
        /// <returns></returns>
        public ActionResult EnableIndex()
        {
            var viewModel = new GameEnableViewModel();
            string sql = "SELECT SID, Game, GameType, Status, UpdateTime, UpdateUser FROM GameEnable Where Level = 1";

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);

            viewModel.GameEnables = new List<GameEnable>();
            foreach (SqlDataReader dr in sqlcmd.ExecuteReader("SELECT SID, Game, GameType, Status, UpdateTime, UpdateUser FROM GameEnable Where Level = 1"))
                viewModel.GameEnables.Add(dr.ToObject<GameEnable>());
            return View(viewModel);

            /*
            SqlCommand scmd = new SqlCommand(sql, conn);
            conn.Open();
            using (SqlDataReader dr = scmd.ExecuteReader())
            {
                viewModel.GameEnables = new List<GameEnable>() { };
                while (dr.Read())
                {
                    GameEnable Item = new GameEnable();
                    Item.SID = dr.GetInt32(0);
                    Item.Game = dr.GetString(1);
                    Item.GameType = dr.GetString(2);
                    Item.Status = dr.GetInt32(3);
                    Item.UpdateTime = dr.GetDateTime(4);
                    Item.UpdateUser = dr.GetString(5);
                    viewModel.GameEnables.Add(Item);
                }
                return View(viewModel);
            }*/

        }

        [HttpPost]
        public ActionResult EnableIndex(GameEnableViewModel postback)
        {
            if (this.ModelState.IsValid)
            {
                for (int i = 0; i < postback.GameEnables.Count; i++)
                {
                    string SqlEdit = "UPDATE GameEnable SET "
                   + " Status = " + postback.GameEnables[i].Status + ","
                   + " UpdateUser = N'Danny',"
                   + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "'"
                   + " WHERE SID = " + postback.GameEnables[i].SID;
                    SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                    sqlcmd.BeginTransaction();
                    try
                    {
                        sqlcmd.ExecuteNonQuery(SqlEdit);
                        sqlcmd.Commit();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                    }
                    return RedirectToAction("EnableIndex");
                }
                return View(postback);
            }
            else
            {
                return View(postback);
            }
        }

        public ActionResult EnableEdit(string GameType)
        {
            var viewModel = new GameEnableViewModel();
            string sql = "SELECT SID, Game, Status, UpdateTime, UpdateUser FROM GameEnable where Level != 1 and GameType =  @GameType";

            /* string sql = "SELECT SID, Game, GameType, Status, UpdateTime, UpdateUser FROM GameEnable"
                 + " where Level != 1 and GameType = N'" + GameType + "'";
             SqlCommand scmd = new SqlCommand(
                 "SELECT SID, Game, Status, UpdateTime, UpdateUser FROM GameEnable where Level != 1 and GameType =  @GameType", conn);          
             conn.Open();*/
            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            sqlcmd.Parameters.AddWithValue("@GameType", GameType);
            SqlDataReader dr = sqlcmd.ExecuteReader(sql);
            viewModel.GameEnables = new List<GameEnable>() { };
            while (dr.Read())
            {
                GameEnable Item = new GameEnable();
                Item.SID = dr.GetInt32(0);
                Item.Game = dr.GetString(1);
                Item.Status = dr.GetInt32(2);
                Item.UpdateTime = dr.GetDateTime(3);
                Item.UpdateUser = dr.GetString(4);
                viewModel.GameEnables.Add(Item);
            }
            return View(viewModel);

        }

        [HttpPost]
        public ActionResult EnableEdit(GameEnableViewModel postback)
        {
            if (this.ModelState.IsValid)
            {

                for (int i = 0; i < postback.GameEnables.Count; i++)
                {
                    string SqlEdit = "UPDATE GameEnable SET "
                   + " Status = " + postback.GameEnables[i].Status + ","
                   + " UpdateUser = N'Danny',"
                   + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "'"
                   + " WHERE SID = " + postback.GameEnables[i].SID;
                    SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                    sqlcmd.BeginTransaction();
                    try
                    {
                        sqlcmd.ExecuteNonQuery(SqlEdit);
                        sqlcmd.Commit();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                    }

                }
                return RedirectToAction("EnableIndex");

                /*
                conn.Open();
                for (int i = 0; i < postback.GameEnables.Count; i++)
                {
                    string SqlEdit = "UPDATE GameEnable SET "
                   + " Status = " + postback.GameEnables[i].Status + ","
                   + " UpdateUser = N'Danny',"
                   + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "'"
                   + " WHERE SID = " + postback.GameEnables[i].SID;

                    using (SqlCommand SqlUpdate = new SqlCommand(SqlEdit, conn))
                    {
                        SqlUpdate.ExecuteNonQuery();
                    }
                }
                conn.Close();
                return RedirectToAction("EnableIndex");
                */
            }
            else
            {
                return View(postback);
            }
        }

        /// <summary>
        /// DST Control
        /// </summary>
        /// <returns></returns>
        public ActionResult DSTIndex()
        {

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            List<DST> results = new List<DST>();
            foreach (SqlDataReader dr in sqlcmd.ExecuteReader("SELECT SID, Game, DSTBegin, DSTEnd, UpdateTime, UpdateUser FROM DST"))
                results.Add(dr.ToObject<DST>());
            return View(results);

            /*
            string sql = "SELECT SID, Game, DSTBegin, DSTEnd, UpdateTime, UpdateUser FROM DST";
            SqlCommand scmd = new SqlCommand(sql, conn);
            using (conn)
            {
                conn.Open();
                using (SqlDataReader dr = scmd.ExecuteReader())
                {
                    List<DST> results = new List<DST>();

                    while (dr.Read())
                    {
                        DST Item = new DST();

                        Item.SID = dr.GetInt32(0);
                        Item.Game = dr.GetString(1);
                        Item.DSTBegin = dr.GetDateTime(2);
                        Item.DSTEnd = dr.GetDateTime(3);
                        Item.UpdateTime = dr.GetDateTime(4);
                        Item.UpdateUser = dr.GetString(5);
                        results.Add(Item);
                    }
                    return View(results);
                }
            }
            */
        }

        public ActionResult DSTEdit(int SID)
        {

            string SqlEdit = "SELECT SID, Game, DSTBegin, DSTEnd  FROM DST where SID = " + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(SqlEdit);
            /*
            conn.Open();
            SqlCommand scmd = new SqlCommand(SqlEdit, conn);
            SqlDataReader dr = scmd.ExecuteReader();
            */
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    DST results = new DST()
                    {
                        SID = (int)dr[0],
                        Game = dr[1].ToString(),
                        DSTBegin = (DateTime)dr[2],
                        DSTEnd = (DateTime)dr[3]
                    };
                    conn.Close();
                    return View(results);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult DSTEdit(DST postback)
        {
            if (this.ModelState.IsValid)
            {
                string SqlEdit = "UPDATE DST SET "
               + " DSTBegin = @DSTBegin,"
               + " DSTEnd = @DSTEnd,"
               + " UpdateUser = N'Danny',"
               + " UpdateTime = N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + "'"
               + " WHERE SID = " + postback.SID;

                SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
                sqlcmd.Parameters.AddWithValue("@DSTBegin", postback.DSTBegin);
                sqlcmd.Parameters.AddWithValue("@DSTEnd", postback.DSTEnd);
                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(SqlEdit);
                    sqlcmd.Commit();
                    return RedirectToAction("DSTIndex");
                }
                catch
                {
                    sqlcmd.Rollback();
                    return View(postback);
                }
                /*
                conn.Open();
                SqlCommand SqlUpdate = new SqlCommand(SqlEdit, conn);
                SqlUpdate.Parameters.AddWithValue("@DSTBegin", postback.DSTBegin);
                SqlUpdate.Parameters.AddWithValue("@DSTEnd", postback.DSTEnd);
                SqlUpdate.ExecuteNonQuery();
                conn.Close();
                */
            }
            else
            {
                return View(postback);
            }
        }


        public ActionResult DSTDelete(int SID)
        {
            string sql = "SELECT SID, Game FROM DST Where SID=" + SID;

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01R);
            SqlDataReader dr = sqlcmd.ExecuteReader(sql);
            /*
            SqlCommand scmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = scmd.ExecuteReader();
            */
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    DST results = new DST()
                    {
                        SID = (int)dr[0],
                        Game = dr[1].ToString()
                    };
                    return View(results);
                }
            }
            conn.Close();
            return View();
        }
        [HttpPost]
        public ActionResult DSTDelete(DST postback)
        {
            string sql = "DELETE FROM DST Where SID= @SID";

            SqlCmd sqlcmd = _HttpContext.GetSqlCmd(DB.LTDB01W);
            sqlcmd.BeginTransaction();
            try
            {
                sqlcmd.ExecuteNonQuery(sql);
                sqlcmd.Commit();
                return RedirectToAction("DSTIndex");
            }
            catch
            {
                sqlcmd.Rollback();
                return View(postback);
            }
            /*
            SqlCommand scmd = new SqlCommand(sql, conn);
            scmd.Parameters.AddWithValue("@SID", postback.SID);
            conn.Open();
            SqlDataReader dr = scmd.ExecuteReader();
            while ((dr.Read()))
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    DST results = new DST()
                    {
                        SID = (int)dr[0],
                        Game = dr[1].ToString()
                    };
                    return View(results);
                }
            }
            conn.Close();
            return View();
            */
        }
    }
}