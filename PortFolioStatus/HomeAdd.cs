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

            saveBtn.Click += (o, e) => {
                Finish();
            };

            cancelBtn.Click += (o, e) => {
                Finish();
            };

        }
    }
}