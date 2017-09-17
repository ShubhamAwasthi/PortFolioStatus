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
using Android.Util;

namespace PortFolioStatus
{
    public class TimePickerFragment : DialogFragment,
                                  TimePickerDialog.IOnTimeSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(TimePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<TimePickerDialog.TimeSetEventArgs> _dateSelectedHandler = delegate { };

        public static TimePickerFragment NewInstance(Action<TimePickerDialog.TimeSetEventArgs> onTimeSelected)
        {
            TimePickerFragment frag = new TimePickerFragment();
            frag._dateSelectedHandler = onTimeSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            TimePickerDialog dialog = new TimePickerDialog(Activity,
                this, currently.Hour, currently.Minute, true);
            return dialog;
        }

        public void OnTimeSet(TimePicker tp, int hour, int minutes)
        {
            Log.Debug(TAG, "Called time changed");
            _dateSelectedHandler(new TimePickerDialog.TimeSetEventArgs(hour, minutes));
        }
    }
}