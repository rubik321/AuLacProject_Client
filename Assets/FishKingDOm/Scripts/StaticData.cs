using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameMode
{
    Campaign,
    PvP,
    Tower,
    Boss,
    Adventure,
    Normal
}
public class StaticData : MonoBehaviour
{
    public static string playerID;
    public static string result;
    public static bool isGameInit = false;
    public static GameMode GameMode = GameMode.Campaign;
    public static GamePlayState state;
    public static string[] CardTeam;
    public static bool IsAuto = false;
    public static bool isTestMode = false;
    public static int level =-1;
    public static int levelCampaign;
    public static int levelTower;
}


