using Android.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace PortFolioStatus
{
    public class DBLayer
    {
        private static readonly string _dbFile = "stocks.db";

        private static string GetPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), _dbFile);
        }

        public static bool InitDB()
        {
            var response =  CreateDB(GetPath());
            if (response)
                Seed();
            return response;
        }

        public static bool InsertUpdate(Stock data)
        {
            return InsertUpdateData(data, GetPath());
        }

        public static bool Delete(Stock data) {
            return DeleteData(data, GetPath());
        }

        public static bool GetRecords(ref List<Stock> records)
        {
            var list = FindRecords(GetPath());
            if (list != null)
            {
                records = list;
                return true;
            }
            records = null;
            return false;
        }

        public static Stock GetRecordByID(int id) {
            Stock response = null;

            var list = FindRecords(GetPath());
            if (list != null)
            {
                response = list.Find(x => x.ID == id);
            }

            return response;
        }

        private static bool CreateDB(string path)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                {
                    connection.CreateTableAsync<Stock>();
                    Log.Debug("CreateDB", "Created DB");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Error("CreateDB", "Unable to create DB");
            }
            return false;
        }

        private static bool InsertUpdateData(Stock data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                var result = 0;
                if (data.ID == 0)
                    result = db.InsertAsync(data).Result;
                else
                    result = db.UpdateAsync(data).Result;
                if (result != 1)
                    throw new ApplicationException("Unable to Update DB for: " + data.ToString());
                Log.Debug("InsertUpdateData", "Single data file inserted or updated");
                return true;
            }
            catch (SQLiteException ex)
            {
                Log.Error("InsertUpdateData", ex.Message);
            }
            return false;
        }

        private static bool DeleteData(Stock data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                var result = 0;
                result = db.DeleteAsync(data).Result;
                Log.Debug("DeleteData", "Single data file deleted");
                return true;
            }
            catch (SQLiteException ex)
            {
                Log.Error("DeleteData", ex.Message);
            }
            return false;
        }

        private static List<Stock> FindRecords(string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                
                var list = db.QueryAsync<Stock>("SELECT * FROM Stock").Result;

                Log.Debug("FindRecords", "Found " + list.Count + " Records");
                return list;
            }
            catch (SQLiteException ex)
            {
                Log.Error("FindRrecords", "Error from DB: " + ex.Message);
                return null;
            }
        }

        public static void Seed()
        {
            var s1 = new Stock { Name = "Reliance Industries", UnitCost = 12.09M, Ticker = "RELIANCE", PurchaseDate = DateTime.Now, Qty = 20, Short = false, Exchange = "NSE" };
            var s2 = new Stock { Name = "EPC Industries", UnitCost = 11.09M, Ticker = "523754", PurchaseDate = DateTime.Now, Qty = 10, Short = true, Exchange = "BOM" };
            InsertUpdateData(s1, GetPath());
            InsertUpdateData(s2, GetPath());
        }
        public static bool Flush()
        {
            try
            {
                var connection = new SQLiteAsyncConnection(GetPath());
                {
                    var pass = connection.DropTableAsync<Stock>().Result;
                    var create = CreateDB(GetPath());
                    Log.Debug("DeletedDB", "Deleted DB");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Error("CreateDB", "Unable to Flush DB");
            }
            return false;
        }
    }
}