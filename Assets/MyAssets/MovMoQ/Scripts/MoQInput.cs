using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoQInput : MonoBehaviour
{
    // Lights
    [SerializeField] private Lights _Lights_L;
    [SerializeField] private Lights _Lights_R;
    [SerializeField] private Lights _Lights_T;
    [SerializeField] private Lights _Lights_B;
    
    // 実験開始時
    [SerializeField] private HMDLogger2 _HMDLogger;
    [SerializeField] private VideoController _VideoController;

    private string logFilePath;
    private StreamWriter streamWriter;

    void Start()
    {
        // ログファイルのパスを設定
        string logDirectory = Application.dataPath + "/MyLogs/input/";
        if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
        
        logFilePath = logDirectory + "Input_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

        // StreamWriterを初期化し、CSVヘッダーを追加
        streamWriter = new StreamWriter(logFilePath, true);
        streamWriter.WriteLine("Time,KeyPressed,Event");
    }

    // Update is called once per frame
    void Update()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;
        // キーボードが接続されていない場合
        if (current == null) return;

        // 上下左右キー
        if (current.upArrowKey.wasPressedThisFrame)
        {
            LogKey("Up", "moQ");
            _Lights_T.Run();
        }
        if (current.downArrowKey.wasPressedThisFrame){
            LogKey("Down", "moQ");
             _Lights_B.Run();
        }
        if (current.leftArrowKey.wasPressedThisFrame){
            LogKey("Left", "moQ");
             _Lights_L.Run();
        }
        if (current.rightArrowKey.wasPressedThisFrame){
            LogKey("Right", "moQ");
             _Lights_R.Run();
        }


        // スペースキー
        if (current.spaceKey.wasPressedThisFrame)
        {
            LogKey("Space", "start");
            _HMDLogger.StartHMDLog();
            _VideoController.PlayVideo();
        }


        // テンキーの数字キー
        if (current.numpad0Key.wasPressedThisFrame)
        {
            LogKey("Numpad0", "key");
            Debug.Log("テンキーの0が押された");
        }
        if (current.numpad1Key.wasPressedThisFrame)
        {
            LogKey("Numpad1", "key");
            Debug.Log("テンキーの1が押された");
        }
        if (current.numpad2Key.wasPressedThisFrame)
        {
            LogKey("Numpad2", "key");
            Debug.Log("テンキーの2が押された");
        }
        if (current.numpad3Key.wasPressedThisFrame)
        {
            LogKey("Numpad3", "key");
            Debug.Log("テンキーの3が押された");
        }
        if (current.numpad4Key.wasPressedThisFrame)
        {
            LogKey("Numpad4", "key");
            Debug.Log("テンキーの4が押された");
        }
        if (current.numpad5Key.wasPressedThisFrame)
        {
            LogKey("Numpad5", "key");
            Debug.Log("テンキーの5が押された");
        }
        if (current.numpad6Key.wasPressedThisFrame)
        {
            LogKey("Numpad6", "key");
            Debug.Log("テンキーの6が押された");
        }
    }

    private void LogKey(string keyName, string eventName)
    {
        // データをCSV形式でフォーマット
        string logData = $"{Time.time},{keyName},{eventName}";
        // StreamWriterを使用してファイルにデータを書き込む
        streamWriter.WriteLine(logData);
    }

    // アプリケーション終了時に呼び出される
    private void OnDestroy()
    {
        // StreamWriterを閉じる
        streamWriter?.Close();
    }
}
