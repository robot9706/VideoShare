using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class Video
    {
        [SQLColumn(0, "ID", true)]
        public int ID { get; set; }

        [SQLColumn(1, "Title")]
        public string Title { get; set; }

        [SQLColumn(2, "Description")]
        public string Description { get; set; }

        [SQLColumn(3, "Length")]
        public int Length { get; set; }

        [SQLColumn(4, "Uploader")]
        public int Uploader { get; set; }

        [SQLColumn(5, "UploadTime")]
        public DateTime UploadTime { get; set; }

        [SQLColumn(6, "Likes")]
        public int Likes { get; set; }

        [SQLColumn(7, "Dislikes")]
        public int Dislikes { get; set; }
    }
}