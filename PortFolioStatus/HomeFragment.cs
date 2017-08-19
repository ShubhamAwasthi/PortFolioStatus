using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System;

namespace PortFolioStatus
{
    public class HomeFragment : Android.Support.V4.App.Fragment
    {
        List<string> stocksList = new List<string>();
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.Home, null);
            for(int i = 0; i  < 200; i++)
                stocksList.Add("Demo");
            ListView stocksListView = view.FindViewById<ListView>(Resource.Id.stocksList);
            stocksListView.Adapter = new StocksListViewAdapter(this.Activity, stocksList);
            stocksListView.ItemClick += (o, e) => Toast.MakeText(inflater.Context, "You clicked" + e.Position, ToastLength.Long).Show();
            var btn = view.FindViewById<Button>(Resource.Id.btnSync);
            var ctx = this.Activity.ApplicationContext;
            btn.Click += (o, e) => {
                                        var manager = (ConnectivityManager)ctx.GetSystemService(Android.Content.Context.ConnectivityService);
                                        var netInfo = manager.ActiveNetworkInfo;
                                        var isOnline = netInfo.IsConnected;
                                        if (isOnline)
                                        {
                                            Toast.MakeText(ctx, "Connected to network!", ToastLength.Short).Show();
                                            Toast.MakeText(ctx, "Calling google finance api!", ToastLength.Short).Show();
                                            var response = GetStocksInfo();
                                            Toast.MakeText(ctx, "Got Response!", ToastLength.Short).Show();
                                            stocksList.Clear();
                                            stocksList.AddRange(response);
                                            stocksListView.Adapter = new StocksListViewAdapter(this.Activity, stocksList);
                                            ((StocksListViewAdapter)stocksListView.Adapter).NotifyDataSetChanged();
                                        }
                                        else
                                        {
                                            Toast.MakeText(ctx, "Not Connected to network!\nPlease connect to network!", ToastLength.Long).Show();
                                        }
            };
            return view;
        }

        private List<string> GetStocksInfo()
        {
            var res = new List<string>();
            for (int i = 0; i < 200; i++)
                res.Add("new val");
            return res;
            //Test comment
        }
    }
}