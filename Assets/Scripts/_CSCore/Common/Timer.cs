using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class Timer
{
    private static readonly DateTime Jan1st1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    //Tuong duong System.TimeCurrentMillis trong Java
    public static long CurrentTimeMillis
    {
        get
        {
            return (long)(DateTime.UtcNow - Jan1st1970Utc).TotalMilliseconds;
        }
    }

    public static long CurrentTimeSeconds
    {
        get
        {
            return (long)(DateTime.UtcNow - Jan1st1970Utc).TotalSeconds;
        }
    }

    public static long calculatingTimePass(long iDateTicks)
    {
        long remainSeconds = System.DateTime.Now.Ticks - iDateTicks;
        long timePass = (long)System.TimeSpan.FromTicks(remainSeconds).TotalSeconds;
        return timePass;
    }

    public static long calculatingTimePassTick(long iDateTicks)
    {
        long remainSeconds = System.DateTime.Now.Ticks - iDateTicks;

        return remainSeconds;
    }

    public static string calculatingTimeDisplay(long remainSeconds, string format, string h = "", string m = "", string s = "")
    {
        StringBuilder sb = new StringBuilder();

        string spaceM = string.IsNullOrEmpty(m) ? ":" : m + " : ";

        if (format.Equals("ss"))
        {
            if (remainSeconds < 10)
                return sb.Append("00").Append(remainSeconds).Append(s).ToString();//(string.Format ("0{0}", remainSeconds));
            return sb.Append("").Append(remainSeconds).Append(s).ToString();// (string.Format ("{0}", remainSeconds));
        }
        //--
        long hour = (long)(remainSeconds / 3600);
        long min = (long)(remainSeconds - hour * 3600) / 60;
        long second = (long)(remainSeconds - (hour * 3600 + min * 60));

        //--
        if (format.Equals("mm/ss"))
        {//cai nay dung trong gamescene nen cho len dau
            min += hour * 60;
            if (second < 10)
                return sb.Append(min).Append(spaceM).Append("0").Append(second).Append(s).ToString();//(string.Format ("{0}:0{1}", min, second));
            return sb.Append(min).Append(spaceM).Append(second).Append(s).ToString();//(string.Format ("{0}:{1}", min, second));
        }
        else if (format.Equals("hh/mm/ss"))
        {

            string spaceH = string.IsNullOrEmpty(h) ? ":" : h + " : ";
            string hourS = hour < 10 ? "0" + hour : "" + hour;
            string minS = min < 10 ? "0" + min : "" + min;
            string secondS = second < 10 ? "0" + second : "" + second;

            return sb.Append(hourS).Append(spaceH).Append(minS).Append(spaceM).Append(secondS).Append(s).ToString(); //(string.Format ("{0}:{1}:{2}", hourS, minS, secondS));
        }
        else if (format.Equals("hh/mm"))
        {

            string spaceH = string.IsNullOrEmpty(h) ? ":" : h + " : ";
            string hourS = hour < 10 ? "0" + hour : "" + hour;
            string minS = min < 10 ? "0" + min : "" + min;
            string secondS = second < 10 ? "0" + second : "" + second;

            if (hour < 1 && min < 1)
                return sb.Append(hourS).Append(spaceH).Append(minS).Append(spaceM).Append(secondS).Append(s).ToString();
            return sb.Append(hourS).Append(spaceH).Append(minS).Append(m).ToString();//(string.Format ("{0}:{1}", hourS, minS));

        }
        else if (format.Equals("dd/hh"))
        {
            long day = remainSeconds / 60 / 60 / 24;
            hour = (long)(remainSeconds / 3600) - day * 24;
            return sb.Append(day).Append(" ngày ").Append(hour).Append(" giờ").ToString();
        }
        else if (format.Equals("dd/hh/mm/ss"))
        {
            long day = remainSeconds / 60 / 60 / 24;
            hour = (long)(remainSeconds / 3600) - day * 24;

            if (day >= 1)
            {
                return sb.Append(day).Append(" ngày, ").Append(hour).Append(" giờ, ").Append(min).Append(" phút, ").Append(second).Append(" giây").ToString();
            }
            else
            {
                string spaceH = string.IsNullOrEmpty(h) ? ":" : h + " : ";
                string hourS = hour < 10 ? "0" + hour : "" + hour;
                string minS = min < 10 ? "0" + min : "" + min;
                string secondS = second < 10 ? "0" + second : "" + second;
                return sb.Append(hourS).Append(spaceH).Append(minS).Append(spaceM).Append(secondS).Append(s).ToString();
            }
        }
        else
        {
            return "";
        }
    }

    public static string calculatingTimeMiliSecond(long remainMiliSeconds, string format, string h = "", string m = "", string s = "")
    {
        string spaceH = string.IsNullOrEmpty(h) ? ":" : h + " : ";
        string spaceM = string.IsNullOrEmpty(m) ? "'" : m + "'";

        if (format.Equals("ss"))
        {
            if ((remainMiliSeconds / 1000) < 10)
                return "00" + (remainMiliSeconds / 1000) + s;
            return "" + (remainMiliSeconds / 1000) + s;
        }
        //--
        long hour = (long)((remainMiliSeconds / 1000) / 3600);
        long min = (long)((remainMiliSeconds / 1000) - hour * 3600) / 60;
        long second = (long)((remainMiliSeconds / 1000) - (hour * 3600 + min * 60));
        long tictac = (long)(remainMiliSeconds - (hour * 3600000 + min * 60000 + second * 1000));
        //--
        string hourS = hour < 10 ? "0" + hour : "" + hour;
        string minS = min < 10 ? "0" + min : "" + min;
        string secondS = second < 10 ? "0" + second : "" + second;
        string tictacS = tictac < 10 ? "0" + tictac : "" + tictac;
        //--
        if (format.Equals("hh/mm/ss"))
        {
            return hourS + spaceH + minS + spaceM + secondS + s; //(string.Format ("{0}:{1}:{2}", hourS, minS, secondS));
        }
        else if (format == "hh/mm")
        {
            if (hour < 1 && min < 1)
                return hourS + spaceH + minS + spaceM + secondS + s;
            return hourS + spaceH + minS + m;//(string.Format ("{0}:{1}", hourS, minS));
        }
        else if (format.Equals("mm/ss"))
        {
            min += hour * 60;
            if (second < 10)
                return min + spaceM + "0" + second + s;//(string.Format ("{0}:0{1}", min, second));
            return min + spaceM + second + s;//(string.Format ("{0}:{1}", min, second));
        }
        else if (format.Equals("mm/ss/tt"))
        {
            min += hour * 60;
            if (second < 10)
                return min + spaceM + "0" + second + s + "''" + tictacS;
            return min + spaceM + second + s + "''" + tictacS;

        }
        else
        {
            return "";
        }
    }

    public static string getTimeDisplay(long remainSeconds)
    {

        if (remainSeconds < 0)
            remainSeconds *= -1;

        long hours = (long)(remainSeconds / 3600);
        long minutes = (long)(remainSeconds - hours * 3600) / 60;
        long seconds = (long)(remainSeconds - (hours * 3600 + minutes * 60));

        if (hours >= (24 * 30))
        {
            return "1 tháng trước";
        }
        else if (hours >= 24)
        {
            if (hours > 24)
            {

            }
            int day = (int)hours / 24;
            int remainHour = (int)(hours - day * 24);

            string time = day + " " + "ngày" + " ";
            if (remainHour >= 1)
                time += remainHour + " " + "giờ trước";
            else
            {
                if (minutes >= 1)
                    time += minutes + " " + "phút trước";
            }

            return (time);
        }
        else if (hours >= 1)
        {

            string time = hours + " " + "giờ trước";
            return (time);
        }
        else
        {
            if (minutes > 1)
                return (minutes + " " + "phút trước");
            else
                return ("Gần đây");
        }
    }

    public static string getTimeDisplay2(double timeCreatedInMiliSeconds, string format = "dd/MM/yyyy hh:mm tt")
    {
        System.DateTime utc = Jan1st1970Utc.AddMilliseconds(timeCreatedInMiliSeconds);
        System.DateTime local = utc.ToLocalTime();

        if (local.Date == System.DateTime.Today)
        {
            return local.ToString("hh:mm tt");
        }

        return local.ToString(format);
    }

    public static string getTimeDisplay4(double timeCreatedInMiliSeconds, string format = "dd/MM/yyyy hh:mm tt")
    {
        System.DateTime utc = Jan1st1970Utc.AddMilliseconds(timeCreatedInMiliSeconds);
        System.DateTime local = utc.ToLocalTime();

        if (local.Date == System.DateTime.Today)
        {
            return local.ToString("hh:mm tt");
        }
        else
        {
            return local.ToString("dd/MM/yyyy");
        }

        return local.ToString(format);
    }

    public static string getTimeDisplay3(double timeCreatedInSeconds, string format = "dd/MM/yyyy hh:mm tt")
    {
        System.DateTime utc = Jan1st1970Utc.AddSeconds(timeCreatedInSeconds);
        System.DateTime local = utc.ToLocalTime();

        if (local.Date == System.DateTime.Today)
        {
            return local.ToString("hh:mm tt");
        }

        return local.ToString(format);
    }

    public static bool IsToday(long timeCreatedInSeconds)
    {
        System.DateTime utc = Jan1st1970Utc.AddSeconds(timeCreatedInSeconds);
        System.DateTime local = utc.ToLocalTime();

        if (local.Date == System.DateTime.Today)
        {
            return true;
        }

        return false;
    }
}

