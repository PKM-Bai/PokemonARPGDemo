using System;
using UnityEngine;


public class Settings : MonoBehaviour
{
    //* 时间相关
    public const float secondThreshold = 0.01f;    // 秒数值，当达到这个值时代表走了一秒。这个数值越小代表时间走的越快
    public const int secondHold = 59;   // 每个时间的临界点
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;

    //* 灯光
    public const float lightChangeDuration = 20f;
    public static TimeSpan morningTime = new TimeSpan(6, 0, 0);
    public static TimeSpan nightTime = new TimeSpan(19, 0, 0);


    //* 加载场景相关
    public const float loadFadeDuration = 1.4f;


}
