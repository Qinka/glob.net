using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Language.Haskell.Time
{
    /// <summary>
    /// The transformer, from C#'s date to Haskell's UTC time.
    /// </summary>
    public class UTCTimeBuilder
    {
        private DateTime __time;
        public  DateTime time { get { return __time; } }
        public  UTCTimeBuilder()
        {
            __time = DateTime.UtcNow;
        }
        public  UTCTimeBuilder(DateTime t)
        {
            __time = t.ToUniversalTime();
        }
        public override string ToString()
        {
            return __time.Year.ToString() + "-" + __time.Month.ToString().PadLeft(2,'0') + "-" + __time.Day.ToString().PadLeft(2, '0') + " "
                + __time.Hour.ToString().PadLeft(2, '0') + ":" + __time.Minute.ToString().PadLeft(2, '0') 
                + ":" + __time.Second.ToString().PadLeft(2, '0') + "." + __time.Millisecond.ToString() + " UTC";
        }
    }

    public class HTTPTimeBuilder
    {
        static private string[] MonthShort = { "Jan", "Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"};
        static private string[] WeekShort = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

        private DateTime __time;
        public DateTime time { get { return __time; } }
        public  HTTPTimeBuilder()
        {
            __time = DateTime.UtcNow;
        }
        public  HTTPTimeBuilder(DateTime t)
        {
            __time = t.ToUniversalTime();
        }
        public override string ToString()
        {
            return  WeekShort[(int)(__time.DayOfWeek)] + ", " + __time.Day.ToString().PadLeft(2, '0') + " " 
                + MonthShort[__time.Month-1] + " " + __time.Year.ToString() + " "
                + __time.Hour.ToString().PadLeft(2, '0') + ":" + __time.Minute.ToString().PadLeft(2, '0')
                + ":" + __time.Second.ToString().PadLeft(2, '0') + " GMT";
        }
    }
}