public struct TimeStruct
{
    private long time;
    private long timeNow;

    public long Time
    {
        get
        {
            return this.time;
        }
    }

    public long TimeNow
    {
        get
        {
            return this.timeNow;
        }
    }

    public static implicit operator TimeStruct(long value)
    {
        return new TimeStruct { time = value, timeNow = System.DateTime.Now.Ticks };
    }

    public static TimeStruct operator -(TimeStruct source, long timePass)
    {
        source.time -= timePass;
        source.timeNow = System.DateTime.Now.Ticks;
        return source;
    }

    public static TimeStruct operator +(TimeStruct source, long timePass)
    {
        source.time += timePass;
        source.timeNow = DateTime.Now.Ticks;
        return source;
    }

    public static bool operator >(TimeStruct source, long value)
    {
        return (source.time > value);
    }

    public static bool operator <(TimeStruct source, long value)
    {
        return (source.time < value);
    }

    public static bool operator ==(TimeStruct source, long value)
    {
        return (source.time == value);
    }

    public static bool operator !=(TimeStruct source, long value)
    {
        return (source.time != value);
    }

    public static bool operator >=(TimeStruct source, long value)
    {
        return (source.time >= value);
    }

    public static bool operator <=(TimeStruct source, long value)
    {
        return (source.time <= value);
    }
}
