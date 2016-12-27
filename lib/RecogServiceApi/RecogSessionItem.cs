using ams;
using System;
using System.Data;

namespace RecogService
{
    [TableName("RecogSessionItem")]
    public class RecogSessionItem
    {
        [DbImport]
        public Guid ID1;
        [DbImport]
        public Guid ID2;
        [DbImport]
        public UserID UserID1;
        [DbImport]
        public UserID UserID2;
        [DbImport]
        public float Similarity;
        [DbImport]
        public DateTime CompareTime;
    }
}
