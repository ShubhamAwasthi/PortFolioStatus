using Android.Graphics;
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

            var chgLastTrade = view.FindViewById<TextView>(Resource.Id.ChangeFromLastTrade);
            var chgPctLastTrade = view.FindViewById<TextView>(Resource.Id.ChangePctFromLastTrade);
            var totalChange = view.FindViewById<TextView>(Resource.Id.TotalChange);
            var totalChangePct = view.FindViewById<TextView>(Resource.Id.TotalChangePct);



            var name = view.FindViewById<TextView>(Resource.Id.Name);
            var ex = view.FindViewById<TextView>(Resource.Id.Exchange);
            var t = view.FindViewById<TextView>(Resource.Id.Ticker);
            var qty = view.FindViewById<TextView>(Resource.Id.Qty);
            var isShort = view.FindViewById<TextView>(Resource.Id.IsShort);
            var origPrice = view.FindViewById<TextView>(Resource.Id.OriginalPrice);
            var origDt = view.FindViewById<TextView>(Resource.Id.OriginalDate);
            var curPrice = view.FindViewById<TextView>(Resource.Id.CurrentPrice);
            var curDt = view.FindViewById<TextView>(Resource.Id.CurrentDate);
            var totalCost = view.FindViewById<TextView>(Resource.Id.TotalCost);
            var totalCurCost = view.FindViewById<TextView>(Resource.Id.TotalCurrentCost);
            

            FixColor(chgLastTrade);
            FixColor(chgPctLastTrade);
            FixColor(totalChange);
            FixColor(totalChangePct);

            MakeYellow(ex, chgLastTrade);
            MakeYellow(t, chgLastTrade);
            MakeYellow(qty, chgLastTrade);
            MakeYellow(isShort, chgLastTrade);
            MakeYellow(origPrice, chgLastTrade);
            MakeYellow(origDt, chgLastTrade);
            MakeYellow(curPrice, chgLastTrade);
            MakeYellow(curDt, chgLastTrade);
            MakeYellow(totalCost, chgLastTrade);
            MakeYellow(totalCurCost, chgLastTrade);

            name.SetTextColor(Color.Chocolate);

            return view;
        }

        private void MakeYellow(TextView view, TextView chgLast)
        {
            if (!chgLast.Text.Contains("!"))
                view.SetTextColor(Color.Yellow);
        }

        private void FixColor(TextView view)
        {
            if (view.Text.Contains("-"))
                view.SetTextColor(Color.Red);
            else if(!view.Text.Contains("!"))
                view.SetTextColor(Color.Green);
        }
    }
}