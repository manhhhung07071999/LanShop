using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public static class RemovableDB
    {
        const string DefaultName = "123xYz79";

        static BsonData.DataBase _db;
        static public BsonData.DataBase DataBase => _db;
        static public BsonData.Collection GetCollection<T>()
        {
            return _db.GetCollection<T>();
        }

        public static List<string> GetDrivers()
        {
            var lst = new List<string>();
            foreach (var dir in DriveInfo.GetDrives())
            {
                if (dir.IsReady && dir.DriveType == DriveType.Removable)
                {
                    lst.Add(dir.Name);
                }
            }
            return lst;
        }

        public static bool HasDb(string dir)
        {
            var path = dir + "/" + DefaultName;
            var info = new DirectoryInfo(path);

            return info.Exists;
        }
        public static string GetDbDrive(List<string> drives)
        {
            foreach (var dir in drives)
            {
                if (HasDb(dir))
                {
                    return dir;
                }
            }

            return null;
        }

        public static void Create(string dir)
        {
            _db = new BsonData.DataBase(dir, DefaultName);
        }

        public static string[] GetAllFile()
        {
            return Directory.GetFiles(_db.PhysicalPath, "*.*", SearchOption.AllDirectories);
        }
    }

    public class CloneModel
    {
        public BsonData.DataBase Database { get; set; }
        public string[] Source { get; set; }
        public string Destination { get; set; }
    }
}
