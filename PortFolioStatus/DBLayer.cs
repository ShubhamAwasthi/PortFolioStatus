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
                if (db.InsertAsync(data).Result != 0)
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

        private static void Seed()
        {
            var s1 = new Stock { Name = "Reliance Industries", UnitCost = 12.09M, Ticker = "RELIANCE", PurchaseDate = DateTime.Now, Qty = 20, Short = false, Exchange = "NSE" };
            var s2 = new Stock { Name = "EPC Industries", UnitCost = 11.09M, Ticker = "523754", PurchaseDate = DateTime.Now, Qty = 10, Short = false, Exchange = "BOM" };
            InsertUpdateData(s1, GetPath());
            InsertUpdateData(s2, GetPath());
        }
    }
}