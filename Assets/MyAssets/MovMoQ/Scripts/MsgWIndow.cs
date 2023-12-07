using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MsgWIndow : MonoBehaviour
{
    [SerializeField] private GameObject titleWindow;
    [SerializeField] private GameObject miscWindow;
    private CanvasGroup miscCanvasGp;
    [SerializeField] private CanvasGroup endWindow;
    [SerializeField] private GameObject _pinsParent;
    private List<UnityEngine.UI.Image> pins;
    private Color originalColor;

    [SerializeField] private TextMeshProUGUI _miscHeader;

    private bool isAppearMiscWindow = false;
    public bool GetMiscWinState() { return isAppearMiscWindow; }
    private bool isFading = false;
    private int _prevPin = -1;
    


    void Start()
    {
        pins = GetAllChildren(_pinsParent);
        originalColor = pins[0].color;

        foreach (UnityEngine.UI.Image pin in pins)
        {
            pin.color = new Color(originalColor[0], originalColor[1], originalColor[2], 0);
        }

        miscCanvasGp = miscWindow.GetComponent<CanvasGroup>();
        miscCanvasGp.alpha = 0;
        endWindow.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 30sごとに呼び出す
    public void ShowMiscWindow()
    {
        if (isAppearMiscWindow || isFading) return; // 既に表示中かフェード中なら何もしない
        isFading = true; // フェード開始
        Debug.Log("MISC表示");
        _miscHeader.text = "回答を選択してください。";
        FadeIn(miscCanvasGp, 0f, 0.5f, () =>
        {
            isAppearMiscWindow = true;
            isFading = false; // フェード終了
        });
    }

    public void HideMiscWindow()
    {
        if (!isAppearMiscWindow || isFading) return; // 既に非表示かフェード中なら何もしない
        isFading = true; // フェード開始
        FadeOut(miscCanvasGp, 1.5f, 0.5f, () =>
        {
            isAppearMiscWindow = false;
            HideAllPins();  //次のmisc表示時にピンが表示されないようにする
            isFading = false; // フェード終了
        });
    }

    public void SetMiscPin(int pin)
    {
        if(isFading) return;  // フェード中の入力を無視
        if (_prevPin == pin)
        {
            _miscHeader.text = pin.ToString() + ":回答ありがとうございます。";
            HideMiscWindow();
            _prevPin = -1;
        }
        else {
            if(_prevPin!=-1) pins[_prevPin].color = new Color(originalColor[0], originalColor[1], originalColor[2], 0);
            pins[pin].color = new Color(originalColor[0], originalColor[1], originalColor[2], 1);
            _miscHeader.text = pin.ToString() + "でよろしいですか？";
            _prevPin = pin;
        }
    }

    // 全てのピンを非表示にする
    private void HideAllPins(){
        foreach(UnityEngine.UI.Image pin in pins)
        {
            pin.color = new Color(originalColor[0], originalColor[1], originalColor[2], 0);
        }
    }   


    // タイトル画面を非表示にする
    public void HideTitleWindow()
    {
        FadeOut(titleWindow.GetComponent<CanvasGroup>(), 0.1f, 0.4f, () =>
        {
            Destroy(titleWindow, 0f);  // 最初しか使わないからシーンから削除
        });
    }

    public void ShowEndWindow()
    {
        FadeIn(endWindow, 0f, 1f, () =>
        {
            Debug.Log("実験終了");
        });
    }

    List<UnityEngine.UI.Image> GetAllChildren(GameObject parent)
    {
        List<UnityEngine.UI.Image> childrenImg = new List<UnityEngine.UI.Image>();

        foreach (Transform child in parent.transform)
        {
            childrenImg.Add(child.GetComponent<UnityEngine.UI.Image>());
        }

        return childrenImg;
    }




    // フェードインを実行するメソッド
    public void FadeIn<T>(T component, float waitTime, float fadeDuration, Action onCompleted) where T : Component
    {
        StartCoroutine(Fader(component, waitTime, fadeDuration, 0, 1, onCompleted));
    }
    // フェードアウトを実行するメソッド
    public void FadeOut<T>(T component, float waitTime, float fadeDuration, Action onCompleted) where T : Component
    {
        StartCoroutine(Fader(component, waitTime, fadeDuration, 1, 0, onCompleted));
    }
    // フェードアウト(キャンバスグループ｜イメージ)
    IEnumerator Fader<T>(T component, float waitTime, float fadeDuration, float startAlpha, float endAlpha, Action onCompleted) where T : Component
    {
        // 指定された時間だけ待機
        yield return new WaitForSeconds(waitTime);

        float counter = 0;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, counter / fadeDuration);

            if (component is CanvasGroup)
            {
                (component as CanvasGroup).alpha = alpha;
            }
            else if (component is UnityEngine.UI.Image)
            {
                Color color = (component as UnityEngine.UI.Image).color;
                color.a = alpha;
                (component as UnityEngine.UI.Image).color = color;
            }

            yield return null;
        }

        // 最終的にアルファ値をendAlphaに設定
        if (component is CanvasGroup)
        {
            (component as CanvasGroup).alpha = endAlpha;
        }
        else if (component is UnityEngine.UI.Image)
        {
            Color color = (component as UnityEngine.UI.Image).color;
            color.a = endAlpha;
            (component as UnityEngine.UI.Image).color = color;
        }

        // 処理が完了したので、コールバックを呼び出す
        onCompleted?.Invoke();
    }
}
