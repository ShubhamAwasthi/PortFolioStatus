using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace PortFolioStatus
{
    internal class StocksListViewAdapter : BaseAdapter
    {
        private FragmentActivity activity;
        private List<StockAdapterListItem> items;

        public StocksListViewAdapter(FragmentActivity activity, List<StockAdapterListItem> items)
        {
            this.activity = activity;
            this.items = items;
        }

        public override int Count => items.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
            {
                view = activity.LayoutInflater.Inflate(Resource.Layout.StocksViewListItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.Name).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.Exchange).Text = item.Exchange;
            view.FindViewById<TextView>(Resource.Id.Ticker).Text = item.Ticker;
            view.FindViewById<TextView>(Resource.Id.Qty).Text = item.Qty;
            view.FindViewById<TextView>(Resource.Id.IsShort).Text = item.IsShort;
            view.FindViewById<TextView>(Resource.Id.OriginalPrice).Text = item.OriginalPrice;
            view.FindViewById<TextView>(Resource.Id.OriginalDate).Text = item.OriginalDate;
            view.FindViewById<TextView>(Resource.Id.CurrentPrice).Text = item.CurrentPrice;
            view.FindViewById<TextView>(Resource.Id.CurrentDate).Text = item.CurrentDate;
            view.FindViewById<TextView>(Resource.Id.ChangeFromLastTrade).Text = item.ChangeFromLastTrade;
            view.FindViewById<TextView>(Resource.Id.ChangePctFromLastTrade).Text = item.ChangePctFromLastTrade;
            view.FindViewById<TextView>(Resource.Id.TotalCost).Text = item.TotalCost;
            view.FindViewById<TextView>(Resource.Id.TotalCurrentCost).Text = item.TotalCurrentCost;
            view.FindViewById<TextView>(Resource.Id.TotalChange).Text = item.TotalChange;
            view.FindViewById<TextView>(Resource.Id.TotalChangePct).Text = item.TotalChangePct;

            return view;
        }
    }
}