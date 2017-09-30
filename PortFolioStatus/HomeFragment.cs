using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using System.Text.RegularExpressions;

namespace PortFolioStatus
{
    public class HomeFragment : Android.Support.V4.App.Fragment
    {
        List<StockAdapterListItem> stockList = new List<StockAdapterListItem>();
        List<Stock> dbList = new List<Stock>();
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            DBLayer.Flush();
            DBLayer.Seed();
            View view = inflater.Inflate(Resource.Layout.Home, null);
            stockList = GetStockListForAdapter();
            var stocksListView = view.FindViewById<ExpandableListView>(Resource.Id.stocksList);
            var adapter = new ExpandableStockListAdapter(this.Activity, stockList, dbList, new List<StockItemGoogle>());
            stocksListView.SetAdapter(adapter);
            stocksListView.ItemClick += (o, e) => Toast.MakeText(Android.App.Application.Context, "You clicked" + e.Position, ToastLength.Long).Show();
            var btn = view.FindViewById<Button>(Resource.Id.btnSync);
            var ctx = this.Activity.ApplicationContext;
            var btnAdd = view.FindViewById<Button>(Resource.Id.btnAdd);
            btnAdd.Click += (o, e) => {
                var intent = new Intent(ctx, typeof(HomeAdd)).SetFlags(ActivityFlags.ClearTask);
                StartActivity(intent);
            };
            btn.Click += (o, e) =>
            {
                var manager = (ConnectivityManager)ctx.GetSystemService(Android.Content.Context.ConnectivityService);
                var isOnline = false;
                try
                {
                    var netInfo = manager.ActiveNetworkInfo;
                    isOnline = netInfo.IsConnected;
                }catch(Exception ex)
                {
                    Log.Error("OnCreateView", "No network" + ex.Message);
                }
                if (isOnline)
                {
                    Toast.MakeText(Android.App.Application.Context, "Connected to network!", ToastLength.Short).Show();
                    Toast.MakeText(Android.App.Application.Context, "Calling google finance api!", ToastLength.Short).Show();
                    var stockGoogle = GetStocksInfo(Android.App.Application.Context);
                    Toast.MakeText(Android.App.Application.Context, "Got Response!", ToastLength.Short).Show();
                    adapter = new ExpandableStockListAdapter(this.Activity, stockList, dbList, stockGoogle)
                    {
                        IsFixed = true
                    };
                    stocksListView.SetAdapter(adapter);
                    //adapter.NotifyDataSetChanged();
                }
                else
                {
                    Toast.MakeText(Android.App.Application.Context, "Not Connected to network!\nPlease connect to network!", ToastLength.Long).Show();
                }
            };
            return view;
        }


        private List<StockAdapterListItem> GetStockListForAdapter()
        {
            DBLayer.GetRecords(ref dbList);
            var respList = new List<StockAdapterListItem>();
            foreach (var item in dbList)
            {
                var stockAdapterListItem = new StockAdapterListItem
                {
                    Name = "Name: "+item.Name,
                    Exchange = "Exchange: " + item.Exchange,
                    Ticker = "Ticker: " + item.Ticker,
                    Qty = "Units: " + item.Qty,
                    IsShort = "Is this Short? " + (item.Short ? "Yes" : "No"),
                    OriginalPrice = "Unit Cost: " + item.UnitCost,
                    OriginalDate = "Transaction Date: " + item.CurrentDateTime,
                    CurrentPrice = "Current Price: Press Sync button!",
                    CurrentDate = "Current Date: Press Sync button!",
                    ChangeFromLastTrade = "Change from last Trade: Press Sync button!",
                    ChangePctFromLastTrade = "ChangePct from last Trade: Press Sync button!",
                    TotalCost = "Total Cost: "+item.UnitCost * item.Qty+"",
                    TotalCurrentCost = "Total Current Cost: Press Sync button!",
                    TotalChange = "Total Change: Press Sync button!",
                    TotalChangePct = "Total Change Pct: Press Sync button!"
                };
                respList.Add(stockAdapterListItem);
            }
            return respList;
        }

