using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PortFolioStatus
{
    [Activity(Label = "Add New Item")]
    public class HomeAdd : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomeAdd);

            var saveBtn = FindViewById<Button>(Resource.Id.btnHomeSave);
            var cancelBtn = FindViewById<Button>(Resource.Id.btnHomeCancel);
            var dateBtn = FindViewById<Button>(Resource.Id.btnHomeDate);
            var selectedDate = FindViewById<EditText>(Resource.Id.HomeAddDate);

            dateBtn.Click += (o, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    selectedDate.Text = time.ToLongDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };
            
            saveBtn.Click += (o, e) =>
            {
                var resp = DBLayer.InsertUpdate(new Stock
                {
                    Name = FindViewById<EditText>(Resource.Id.HomeAddName).Text,
                    PurchaseDate = DateTime.Parse(FindViewById<EditText>(Resource.Id.HomeAddDate).Text),
                    Ticker = FindViewById<EditText>(Resource.Id.HomeAddSymbol).Text,
                    Exchange = FindViewById<EditText>(Resource.Id.HomeAddExchange).Text,
                    Qty = int.Parse(FindViewById<EditText>(Resource.Id.HomeAddUnits).Text),
                    UnitCost = decimal.Parse(FindViewById<EditText>(Resource.Id.HomeAddPrice).Text),
                    Short = FindViewById<EditText>(Resource.Id.HomeAddShort).Text.ToUpper().StartsWith("Y") ? true : false,
                });
                Toast.MakeText(this, resp ? "success" : "fail", ToastLength.Long).Show();
                
                Finish();
            };

            cancelBtn.Click += (o, e) =>
            {
                Finish();
            };

        }
    }
}