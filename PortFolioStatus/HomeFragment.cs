using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace PortFolioStatus
{
    public class HomeFragment : Android.Support.V4.App.Fragment
    {
        List<string> stocksList = new List<string>();
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.Home, null);
            stocksList.Add("First");
            stocksList.Add("Second");
            ListView stocksListView = view.FindViewById<ListView>(Resource.Id.stocksList);
            stocksListView.Adapter = new StocksListViewAdapter(this.Activity, stocksList);
            stocksListView.ItemClick += (o, e) => Toast.MakeText(inflater.Context, "You clicked" + e.Position, ToastLength.Long).Show();
            return view;
        }
    }
}