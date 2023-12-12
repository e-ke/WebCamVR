using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CheckPaths : MonoBehaviour
{
    public Text pathsText; // InspectorからアサインするテキストUI

    void Start()
    {
        if (pathsText == null)
        {
            Debug.LogError("PathsText is not assigned!");
            return;
        }

        string internalStoragePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        // パスをテキストに設定
        pathsText.text = "DataPath: " + Application.dataPath + "\n" +
                         "PersistentDataPath: " + Application.persistentDataPath + "\n" +
                         "StreamingAssetsPath: " + Application.streamingAssetsPath + "\n" +
                         "InternalStoragePath: " + internalStoragePath + "\n" +
                         "InternalStorage[0]:" + FirstDirectory(internalStoragePath);
    }

    // 指定されたパス内の最初のフォルダを取得するメソッド
    string FirstDirectory(string path)
    {
        try
        {
            string[] directories = Directory.GetDirectories(path);
            if (directories.Length > 0)
            {
                return directories[0]; // 最初のフォルダのみを返す
            }
            else
            {
                return "None"; // フォルダがない場合
            }
        }
        catch
        {
            return "None"; // エラーが発生した場合、エラーメッセージを表示しない
        }
    }
}
