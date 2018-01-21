using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Util;
using Android.Support.V4.View;

namespace PortFolioStatus
{
    [Activity(Label = "Portfolio\nStatus", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Android.Support.V4.App.FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var arry = new List<Stock>();
            //DBLayer.Flush();
            //if (!DBLayer.GetRecords(ref arry))
            //{
            //    DBLayer.InitDB();
            //}
            //else if (arry.Count == 0)
            //{
            //    DBLayer.Seed();
            //}
            SetContentView(Resource.Layout.Main);
            var fragments = new Android.Support.V4.App.Fragment[] {
                new HomeFragment(),
                new SummaryFragment()
            };
            var titles = Android.Runtime.CharSequence.ArrayFromStringArray(new[]
            {
                "Home",
                "Summary"
            });
            var vp = FindViewById<ViewPager>(Resource.Id.viewPager);

            var adapter = new TabAdapter(base.SupportFragmentManager, fragments, titles);
            vp.Adapter = adapter;

        }
    }
}

