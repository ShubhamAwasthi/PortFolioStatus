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
            var selectedDate = FindViewById<EditText>(Resource.Id.homeDate);
            var timeBtn = FindViewById<Button>(Resource.Id.btnHomeTime);
            var selectedTime = FindViewById<EditText>(Resource.Id.homeTime);
            dateBtn.Click += (o, e) => {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    selectedDate.Text = time.ToLongDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };
            timeBtn.Click += (o, e) => {
                TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (TimePickerDialog.TimeSetEventArgs time)
                {
                    selectedTime.Text = time.HourOfDay + ":" + time.Minute;
                });
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };
            saveBtn.Click += (o, e) => {
                Finish();
            };

            cancelBtn.Click += (o, e) => {
                Finish();
            };

        }
    }
}