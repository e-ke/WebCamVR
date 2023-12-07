using System.Collections;
using System.IO;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HMDLogger2 : MonoBehaviour
{
    [SerializeField] private VelocityEstimator _ve;
    private string _logDirectory;
    private string _logFilePath;
    private float _logInterval = 1f / 20f; // 20Hzでログを取る

    private StreamWriter streamWriter;

    void Start()
    {
        // ログファイルのパスを設定
        _logDirectory = Application.dataPath + "/MyLogs/HMD/";
        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }
    }

    // 記録を開始するメソッド（実験開始時に呼び出す）
    public void StartHMDLog()
    {
        _logFilePath = _logDirectory + "HMDLog_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
        // StreamWriterを初期化し、ヘッダーを書き込む
        streamWriter = new StreamWriter(_logFilePath, true);
        streamWriter.WriteLine("Time,VelX,VelY,VelZ,AccX,AccY,AccZ,AngVelX,AngVelY,AngVelZ");
        
        if(_ve.enabled)
            Debug.Log("VelocityEstimator is enabled.");
            _ve.BeginEstimatingVelocity();
            // ログ記録の開始
            StartCoroutine(LogData());
    }

    private IEnumerator LogData()
    {
        while (true)
        {
            if (_ve != null)
            {
                // HMDの速度、加速度、角速度を取得
                Vector3 velocity = _ve.GetVelocityEstimate();
                Vector3 acceleration = _ve.GetAccelerationEstimate();
                Vector3 angularVelocity = _ve.GetAngularVelocityEstimate();

                // 現在時刻を取得し、指定された形式でフォーマット
                string currentTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
                // データをCSV形式でフォーマットし、StreamWriterを使用してファイルに書き込む
                string logData = $"{currentTime},{velocity.x},{velocity.y},{velocity.z},{acceleration.x},{acceleration.y},{acceleration.z},{angularVelocity.x},{angularVelocity.y},{angularVelocity.z}";
                streamWriter.WriteLine(logData);
            }
            else
            {
                Debug.Log("VelocityEstimator is not available.");
            }
            yield return new WaitForSeconds(_logInterval);
        }
    }

    private void OnDestroy()
    {
        // StreamWriterを閉じる
        streamWriter?.Close();
    }
}
