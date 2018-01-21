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
            var response = CreateDB(GetPath());
            if (response)
                Seed();
            return response;
        }

        public static bool InsertUpdate(Stock data)
        {
            return InsertUpdateData(data, GetPath());
        }

        public static bool Delete(Stock data)
        {
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

        public static Stock GetRecordByID(int id)
        {
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
            catch (Exception ex)
            {
                Log.Error("FindRrecords", "Error from DB: " + ex.Message);
                return null;
            }
        }

        public static void Seed()
        {
            var s = new List<Stock>()
            { new Stock { Name = "Atlas Cycles", UnitCost = 242.50M, Ticker = "ATLASCYCLE", PurchaseDate = DateTime.Now, Qty = 200, Short = false, Exchange = "NSE" },
new Stock { Name = "Coal India", UnitCost = 278.65M, Ticker = "COALINDIA", PurchaseDate = DateTime.Now, Qty = 150, Short = false, Exchange = "NSE" },
new Stock { Name = "Greaves Cotton", UnitCost = 163.00M, Ticker = "GREAVESCOT", PurchaseDate = DateTime.Now, Qty = 150, Short = false, Exchange = "NSE" },
new Stock { Name = "JMT Auto", UnitCost = 5.65M, Ticker = "JMTAUTOLTD", PurchaseDate = DateTime.Now, Qty = 400, Short = false, Exchange = "NSE" },
new Stock { Name = "O N G C", UnitCost = 195.75M, Ticker = "ONGC", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Sun Pharma.Inds.", UnitCost = 531.18M, Ticker = "SUNPHARMA", PurchaseDate = DateTime.Now, Qty = 130, Short = false, Exchange = "NSE" },
new Stock { Name = "UCO Bank", UnitCost = 38.70M, Ticker = "UCOBANK", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Wockhardt", UnitCost = 929.95M, Ticker = "WOCKPHARMA", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
            new Stock { Name = "Aditya Birla Cap", UnitCost = 193.93M, Ticker = "ABCAPITAL", PurchaseDate = DateTime.Now, Qty = 250, Short = false, Exchange = "NSE" },
new Stock { Name = "Amba Enterprises", UnitCost = 0.00M, Ticker = "539196", PurchaseDate = DateTime.Now, Qty = 30, Short = false, Exchange = "BOM" },
new Stock { Name = "Andhra Bank", UnitCost = 58.50M, Ticker = "ANDHRABANK", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Apex Frozen", UnitCost = 328.80M, Ticker = "APEX", PurchaseDate = DateTime.Now, Qty = 50, Short = false, Exchange = "NSE" },
new Stock { Name = "Astra Microwave", UnitCost = 137.08M, Ticker = "ASTRAMICRO", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "CG Power & Indu.", UnitCost = 86.15M, Ticker = "CGPOWER", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Cupid Trades", UnitCost = 144.41M, Ticker = "512361", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "BOM" },
new Stock { Name = "Elder Pharma", UnitCost = 105.45M, Ticker = "ELDERPHARM", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Equitas Holdings", UnitCost = 164.30M, Ticker = "EQUITAS", PurchaseDate = DateTime.Now, Qty = 150, Short = false, Exchange = "NSE" },
new Stock { Name = "Gati", UnitCost = 157.80M, Ticker = "GATI", PurchaseDate = DateTime.Now, Qty = 350, Short = false, Exchange = "NSE" },
new Stock { Name = "Greaves Cotton", UnitCost = 135.39M, Ticker = "GREAVESCOT", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "H D I L", UnitCost = 60.81M, Ticker = "HDIL", PurchaseDate = DateTime.Now, Qty = 600, Short = false, Exchange = "NSE" },
new Stock { Name = "Hindalco Inds.", UnitCost = 260.04M, Ticker = "HINDALCO", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "HPL Electric", UnitCost = 135.25M, Ticker = "HPL", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "JMT Auto 2", UnitCost = 28.25M, Ticker = "JMTAUTOLTD", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "JVL Agro Indus", UnitCost = 37.75M, Ticker = "JVLAGRO", PurchaseDate = DateTime.Now, Qty = 200, Short = false, Exchange = "NSE" },
new Stock { Name = "M R P L", UnitCost = 131.02M, Ticker = "MRPL", PurchaseDate = DateTime.Now, Qty = 1000, Short = false, Exchange = "NSE" },
new Stock { Name = "Marico", UnitCost = 327.65M, Ticker = "MARICO", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Munjal Auto Inds", UnitCost = 85.98M, Ticker = "MUNJALAU", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "O N G C 2", UnitCost = 180.55M, Ticker = "ONGC", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Patidar Build.", UnitCost = 234.55M, Ticker = "524031", PurchaseDate = DateTime.Now, Qty = 25, Short = false, Exchange = "BOM" },
new Stock { Name = "Power Fin.Corpn.", UnitCost = 119.38M, Ticker = "PFC", PurchaseDate = DateTime.Now, Qty = 600, Short = false, Exchange = "NSE" },
new Stock { Name = "Pricol", UnitCost = 108.68M, Ticker = "540293", PurchaseDate = DateTime.Now, Qty = 400, Short = false, Exchange = "BOM" },
new Stock { Name = "Repco Home Fin", UnitCost = 770.48M, Ticker = "REPCOHOME", PurchaseDate = DateTime.Now, Qty = 25, Short = false, Exchange = "NSE" },
new Stock { Name = "Snowman Logistic", UnitCost = 74.10M, Ticker = "SNOWMAN", PurchaseDate = DateTime.Now, Qty = 400, Short = false, Exchange = "NSE" },
new Stock { Name = "Strides Shasun", UnitCost = 880.13M, Ticker = "STAR", PurchaseDate = DateTime.Now, Qty = 120, Short = false, Exchange = "NSE" },
new Stock { Name = "Sun Pharma.Inds. 2", UnitCost = 551.94M, Ticker = "SUNPHARMA", PurchaseDate = DateTime.Now, Qty = 330, Short = false, Exchange = "NSE" },
new Stock { Name = "The Byke Hospi.", UnitCost = 187.75M, Ticker = "BYKE", PurchaseDate = DateTime.Now, Qty = 15, Short = false, Exchange = "NSE" },
new Stock { Name = "Torrent Power", UnitCost = 271.40M, Ticker = "TORNTPOWER", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "UCO Bank 2", UnitCost = 46.25M, Ticker = "UCOBANK", PurchaseDate = DateTime.Now, Qty = 100, Short = false, Exchange = "NSE" },
new Stock { Name = "Wockhardt 2", UnitCost = 805.43M, Ticker = "WOCKPHARMA", PurchaseDate = DateTime.Now, Qty = 200, Short = false, Exchange = "NSE" },};
            foreach (var i in s)
            {
                InsertUpdateData(i, GetPath());
            }
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