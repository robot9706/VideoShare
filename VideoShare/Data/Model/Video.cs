using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class Video
    {
        public const string Table = "Video";

        [SQLColumn(0, "ID", true, "VIDEO_ID_SEQ")]
        public int ID;

        [SQLColumn(1, "Title")]
        public string Title;

        [SQLColumn(2, "Description")]
        public string Description;

        [SQLColumn(3, "Length")]
        public int Length;

        [SQLColumn(4, "Uploader")]
        public int Uploader;

        [SQLColumn(5, "UploadTime")]
        public DateTime UploadTime;

        [SQLColumn(6, "Likes")]
        public int Likes;

        [SQLColumn(7, "Dislikes")]
        public int Dislikes;
    }
}