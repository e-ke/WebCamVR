using System.IO;
using System.Security.Cryptography;
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

    [SerializeField] private MsgWIndow _msgWindow;
    [SerializeField] private Timer30s _timer30s;

    private string logFilePath;
    private StreamWriter streamWriter;
    private bool isTitle = true;  // 起動時はtrue
    // private bool isDemo = false;  // デモプレイ中はtrue
    private bool isSend = false;  // 2回目の入力を送信したらtrue
    // 前回のキー入力
    private Key _prevKey = Key.None;
    private Key[] _tenkeys = new Key[] { Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4, Key.Numpad5, Key.Numpad6 };

    void Start()
    {
        // ログファイルのパスを設定
        string logDirectory = Application.dataPath + "/MyLogs/input/";
        if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
        
        logFilePath = logDirectory + "Input_" + System.DateTime.Now.ToString("yyMMdd_HHmmss") + ".csv";

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
        if (isTitle && current.spaceKey.wasPressedThisFrame)  // 実験開始時一度しか処理が行われないようにする
        {
            LogKey("Space", "start");
            _HMDLogger?.StartHMDLog();
            _VideoController.PlayVideo();
            
            _msgWindow.HideTitleWindow();
            _msgWindow.HideMiscWindow();  // スペース押したらMISC(デモ)消す
            
            _timer30s.StartTimer();  // 30s間隔でmisc表示する用のタイマー開始

            isTitle = false;
            // isDemo = false;
            isSend = false;
            _prevKey = Key.None;
        }

        // デモプレイ開始
        if(current.dKey.wasPressedThisFrame){
            if(isTitle){
                _msgWindow.ShowMiscWindow();
                LogKey("D", "demo");
                isSend = false;
            }
        }

        foreach (Key key in _tenkeys)
        {
            if (current[key].wasPressedThisFrame)
            {
                // Debug.Log(isDemo.ToString() + _msgWindow.GetMiscWinState().ToString() + isSend.ToString());
                if (isTitle)
                {
                    // misc表示されてなかったら表示
                    if (!_msgWindow.GetMiscWinState())
                     {
                        continue;
                    }
                    else // misc表示中
                    {
                        if (_msgWindow.GetMiscWinState() && !isSend)  // MISC表示中なら
                        {
                            string keyNum = key.ToString().Substring(6);
                            if (_prevKey != key)
                            {
                                _msgWindow.SetMiscPin(int.Parse(keyNum));
                                LogKey(keyNum, "d_key");
                                _prevKey = key;
                            }
                            else
                            {
                                _msgWindow.SetMiscPin(int.Parse(keyNum));
                                LogKey(keyNum, "d_ans");
                                _prevKey = Key.None;
                                isSend = true;
                            }
                        }
                    }
                }
                else // 実験中
                {
                    string keyNum = key.ToString().Substring(6);
                    if (_msgWindow.GetMiscWinState() && !isSend)  // MISC表示中なら
                    {
                        if (_prevKey != key) {
                            _msgWindow.SetMiscPin(int.Parse(keyNum));
                            LogKey(keyNum, "key");
                            _prevKey = key;
                        }
                        else{
                            _msgWindow.SetMiscPin(int.Parse(keyNum));
                            LogKey(keyNum, "ans");
                            _prevKey = Key.None;
                            isSend = true;
                        }
                    }
                }
            }
        }
    }

    public void SetSendFlag(bool flag)
    {
        isSend = flag;
    }

    private void LogKey(string keyName, string eventName)
    {
        // 現在時刻を取得し、指定された形式でフォーマット
        string currentTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
        // データをCSV形式でフォーマット
        string logData = $"{currentTime},{keyName},{eventName}";

        // StreamWriterを使用してファイルにデータを書き込む
        streamWriter.WriteLine(logData);
        streamWriter.Flush();  // キー入力の頻度は低いからすぐ書き込む
    }

    // アプリケーション終了時に呼び出される
    private void OnDestroy()
    {
        // StreamWriterを閉じる
        streamWriter?.Close();
    }
}
