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
        private static DateTime ParsedDate(string date)
        {


            var monthDigit = new Dictionary<string, string>() {
                { "january", "1" },
                { "february", "2" },
                { "march", "3" },
                { "april", "4" },
                { "may", "5" },
                { "june", "6" },
                { "july", "7" },
                { "august", "8" },
                { "september", "9" },
                { "october", "10" },
                { "november", "11" },
                { "december", "12" },
            };
            return DateTime.Parse(date);
            var tokens = date.Split(' ');
            var year = tokens[2].Trim();
            var month = monthDigit[tokens[1].ToLower().Trim()];
            var day = tokens[0].Trim();
            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomeAdd);

            var saveBtn = FindViewById<Button>(Resource.Id.btnHomeSave);
            var cancelBtn = FindViewById<Button>(Resource.Id.btnHomeCancel);
            var dateBtn = FindViewById<Button>(Resource.Id.btnHomeDate);
            var selectedDate = FindViewById<EditText>(Resource.Id.HomeAddDate);
            var id = Intent.GetStringExtra("Id") ?? null;
            if (!string.IsNullOrEmpty(id))
            {
                Stock stock = DBLayer.GetRecordByID(int.Parse(id));
                FindViewById<EditText>(Resource.Id.HomeAddName).Text = stock.Name;
                FindViewById<EditText>(Resource.Id.HomeAddSymbol).Text = stock.Ticker;
                FindViewById<EditText>(Resource.Id.HomeAddExchange).Text = stock.Exchange;
                FindViewById<EditText>(Resource.Id.HomeAddUnits).Text = stock.Qty.ToString();
                FindViewById<EditText>(Resource.Id.HomeAddPrice).Text = stock.UnitCost.ToString();
                FindViewById<EditText>(Resource.Id.HomeAddShort).Text = stock.Short ? "yes" : "no" ;
                FindViewById<EditText>(Resource.Id.HomeAddDate).Text = stock.PurchaseDate.ToString("dddd, MMMM %d, yyyy");
            }

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
                var date = FindViewById<EditText>(Resource.Id.HomeAddDate).Text;
                DateTime parsedDate = ParsedDate(date);
                var isShort = FindViewById<EditText>(Resource.Id.HomeAddShort).Text.ToUpper().StartsWith("Y");
                var insert = new Stock
                {
                    Name = FindViewById<EditText>(Resource.Id.HomeAddName).Text,
                    PurchaseDate = parsedDate,
                    Ticker = FindViewById<EditText>(Resource.Id.HomeAddSymbol).Text,
                    Exchange = FindViewById<EditText>(Resource.Id.HomeAddExchange).Text,
                    Qty = int.Parse(FindViewById<EditText>(Resource.Id.HomeAddUnits).Text),
                    UnitCost = decimal.Parse(FindViewById<EditText>(Resource.Id.HomeAddPrice).Text),
                    Short = isShort,
                };
                if (!string.IsNullOrEmpty(id))
                    insert.ID = int.Parse(id);
                else {
                    var refRecord = new List<Stock>();
                    DBLayer.GetRecords(ref refRecord);
                    var record = refRecord.Where(x => x.Name.Trim() == insert.Name.Trim());
                    if (record.Any()) {
                        Toast.MakeText(this, "Please Choose different name", ToastLength.Long).Show();
                        return;
                    }
                }
                var resp = DBLayer.InsertUpdate(insert);
                Toast.MakeText(this, resp ? "success" : "fail", ToastLength.Long).Show();

                var intent = new Intent(this, typeof(MainActivity)).SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            };

            cancelBtn.Click += (o, e) =>
            {
                base.Finish();
            };

        }
    }
}