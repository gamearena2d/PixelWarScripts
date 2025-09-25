using System.IO;
using UnityEngine;

public static class FileLogger
{
    private static string logFilePath = Path.Combine(Application.persistentDataPath, "game_log.txt");

    public static void Log(string message)
    {
        try
        {
            File.AppendAllText(logFilePath, System.DateTime.Now.ToString("HH:mm:ss") + " - " + message + "\n");
        }
        catch
        {
            // Ignora errori di scrittura
        }
    }
}
