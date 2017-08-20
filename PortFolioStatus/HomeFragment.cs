using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Android.Content;

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
            ListView stocksListView = view.FindViewById<ListView>(Resource.Id.stocksList);
            stocksListView.Adapter = new StocksListViewAdapter(this.Activity, stockList);
            stocksListView.ItemClick += (o, e) => Toast.MakeText(inflater.Context, "You clicked" + e.Position, ToastLength.Long).Show();
            var btn = view.FindViewById<Button>(Resource.Id.btnSync);
            var ctx = this.Activity.ApplicationContext;
            btn.Click += (o, e) =>
            {
                var manager = (ConnectivityManager)ctx.GetSystemService(Android.Content.Context.ConnectivityService);
                var netInfo = manager.ActiveNetworkInfo;
                var isOnline = netInfo.IsConnected;
                if (isOnline)
                {
                    Toast.MakeText(ctx, "Connected to network!", ToastLength.Short).Show();
                    Toast.MakeText(ctx, "Calling google finance api!", ToastLength.Short).Show();
                     GetStocksInfo(ctx);
                    Toast.MakeText(ctx, "Got Response!", ToastLength.Short).Show();
                    stocksListView.Adapter = new StocksListViewAdapter(this.Activity, stockList);
                    ((StocksListViewAdapter)stocksListView.Adapter).NotifyDataSetChanged();
                }
                else
                {
                    Toast.MakeText(ctx, "Not Connected to network!\nPlease connect to network!", ToastLength.Long).Show();
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
                    Name = Android.Text.Html.FromHtml("<b>Name: "+item.Name+"</b>").ToString(),
                    Exchange = Android.Text.Html.FromHtml("<b>Exchange: " + item.Exchange + "</b>").ToString(),
                    Ticker = Android.Text.Html.FromHtml("<b>Ticker: " + item.Ticker + "</b>").ToString(),
                    Qty = Android.Text.Html.FromHtml("<b>Units: " + item.Qty + "</b>").ToString(),
                    IsShort = Android.Text.Html.FromHtml("<b>Is this Short? " + (item.Short ? "Yes" : "No") + "</b>").ToString(),
                    OriginalPrice = Android.Text.Html.FromHtml("<b>Unit Cost: " + item.UnitCost + "</b>").ToString(),
                    OriginalDate = Android.Text.Html.FromHtml("<b>Transaction Date: " + item.CurrentDateTime + "</b>").ToString(),
                    CurrentPrice = Android.Text.Html.FromHtml("<b>Current Price: <font color='#EE0000'>Press Sync button!</font></b>").ToString(),
                    CurrentDate = Android.Text.Html.FromHtml("<b>Current Date: <font color='#EE0000'>Press Sync button!</font></b>").ToString(),
                    ChangeFromLastTrade = Android.Text.Html.FromHtml("<b>Change from last Trade: <font color='#EE0000'>Press Sync button!</font></b>").ToString(),
                    ChangePctFromLastTrade = Android.Text.Html.FromHtml("<b>ChangePct from last Trade: <font color='#EE0000'>Press Sync button!</font></b>").ToString(),
                    TotalCost = Android.Text.Html.FromHtml("<b>Total Cost: "+item.UnitCost * item.Qty+"</b>").ToString(),
                    TotalCurrentCost = Android.Text.Html.FromHtml("<b>Total Current Cost: <font color='#EE0000'>Press Sync button!</font></b>").ToString(),
                    TotalChange = Android.Text.Html.FromHtml("<b>Total Change: <font color='#EE0000'>Press Sync button!</font></b>").ToString(),
                    TotalChangePct = Android.Text.Html.FromHtml("<b>Total Change Pct: <font color='#EE0000'>Press Sync button!</font></b>").ToString()
                };
                respList.Add(stockAdapterListItem);
            }
            return respList;
        }

        private void GetStocksInfo(Context ctx)
        {
            var stockGoogle = new List<StockItemGoogle>();
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync("http://finance.google.com/finance/info?client=ig&q=NSE:RELIANCE,NSE:TATAMOTORS,BOM:523754,NSE:Infy").Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    content = content.Substring(4);
                    var jArrayContent = JArray.Parse(content);
                    stockGoogle = GetStocksFromGoogleResponse(jArrayContent);
                }
            }
            catch(Exception e)
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
                        adapterItem.CurrentPrice = Android.Text.Html.FromHtml("<b>Current Price: "+item.Price+"</b>").ToString();
                        adapterItem.CurrentDate = Android.Text.Html.FromHtml("<b>Current Date: "+item.Date+"</b>").ToString();
                        adapterItem.ChangeFromLastTrade = Android.Text.Html.FromHtml("<b>Change from last Trade: "+(FixForShort(item.Change, isShort) < 0 ? "<font color='#EE0000' >"+ FixForShort(item.Change, isShort) + "</font>": "<font color='#00EE00' >" + FixForShort(item.Change, isShort) + "</font>") +"</b>").ToString();
                        adapterItem.ChangePctFromLastTrade = Android.Text.Html.FromHtml("<b>ChangePct from last Trade: "+ (FixForShort(item.ChangePct, isShort) < 0 ? "<font color='#EE0000' >" + FixForShort(item.ChangePct, isShort) + "</font>" : "<font color='#00EE00' >" + FixForShort(item.ChangePct, isShort) + "</font>") + "</b>").ToString();
                        adapterItem.TotalCurrentCost = Android.Text.Html.FromHtml("<b>Total Current Cost: "+ (item.Price * stockListItem.Qty < 0 ? "<font color='#EE0000' >" + item.Price * stockListItem.Qty + "</font>" : "<font color='#00EE00' >" + item.Price * stockListItem.Qty + "</font>") + "</b>").ToString();
                        adapterItem.TotalChange = Android.Text.Html.FromHtml("<b>Total Change: "+ (FixForShort((item.Price - stockListItem.UnitCost), isShort) * stockListItem.Qty < 0 ? "<font color='#EE0000' >" + FixForShort((item.Price - stockListItem.UnitCost), isShort) * stockListItem.Qty + "</font>" : "<font color='#00EE00' >" + FixForShort((item.Price - stockListItem.UnitCost), isShort) * stockListItem.Qty + "</font>") + "</b>").ToString();
                        adapterItem.TotalChangePct = Android.Text.Html.FromHtml("<b>Total Change Pct: "+ (Percentage(stockListItem.UnitCost, item.Price, isShort) < 0 ? "<font color='#EE0000' >" + Percentage(stockListItem.UnitCost, item.Price, isShort) + "</font>" : "<font color='#00EE00' >" + Percentage(stockListItem.UnitCost, item.Price, isShort) + "</font>") + "</b>").ToString();
                    }
                }
            }
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
                    Ticker = item["t"].Value<string>(),
                    Exchange = item["e"].Value<string>(),
                    Price = item["l_fix"].Value<decimal>(),
                    Date = item["lt_dts"].Value<System.DateTime>(),
                    Change = item["c_fix"].Value<decimal>(),
                    ChangePct = item["cp_fix"].Value<decimal>()
                };
                list.Add(stock);
            }

            return list;
        }
    }
}