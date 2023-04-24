using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Gemini.Modules.Utils
{
    public class LiteDbUtil
    {
        public static bool Save<T>(T obj, LiteDatabase db)
            where T : new()
        {
            var col = db.GetCollection<T>();
            var value = col.Insert(obj);
            return value.AsInt32 > 0;
        }

        public static bool SaveBulk<T>(IEnumerable<T> objs, LiteDatabase db)
            where T : new()
        {
            var col = db.GetCollection<T>();
            var value = col.InsertBulk(objs);
            return value > 0;
        }
        public static bool Update<T>(T obj, LiteDatabase db)
            where T : new()
        {
            var col = db.GetCollection<T>();
            var success = col.Update(obj);
            return success;
        }


        public static bool Delete<T>(int docId, LiteDatabase db)
            where T : new()
        {
            var col = db.GetCollection<T>();
            var success = col.Delete(docId);
            return success;
        }

        public static bool DeleteFile(string fileId, LiteDatabase db)
        {
            if (!string.IsNullOrWhiteSpace(fileId) && db.FileStorage.Exists(fileId))
                return db.FileStorage.Delete(fileId);
            return false;
        }


        public static Stream OpenRead(string fileId, LiteDatabase db)
        {
            if (db.FileStorage.Exists(fileId))
            {
                return db.FileStorage.OpenRead(fileId);
            }
            return null;
        }
        public static Stream OpenWrite(string fileId, LiteDatabase db)
        {
            return db.FileStorage.OpenWrite(fileId, fileId);
        }
        public static void SaveFile(string fileId, byte[] bytes, LiteDatabase db)
        {
            var stream = db.FileStorage.OpenWrite(fileId, fileId);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();

        }
        public static bool DeleteBulk<T>(IEnumerable<int> objs, LiteDatabase db)
            where T : new()
        {
            var col = db.GetCollection<T>();
            foreach (var id in objs)
            {
                col.Delete(id);
            }

            return true;
        }

        public static T FindById<T>(int docId, LiteDatabase db)
            where T : new()
        {
            var col = db.GetCollection<T>();
            var doc = col.FindById(docId);
            return doc;
        }
    }
}
