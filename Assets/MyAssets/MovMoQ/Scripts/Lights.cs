using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lights : MonoBehaviour
{
    [SerializeField] private GameObject spritePrefab; // Spriteのプレハブ
    [SerializeField] private int rightNum = 5;        // 生成するSpriteの数
    [SerializeField] private Vector3 startPoint;      // 開始位置
    [SerializeField] private Vector3 endPoint;        // 終了位置
    [SerializeField] private float oneStrokeDuration = 0.6f; // 1ストロークの時間
    [SerializeField] private int strokeNum = 5;       // ストローク数
    [SerializeField] private float targetAlpha = 1f;  // 目標透明度

    [SerializeField] private bool mirrorX = false; // Y軸対称の設定

    private List<GameObject> spriteObjects = new List<GameObject>(); // 生成されたSpriteのリスト
    private List<GameObject> mirroredSpriteObjects = new List<GameObject>(); // YZ平面対称のSpriteリスト
    private bool isRunning = false;                   // 実行中フラグ
    public bool IsRunning()
    {
        return isRunning;
    }

    
    void Start()
    {
        CreateSprites();
    }

    private void CreateSprites()
    {
        if (rightNum <= 0 || spritePrefab == null) return;

        // 各Sprite間の間隔を計算
        Vector3 interval = (endPoint - startPoint) / Mathf.Max(rightNum - 1, 1);

        for (int i = 0; i < rightNum; i++)
        {
            // Spriteをインスタンス化して配置
            GameObject newSprite = Instantiate(spritePrefab, startPoint + interval * i, Quaternion.identity);
            // このオブジェクトの子として設定
            newSprite.transform.SetParent(transform, false);
            // 生成したSpriteをリストに追加
            spriteObjects.Add(newSprite);
        }

        // mirrorYがtrueの場合、すべてのSpriteを複製してY軸対称に配置
        if (mirrorX)
        {
            foreach (GameObject original in spriteObjects.ToArray()) // ToArray()でリストのコピーを作成
            {
                GameObject mirroredSprite = Instantiate(original, transform, false);
                Vector3 mirroredPosition = original.transform.localPosition;
                mirroredPosition.x = -mirroredPosition.x; // X座標を反転
                mirroredSprite.transform.localPosition = mirroredPosition;
                mirroredSpriteObjects.Add(mirroredSprite);
            }
        }
    }


    public void Run()
    {
        if (!isRunning) StartCoroutine(ExecuteProcess());
    }

    private IEnumerator ExecuteProcess()
    {
        isRunning = true;
        for (int c = 0; c < strokeNum; c++)
        {
            float delay = oneStrokeDuration / (rightNum + 1);
            for (int i = 0; i < rightNum; i++)
            {
                StartCoroutine(ChangeAlpha(spriteObjects[i].GetComponent<SpriteRenderer>(), oneStrokeDuration / 3, targetAlpha));
                if (mirrorX && i < mirroredSpriteObjects.Count)
                {
                    StartCoroutine(ChangeAlpha(mirroredSpriteObjects[i].GetComponent<SpriteRenderer>(), oneStrokeDuration / 3, targetAlpha));
                }
                yield return new WaitForSeconds(delay);
            }
        }
        isRunning = false;
        Debug.Log("Finished");
    }

    // duration秒かけてSpriteの透明度を変更(小さいほど早く点滅)
    private IEnumerator ChangeAlpha(SpriteRenderer spriteRenderer, float duration, float targetAlpha)
    {
        float initialAlpha = spriteRenderer.color.a;
        float elapsedTime = 0;

        // 目標透明度に変更
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Color newColor = spriteRenderer.color;
            newColor.a = Mathf.Lerp(initialAlpha, targetAlpha, t);
            spriteRenderer.color = newColor;
            yield return null;
        }

        // 透明度を0に戻す
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Color newColor = spriteRenderer.color;
            newColor.a = Mathf.Lerp(targetAlpha, 0, t);
            spriteRenderer.color = newColor;
            yield return null;
        }
    }
}