        private List<StockItemGoogle> GetStocksInfo(Context ctx)
        {
            var stockGoogle = new List<StockItemGoogle>();
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync("https://finance.google.com/finance?q=NSE:RELIANCE,NSE:TATAMOTORS,BOM:523754,NSE:Infy").Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    content = Regex.Match(content, "rows\":(?<value>\\[.*?}\\])").Value.Substring(6);
                    var jArrayContent = JArray.Parse(content);
                    stockGoogle = GetStocksFromGoogleResponse(jArrayContent);
                }
            }
            catch(HttpRequestException e)
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
                    if(adapterItem.Exchange.Contains(item.Exchange) && adapterItem.Ticker.Contains(item.Ticker))
                    {
                        var stockListItem = GetItemFromList(item.Exchange, item.Ticker);
                        var stockListItemDB = GetItemFromDB(item.Exchange, item.Ticker);
                        bool isShort = stockListItemDB.Short;
                        adapterItem.CurrentPrice = "Current Price: " + item.Price;
                        adapterItem.CurrentDate = "Current Date: " + item.Date;
                        adapterItem.ChangeFromLastTrade = "Change from last Trade: " + (FixForShort(item.Change, isShort) < 0 ? "" + FixForShort(item.Change, isShort) + "" : "" + FixForShort(item.Change, isShort) + "");
                        adapterItem.ChangePctFromLastTrade = "ChangePct from last Trade: " + (FixForShort(item.ChangePct, isShort) < 0 ? "" + FixForShort(item.ChangePct, isShort) + "" : "" + FixForShort(item.ChangePct, isShort) + "");
                        adapterItem.TotalCurrentCost = "Total Current Cost: " + (item.Price * stockListItem.Qty < 0 ? "" + item.Price * stockListItem.Qty + "" : "" + item.Price * stockListItem.Qty + "");
                        adapterItem.TotalChange = "Total Change: " + (FixForShort((item.Price - stockListItem.UnitCost), isShort) * stockListItem.Qty < 0 ? "" + FixForShort((item.Price - stockListItem.UnitCost), isShort) * stockListItem.Qty + "" : "" + FixForShort((item.Price - stockListItem.UnitCost), isShort) * stockListItem.Qty + "");
                        adapterItem.TotalChangePct = "Total Change Pct: " + (Percentage(stockListItem.UnitCost, item.Price, isShort) < 0 ? "" + Percentage(stockListItem.UnitCost, item.Price, isShort) + "" : "" + Percentage(stockListItem.UnitCost, item.Price, isShort) + "");
                    }
                }
            }
            return stockGoogle;
            //Test comment
        }

        private decimal FixForShort(decimal item, bool isShort = false)
        {
            if (isShort)
                return -item;
            return item;
        }

        private Stock GetItemFromDB(string exchange, string ticker)
        {
            foreach (var item in dbList)
                if (item.Ticker == ticker && item.Exchange == exchange)
                    return item;
            return null;
        }

        private decimal Percentage(decimal initial, decimal final, bool IsShort = false)
        {
            decimal resp = 0;
            if(IsShort)
                resp =  (initial - final) * 100 / final;
            else
                resp = (final - initial) * 100 / initial;
            resp = Math.Round(resp, 2);
            return resp;
        }

        private Stock GetItemFromList(string exchange, string ticker)
        {
            foreach(var item in dbList)
            {
                if (item.Exchange == exchange && item.Ticker == ticker)
                    return item;
            }
            return null;
        }

        private List<StockItemGoogle> GetStocksFromGoogleResponse(JArray arr)
        {
            var list = new List<StockItemGoogle>();

            foreach (var item in arr)
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

            return list;
        }
    }
}