using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtils
{
    public static readonly long SECOND2_MILLI_SECOND = 1000;
    public static readonly long MIN_MILLI_SECOND = 60 * 1000;
    public static readonly long HOUR_MILLI_SECOND = 60 * 60 * 1000;
    public static readonly long DAY_MILLI_SECOND = 24 * HOUR_MILLI_SECOND;

    public static readonly long WEEK_MILLI_SECOND = 7 * DAY_MILLI_SECOND;

    //            
    public static readonly long HOUR_SECOND = 60 * 60;

    public static readonly long DAY_SECOND = 24 * HOUR_SECOND;

    //            
    public static readonly long HOUR_MIN = 60;
    public static readonly long DAY_MIN = 24 * HOUR_MIN;

    public static readonly long WEEK_MIN = 7 * DAY_MIN;

    //            
    public static readonly long DAY_HOUR = 24;
    public static readonly long WEEK_HOUR = 7 * DAY_HOUR;

    private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long CurrentTimeMillis => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


    public static long CurrentTimeSeconds()
    {
        return (long)(CurrentTimeMillis / 1000);
    }

    /// <param name="posTime"> pos time de cache time hanh dong </param>
    /// <param name="timeSecond"> tinh theo giay, neu qua thoi gian nay se tra ve true</param>
    public static bool AfterTime(long posTime, float timeSecond)
    {
        return CurrentTimeMillis - posTime > (timeSecond * SECOND2_MILLI_SECOND);
    }

    public static string FormatHourFromSec(long sec)
    {
        long hour = sec / 3600;
        long tmp = sec % 3600;
        long min = tmp / 60;
        long sec2 = tmp % 60;
        //
        long day = hour / 24;
        if (day > 0)
        {
            hour = hour % 24;
            return day + "N " + FormatPrefixZero(hour) + ":" + FormatPrefixZero(min) + ":" + FormatPrefixZero(sec2);
        }
        else return FormatPrefixZero(hour) + ":" + FormatPrefixZero(min) + ":" + FormatPrefixZero(sec2);
    }

    public static string FormatHourMinFromMin(long min)
    {
        long hour = min / 60;
        long cMin = min % 60;
        return FormatPrefixZero(hour) + "h:" + FormatPrefixZero(cMin)+"p";
    }

    public static string FormatHourFromSec2(long sec)
    {
        TimeSpan t = TimeSpan.FromSeconds(sec);
        string time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
            t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
        return time;
    }


    private static string FormatPrefixZero(long value)
    {
        if (value < 10)
        {
            return "0" + value;
        }
        else
        {
            return value.ToString();
        }
    }
}