using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // UI操作のために追加
using UnityEngine.XR;

public class HMDLogger : MonoBehaviour
{
    private InputDevice hmdDevice;
    private string logDirectory;
    private string logFilePath;

    private float logInterval = 1f / 20f; // 20Hzでログを取る

    public Text angularVelocityText; // UIテキストへの参照

    void Start()
    {
        // HMDのデバイスを取得
        hmdDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        // ログファイルのパスを設定
        logDirectory = Application.dataPath + "/MyFileIO/logs/";
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    // 記録を開始するメソッド(実験開始時に呼び出す)
    public void StartHMDLog()
    {
        logFilePath = logDirectory + "HMDLog_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
        // CSVヘッダーを追加
        File.AppendAllText(logFilePath, "Time,PosX,PosY,PosZ,RotX,RotY,RotZ,RotW,AccX,AccY,AccZ,AngVelX,AngVelY,AngVelZ\n");
        // ログ記録の開始
        StartCoroutine(LogData());
    }

    private IEnumerator LogData()
    {
        while (true)
        {
            if (hmdDevice.isValid)
            {
                // HMDのデータを取得
                hmdDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
                hmdDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
                hmdDevice.TryGetFeatureValue(CommonUsages.deviceAcceleration, out Vector3 acceleration);
                hmdDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 angularVelocity);

                // UIテキストを更新
                UpdateAngularVelocityText(angularVelocity);

                // データをCSV形式でフォーマット
                string logData = $"{Time.time},{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z},{rotation.w},{acceleration.x},{acceleration.y},{acceleration.z},{angularVelocity.x},{angularVelocity.y},{angularVelocity.z}\n";

                // CSVファイルにデータを追記
                File.AppendAllText(logFilePath, logData);
            }
            else
            {
                Debug.Log("HMD is not valid.");
            }

            // 次のログまで待機
            yield return new WaitForSeconds(logInterval);
        }
    }

    // UIテキストに角速度を表示するメソッド
    private void UpdateAngularVelocityText(Vector3 angularVelocity)
    {
        angularVelocityText.text = $"Angular Velocity: {angularVelocity.x:F2}, {angularVelocity.y:F2}, {angularVelocity.z:F2}";
    }
}
