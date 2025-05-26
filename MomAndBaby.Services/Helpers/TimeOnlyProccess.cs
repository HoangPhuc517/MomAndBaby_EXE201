using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.Helpers
{
    public static class TimeOnlyProccess
    {
        public static string ConvertToString(TimeOnly start, TimeOnly end)
        {
            return start.ToString("HH\\:mm") + "-" + end.ToString("HH\\:mm");
        }

        public static (TimeOnly Start, TimeOnly End) ConvertToTimeOnlyRange(string timeRange)
        {
            var parts = timeRange.Split('-');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid time range format. Expected format: 'HH:mm-HH:mm'.");

            var start = TimeOnly.ParseExact(parts[0], "HH:mm");
            var end = TimeOnly.ParseExact(parts[1], "HH:mm");

            return (start, end);
        }

    }
}
