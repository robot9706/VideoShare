using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class Playlist
    {
        public const string Table = "PLAYLIST";

        #region Structure
        [SQLColumn(0, "ID", true, "PLAYLIST_ID_SEQ")]
        public int ID;

        [SQLColumn(1, "CREATOR")]
        public int Creator;

        [SQLColumn(2, "CREATIONDATE")]
        public DateTime CreationDate;

        [SQLColumn(3, "TITLE")]
        public string Title;
        #endregion

        #region Functions
        private const string SQL_GetVideos = "select * from \"VIDEO\", \"PLAYLISTCONTENT\" " + 
											 "where \"VIDEO\".ID=\"PLAYLISTCONTENT\".VideoID and \"PLAYLISTCONTENT\".PlaylistID=:plid " + 
											 "order by \"VIDEO\".UploadTime desc";

        public List<Video> GetVideos()
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_GetVideos))
            {
                command.Parameters.Add("plid", ID);

                return Global.Database.Select<Video>(command);
            }
        }

        private const string SQL_FindList = "select * from \"PLAYLIST\" where ID=:plid";

        public static Playlist Find(int id)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_FindList))
            {
                command.Parameters.Add("plid", id);

                List<Playlist> list = Global.Database.Select<Playlist>(command);

                if (list.Count == 1)
                    return list[0];

                return null;
            }
        }

        private const string SQL_DeleteContent = "delete from \"PLAYLISTCONTENT\" where PlaylistID=:plid";

        public void Delete()
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_DeleteContent))
            {
                command.Parameters.Add("plid", ID);

                command.ExecuteNonQuery();
            }

            Global.Database.Delete<Playlist>(this);
        }

        private const string SQL_DeleteContentWithID = "delete from \"PLAYLISTCONTENT\" where PlaylistID=:plid and VideoID=:vlid";

        public void DeleteFromList(Video video)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_DeleteContentWithID))
            {
                command.Parameters.Add("plid", ID);
                command.Parameters.Add("vlid", video.ID);

                command.ExecuteNonQuery();
            }
        }
        #endregion
    }
}