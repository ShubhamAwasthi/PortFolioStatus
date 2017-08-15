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
        private List<string> items;

        public StocksListViewAdapter(FragmentActivity activity, List<string> items)
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

            view.FindViewById<TextView>(Resource.Id.Name).Text = item;

            return view;
        }
    }
}