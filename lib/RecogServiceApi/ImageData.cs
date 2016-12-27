using ams;
using ams.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace RecogService
{
    public class ImageData
    {
        [DbImport]
        public Guid? ID;
        [DbImport]
        public UserID? CorpID;
        [DbImport]
        public UserID? MemberID;
        [DbImport]
        public ImageType? ImageType;
        [DbImport]
        public string TakePictureKey;
        [DbImport]
        public DateTime? CreateTime;
        [DbImport]
        public byte[] data;
        [DbImport("Template")]
        public byte[] template;
        [DbImport]
        public bool? Success;

        public string token;

        public MemberData Member
        {
            set
            {
                if (value == null)
                {
                    CorpID = null;
                    MemberID = null;
                }
                else
                {
                    CorpID = value.CorpID;
                    MemberID = value.ID;
                }
            }
        }

        internal bool build_template;
        public bool Save(SqlCmd imageDB)
        {
            if (data == null) return false;
            if (!build_template)
                this.ID = null;
            using (_HttpContext.GetSqlCmd(out imageDB, imageDB, MainPlatformInfo.Instance.PhotoDB))
            {
                imageDB.Parameters.Clear();
                SqlBuilder sql1 = new SqlBuilder();
                sql1["", "CorpID        "] = CorpID ?? 0;
                sql1["", "MemberID      "] = MemberID ?? 0;
                sql1["", "ImageType     "] = ImageType?.ToString();
                sql1["", "TakePictureKey"] = TakePictureKey;
                sql1["", "Success       "] = (Success ?? true) ? 1 : 0;
                if (build_template)
                {
                    sql1["", "id        "] = ID;
                    sql1["", "data      "] = SqlBuilder.str.Null;
                }
                else
                {
                    imageDB.Parameters.Add("@data", SqlDbType.Image).Value = data;
                    sql1["", "data      "] = (SqlBuilder.str)"@data";
                }
                if (template != null)
                {
                    imageDB.Parameters.Add("@template", SqlDbType.Image).Value = template;
                    sql1["", "template  "] = (SqlBuilder.str)"@template";
                }
                string sql2 = sql1.Build("exec sp_SaveImage ", SqlBuilder.op.AtFieldValue);
                foreach (Action commit in imageDB.BeginTran())
                {
                    imageDB.FillObject(this, sql2);
                    if (this.ID.HasValue)
                        commit();
                }
            }
            return this.ID.HasValue;
        }



        public static Guid? GetImageID(SqlCmd imageDB, UserID? memberID, ImageType imageType, int index)
        {
            ImageData image;
            Load(imageDB, memberID, imageType, index, out image, false, false);
            return image?.ID;
        }

        public static bool Load(SqlCmd imageDB, Guid image_id, out ImageData image, bool data = true, bool template = false)
        {
            return Load(imageDB, $"exec sp_GetImageByID '{image_id}'", out image);
        }

        public static bool Load(SqlCmd imageDB, UserID? memberID, ImageType imageType, int index, out ImageData image, bool data = true, bool template = false)
        {
            if (memberID.HasValue) return Load(imageDB, $"exec sp_GetImage @MemberID={memberID}, @ImageType='{imageType}', @Index={index}, @data1={(data ? 1 : 0)}, @data2={(template ? 1 : 0)}", out image);
            return _null.noop(false, out image);
        }

        static bool Load(SqlCmd imageDB, string sql, out ImageData image)
        {
            image = null;
            using (_HttpContext.GetSqlCmd(out imageDB, imageDB, MainPlatformInfo.Instance.PhotoDB))
            {
                foreach (SqlDataReader r in imageDB.ExecuteReaderEach(sql))
                {
                    if (image == null)
                        image = new ImageData();
                    r.FillObject(image);
                }
            }
            return image != null;
        }
    }
}