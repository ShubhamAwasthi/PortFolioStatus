using Android.Support.V4.App;
using Java.Lang;

namespace PortFolioStatus
{
    public class TabAdapter : FragmentPagerAdapter
    {
        private Android.Support.V4.App.Fragment[] Fragments;
        private ICharSequence[] Titles;

        public TabAdapter(Android.Support.V4.App.FragmentManager fm, Android.Support.V4.App.Fragment[] fragments, ICharSequence[] titles) : base(fm)
        {
            Fragments = fragments;
            Titles = titles;
        }
        public override int Count => Fragments.Length;

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return Fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return Titles[position];
        }
    }
}