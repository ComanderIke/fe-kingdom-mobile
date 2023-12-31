using UnityEditor.Graphs;
using UnityEngine;

public class MyDebug
{
    public static void Log(string message, Color color= default)
    {
        string colorString= string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (byte)(color.r * 255f), (byte)(color.g * 255f),
            (byte)(color.b * 255f));
        Debug.Log(colorString+message+"</color>");
    }
    public static void LogTest(string message)
    {
        Color color = new Color(.6f, .4f, .4f);
        string colorString= string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (byte)(color.r * 255f), (byte)(color.g * 255f),
            (byte)(color.b * 255f));
        Debug.Log(colorString+"#TEST: "+message+"</color>");
    }
    public static void LogTest(object message)
    {
       LogTest(message.ToString());
    }
    public static void LogInput(string message)
    {
        Color color = new Color(0.9f, .5f, .6f);
        string colorString= string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (byte)(color.r * 255f), (byte)(color.g * 255f),
            (byte)(color.b * 255f));
        Debug.Log(colorString+"#INPUT: "+message+"</color>");
    }

    public static void LogEngine(string message)
    {
        Color color = new Color(0.9f, .5f, .0f);
        string colorString= string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (byte)(color.r * 255f), (byte)(color.g * 255f),
            (byte)(color.b * 255f));
        Debug.Log(colorString+"#ENGINE: "+message+"</color>");
    }

    public static void LogMusic(string message)
    {
        Color color = new Color(0.8f, .8f, .2f);
        string colorString= string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (byte)(color.r * 255f), (byte)(color.g * 255f),
            (byte)(color.b * 255f));
        Debug.Log(colorString+"#MUSIC: "+message+"</color>");
    }

    public static void LogPersistance(string message)
    {
        Color color=new Color(.5f, .7f, 1);
        string colorString= string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (byte)(color.r * 255f), (byte)(color.g * 255f),
            (byte)(color.b * 255f));
        Debug.Log(colorString+"#PERSISTANCE: "+message+"</color>");
    }

    public static void LogLogic(string message)
    {
        Color color=new Color(.5f, 1f, 1);
        string colorString= string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (byte)(color.r * 255f), (byte)(color.g * 255f),
            (byte)(color.b * 255f));
        Debug.Log(colorString+"#LOGIC: "+message+"</color>");
    }
}