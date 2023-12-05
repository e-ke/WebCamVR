using System.Collections;
using System.IO;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HMDLogger2 : MonoBehaviour
{
    [SerializeField] private VelocityEstimator _ve;
    private string logDirectory;
    private string logFilePath;
    private float logInterval = 1f / 20f; // 20Hzでログを取る

    void Start()
    {
        // ログファイルのパスを設定
        logDirectory = Application.dataPath + "/MyFileIO/logs/";
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    // 記録を開始するメソッド（実験開始時に呼び出す）
    public void StartHMDLog()
    {
        logFilePath = logDirectory + "HMDLog_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
        // CSVヘッダーを追加
        File.AppendAllText(logFilePath, "Time,VelX,VelY,VelZ,AccX,AccY,AccZ,AngVelX,AngVelY,AngVelZ\n");
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

                // データをCSV形式でフォーマット
                string logData = $"{Time.time},{velocity.x},{velocity.y},{velocity.z},{acceleration.x},{acceleration.y},{acceleration.z},{angularVelocity.x},{angularVelocity.y},{angularVelocity.z}\n";

                // CSVファイルにデータを追記
                File.AppendAllText(logFilePath, logData);
            }
            else
            {
                Debug.Log("VelocityEstimator is not available.");
            }

            // 次のログまで待機
            yield return new WaitForSeconds(logInterval);
        }
    }
}
