using Klak.Spout;
using UnityEngine;

/// <summary>
/// 受け取ったテクスチャのサイズをinterval秒ごとに表示
/// - SpoutReceiverのTargetTextureを設定しないでおくと,受け取ったテクスチャのサイズが表示
/// - TargetTextureにRenderTextureを設定している場合,そのRenderTextureのサイズが表示
/// - obsからの場合,obsの設定>映像>出力解像度に依存
/// </summary>
public class SpoutRecvInfo : MonoBehaviour
{
    public SpoutReceiver spoutReceiver;
    private float timer = 0.0f;
    static private float interval = 1.0f; // 1秒ごとにチェック

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if (spoutReceiver.receivedTexture != null)
            {
                Debug.Log("Width: " + spoutReceiver.receivedTexture.width + ", Height: " + spoutReceiver.receivedTexture.height);
            }

            timer = 0.0f;
        }
    }
}
