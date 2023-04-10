using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int headShotCount;
    private static int killCount;
    private static DateTime startTime;
    private static DateTime endTime;
    private static TimeSpan clearTime;
    private static string clearTimeText;

    public static void Init() {
        headShotCount = 0;
        killCount = 0;
        startTime = DateTime.Now;
    }

    public static void OnHeadShot() {
        headShotCount++;
    }

    public static void OnKill() {
        killCount++;
    }

    public static void End(bool isClear) {
        endTime = DateTime.Now;
        if (isClear) {
            clearTime = endTime - startTime;
            clearTimeText = $"{clearTime.Minutes} : {clearTime.Seconds}";
        }
        else {
            clearTimeText = "-- : --";
        }
    }

    public static int Score() {
        return headShotCount * 10 + killCount * 50;
    }

    public static int getHeadShotCount { get { return headShotCount; } }
    public static int getKillCound { get { return killCount; } }

    public static String getClearTime { get { return clearTimeText; } }
}
