using UnityEngine;
using System.Collections;

public class Timer30s : MonoBehaviour
{
    private Coroutine timerCoroutine;
    [SerializeField] private MsgWIndow _msgWindow;
    [SerializeField] private MoQInput _moQInput;

    private int _count = 0;

    // タイマーをスタートするメソッド
    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // 既に実行中のタイマーがあれば停止
        }
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    // 30秒ごとに実行されるコルーチン
    private IEnumerator TimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(30); // 30秒待機
            ExecutePeriodicMethod(); // 特定のメソッドを実行
        }
    }

    // 定期的に実行されるメソッド
    private void ExecutePeriodicMethod()
    {
        // 10分経過で終了
        if (_count == 19)  
        {
            Debug.Log("10分経過");
            _msgWindow.ShowEndWindow();
            StopCoroutine(timerCoroutine);
            return;
        }
        Debug.Log("30秒経過");
        _moQInput.SetSendFlag(false);
        _msgWindow.ShowMiscWindow();
        _count++;
    }
}
