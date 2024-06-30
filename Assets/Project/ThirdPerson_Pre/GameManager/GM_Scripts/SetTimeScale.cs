using UnityEngine;

public static class SetTimeScale
{
    public static void FrezzGame() => Time.timeScale = 0f;
    public static void UnFrezzeGame() => Time.timeScale = 1f;
}
