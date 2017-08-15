using Android.OS;
using Android.Views;

namespace PortFolioStatus
{
    public class SummaryFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.Summary, container, false);
        }
    }
}