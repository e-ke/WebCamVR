using System;
using System.IO;
using UnityEngine;

public class CsvWriter
{
    private StreamWriter _streamWriter;
    private string _filePath;

    public CsvWriter(string directory, string filename, string header)
    {
        // persistentDataPathを使用してログディレクトリのパスを設定
        string logDirectory = Path.Combine(Application.persistentDataPath, directory);
        if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);

        _filePath = Path.Combine(logDirectory, DateTime.Now.ToString("yyMMdd_HHmmss") + "_" + filename + ".csv");

        _streamWriter = new StreamWriter(_filePath, true);
        _streamWriter.WriteLine(header);
    }

    public void LogKey(string keyName, string eventName)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss.fff");
        string logData = $"{currentTime},{keyName},{eventName}";

        _streamWriter.WriteLine(logData);
        _streamWriter.Flush();
    }

    public void Close()
    {
        _streamWriter?.Close();
    }
}
