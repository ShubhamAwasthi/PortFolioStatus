using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System;
using Android.Net;
using Android.Util;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Android.Content;
using System.Linq;

namespace PortFolioStatus
{
    public class SummaryFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Summary, container, false);

            var displayText = view.FindViewById<TextView>(Resource.Id.summaryText);

            displayText.Text = "This is the summary of your transactions";

            decimal transactionCost = 0;
            decimal currentCost = 0;

            var records = new List<Stock>();
            DBLayer.GetRecords(ref records);
            bool RecordStatus = GetCosts(ref transactionCost, ref currentCost, this.Activity.ApplicationContext, records);
            if (RecordStatus)
            {
                displayText.Text += "\nTotal cost: " + transactionCost.ToString() + "\nTotal Value: " + currentCost.ToString();
            }
            else {
                displayText.Text += "\nPlease connect with net to find values";
            }
            return view;
        }

        private bool GetCosts(ref decimal transactionCost, ref decimal currentCost, Context ctx, List<Stock> stockList)
        {
            var manager = (ConnectivityManager)ctx.GetSystemService(Android.Content.Context.ConnectivityService);
            var isOnline = false;
            try
            {
                var netInfo = manager.ActiveNetworkInfo;
                isOnline = netInfo.IsConnected;
            }
            catch (Exception ex)
            {
                Log.Error("OnCreateView", "No network" + ex.Message);
            }
            if (isOnline)
            {
                var stockGoogle = GetStocksInfo(ctx, stockList);
                foreach (var item in stockList)
                {
                    if (item.Short)
                    {
                        currentCost += item.UnitCost * item.Qty;
                        transactionCost += item.CurrentUnitCost * item.Qty;
                    }
                    else
                    {
                        transactionCost += item.UnitCost * item.Qty;
                        currentCost += item.CurrentUnitCost * item.Qty;
                    }
                }
            }
            return isOnline;
        }
        private List<StockItemGoogle> GetStocksInfo(Context ctx, List<Stock> stockList)
        {
            var stockGoogle = new List<StockItemGoogle>();
            var records = new List<Stock>();
            DBLayer.GetRecords(ref records);
            
            try
            {
                var list = new List<Stock>();
                var count = list.Count;
                do
                {
                    list = records.Skip(count).Take(10).ToList();
                    count += 10;
                    var items = string.Join(",", list.Select(x => x.Exchange + ":" + x.Ticker).ToList());
                    using (var client = new HttpClient())
                    {
                        var response = client.GetAsync("https://finance.google.com/finance?q=" + items).Result;
                        var content = response.Content.ReadAsStringAsync().Result;
                        content = Regex.Match(content, "rows\":(?<value>\\[.*}\\]),\"visible_cols").Value.Substring(6);
                        content = content.Substring(0, content.LastIndexOf(",\"visible_cols"));
                        var jArrayContent = JArray.Parse(content);
                        stockGoogle.AddRange(GetStocksFromGoogleResponse(jArrayContent));
                    }
                } while (count < records.Count);
            }
            catch (HttpRequestException e)
            {
                Toast.MakeText(ctx, "Error while querying the google finance api\n" + e.Message, ToastLength.Long).Show();
            }
            catch (Exception e)
            {
                Toast.MakeText(ctx, "Error while querying the google finance api\n" + e.Message, ToastLength.Long).Show();
            }
            foreach (var item in stockGoogle)
            {
                foreach (var adapterItem in stockList)
                {
                    if (adapterItem.Exchange.Contains(item.Exchange) && adapterItem.Ticker.Contains(item.Ticker))
                    {
                        var stockListItemDB = GetItemFromList(item.Exchange, item.Ticker, stockList, adapterItem.Name);
                        bool isShort = stockListItemDB.Short;
                        adapterItem.CurrentUnitCost = item.Price;
                    }
                }
            }
            return stockGoogle;
            //Test comment
        }

        private Stock GetItemFromList(string exchange, string ticker, List<Stock> dbList, string name)
        {
            foreach (var item in dbList)
            {
                if (item.Exchange == exchange && item.Ticker == ticker && item.Name.Trim() == name.Trim())
                    return item;
            }
            return null;
        }


        private List<StockItemGoogle> GetStocksFromGoogleResponse(JArray arr)
        {
            var list = new List<StockItemGoogle>();

            foreach (var item in arr)
            {
                try
                {
                    var stock = new StockItemGoogle
                    {

                        Ticker = item["values"][0].Value<string>(),
                        Exchange = item["values"][8].Value<string>(),
                        Price = item["values"][2].Value<decimal>(),
                        Date = DateTime.Now,
                        Change = item["values"][3].Value<decimal>(),
                        ChangePct = item["values"][5].Value<decimal>()
                    };
                    list.Add(stock);
                }
                catch (Exception ex) { }
                
            }

            return list;
        }
    }
}