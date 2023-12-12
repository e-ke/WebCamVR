using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SimpleUDPServ : MonoBehaviour
{
    public Text receivedMessageText; // Inspectorからアサインするためのpublic変数
    public VideoPlayer _vp; // Inspectorからアサインするためのpublic変数
    [SerializeField] private Lights _Lights_L;
    [SerializeField] private Lights _Lights_R;
    [SerializeField] private Lights _Lights_T;
    [SerializeField] private Lights _Lights_B;

    [SerializeField] private MsgWIndow _msgWindow;
    private String _prevKey = "";
    private bool isDemo = true;
    private bool isSend = false;
    // setter 
    public void SetIsSend(bool isSend)
    {
        this.isSend = isSend;
    }
    
    // [SerializeField] private HMDLogger2 _hmdLogger;
    [SerializeField] private VideoController _videoController;
    [SerializeField] private Timer30s _timer30s;

    private CsvWriter _csvWriter;

    private Thread udpThread;
    private UdpClient udpClient;
    private bool isRunning = true;
    private string receivedMessage = ""; // 受信メッセージを格納するための変数

    private void Start()
    {
        udpThread = new Thread(new ThreadStart(ThreadMethod))
        {
            IsBackground = true
        };
        udpThread.Start();

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "mp4","1.mp4");
        _vp.url = filePath;

        _csvWriter = new CsvWriter("input", "input", "time,key,event");
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(receivedMessage))
        {
            if (receivedMessageText != null)
            {
                receivedMessageText.text = receivedMessage; // UIテキストを更新
            }

            if(receivedMessage == "T"){
                _csvWriter.LogKey(receivedMessage, "moQ");
                _Lights_T.Run();
            }
            if(receivedMessage == "B"){
                _csvWriter.LogKey(receivedMessage, "moQ");
                _Lights_B.Run();
            }
            if(receivedMessage == "L"){
                _csvWriter.LogKey(receivedMessage, "moQ");
                _Lights_L.Run();
            }
            if(receivedMessage == "R"){
                _csvWriter.LogKey(receivedMessage, "moQ");
                _Lights_R.Run();
            }
            // 0 ~ 6
            if (int.TryParse(receivedMessage, out int number) && number >= 0 && number <= 6)
            {
                if (_msgWindow.GetMiscWinState() && !isSend)  // MISC表示中なら
                {
                    if (_prevKey != receivedMessage)
                    {
                        _msgWindow.SetMiscPin(int.Parse(receivedMessage));
                        if (!isDemo) { _csvWriter.LogKey(receivedMessage, "key"); }
                        _prevKey = receivedMessage;
                    }
                    else
                    {
                        _msgWindow.SetMiscPin(int.Parse(receivedMessage));
                        if (!isDemo) { _csvWriter.LogKey(receivedMessage, "ans"); }
                        _prevKey = "";
                        isSend = true;
                    }
                }
            }
            if(isDemo){
                if (receivedMessage == "D")
                {
                    _msgWindow.ShowMiscWindow();
                    isSend = false;
                }
                else if (receivedMessage == "s")
                {
                    isDemo = false;
                    _prevKey = "";
                    isSend = false;

                    _msgWindow.HideTitleWindow();
                    _msgWindow.HideMiscWindow();  // スペース押したらMISC(デモ)消す

                    _videoController.PlayVideo();  // 動画再生
                    _csvWriter.LogKey("0", "start");

                    _timer30s.StartTimer();  // 30s間隔でmisc表示する用のタイマー開始
                }
            }
            receivedMessage = ""; // メッセージをリセット
        }
    }

    private void ThreadMethod()
    {
        udpClient = new UdpClient(7777); // ポート番号(適宜変更)
        while (isRunning)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpClient.Receive(ref remoteEndPoint);
            receivedMessage = Encoding.UTF8.GetString(data);
        }
    }

    private void OnApplicationQuit()
    {
        isRunning = false;
        if (udpClient != null) udpClient.Close();
        if (udpThread != null && udpThread.IsAlive) udpThread.Abort();
    }
}