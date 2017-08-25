using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using Java.Lang;
using Android.Graphics;

namespace PortFolioStatus
{
    public class ExpandableStockListAdapter : BaseExpandableListAdapter
    {
        private FragmentActivity activity;
        private List<StockAdapterListItem> items;
        private List<Stock> dbItems;
        public bool IsFixed = false;
        private List<StockItemGoogle> googleItems;
        public ExpandableStockListAdapter(FragmentActivity activity, List<StockAdapterListItem> items, List<Stock> dbItems, List<StockItemGoogle> googleItems)
        {
            this.activity = activity;
            this.items = items;
            this.dbItems = dbItems;
            this.googleItems = googleItems;
        }


        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return 1;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                var inflater = activity.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
                view = inflater.Inflate(Resource.Layout.StocksViewListItem, null);
            }
            var item = items[groupPosition];
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

            if (!IsFixed)
                return view;

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
            else if (!view.Text.Contains("!"))
                view.SetTextColor(Color.Green);
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                var inflater = activity.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
                view = inflater.Inflate(Resource.Layout.StocksViewGroupListItem, null);
            }

            var title = view.FindViewById<TextView>(Resource.Id.groupTitle);
            var price = view.FindViewById<TextView>(Resource.Id.groupPrice);
            var currentPrice = view.FindViewById<TextView>(Resource.Id.groupCurrentPrice);

            var listItem = items[groupPosition];

            var dbItem = dbItems.Find(x=>listItem.Exchange.Contains(x.Exchange) 
                                    && listItem.Ticker.Contains(x.Ticker) 
                                    && listItem.OriginalPrice.Contains(x.UnitCost.ToString()));

            title.Text = dbItem.Name.Trim().PadRight(25).PadLeft(30);
            var titleColor = Color.Snow;
            title.SetTextColor(titleColor);
            price.SetTextColor(titleColor);
            currentPrice.SetTextColor(titleColor);
            if (!IsFixed)
            {
                if (dbItem.Short)
                {
                    currentPrice.Text = dbItem.UnitCost.ToString().Trim().PadRight(12).PadLeft(10);
                    price.Text = "-".Trim().PadRight(12).PadLeft(10);
                }
                else
                {
                    currentPrice.Text = "-".Trim().PadRight(12).PadLeft(10);
                    price.Text = dbItem.UnitCost.ToString().Trim().PadRight(12).PadLeft(10);
                }
            }
            else
            {
                var googleItem = GetIndexForGoogle(dbItem.Exchange, dbItem.Ticker);
                if (dbItem.Short)
                {
                    currentPrice.Text = dbItem.UnitCost.ToString().Trim().PadRight(12).PadLeft(10);
                    price.Text = (googleItem.Price.ToString().Trim() + "*").PadRight(12).PadLeft(10);
                    if (dbItem.UnitCost < googleItem.Price)
                        price.SetTextColor(Color.Red);
                    else
                        price.SetTextColor(Color.Green);
                }
                else
                {
                    price.Text = dbItem.UnitCost.ToString().Trim().PadRight(12).PadLeft(10);
                    currentPrice.Text = (googleItem.Price.ToString().Trim() + "*").PadRight(12).PadLeft(10);
                    if (dbItem.UnitCost > googleItem.Price)
                        currentPrice.SetTextColor(Color.Red);
                    else
                        currentPrice.SetTextColor(Color.Green);
                }
            }
            return view;
        }

        private StockItemGoogle GetIndexForGoogle(string exchange, string ticker)
        {
            foreach (var item in googleItems)
                if (item.Exchange.Equals(exchange) && item.Ticker.Equals(ticker))
                    return item;
            return null;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override int GroupCount
        {
            get { return items.Count; }
        }

        public override bool HasStableIds
        {
            get { return true; }
        }
    }
}