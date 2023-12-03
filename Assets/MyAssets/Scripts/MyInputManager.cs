using UnityEngine;
using UnityEngine.InputSystem;

public class MyInputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;
        // キーボードが接続されていない場合
        if (current == null) return;

        // aキー
        if (current.aKey.wasPressedThisFrame)
        {
            Debug.Log("aが押された");
        }

        // テンキーの数字キー
        if (current.numpad0Key.wasPressedThisFrame)
        {
            Debug.Log("テンキーの0が押された");
        }
        if (current.numpad1Key.wasPressedThisFrame)
        {
            Debug.Log("テンキーの1が押された");
        }
        if (current.numpad2Key.wasPressedThisFrame)
        {
            Debug.Log("テンキーの2が押された");
        }
        if (current.numpad3Key.wasPressedThisFrame)
        {
            Debug.Log("テンキーの3が押された");
        }
        if (current.numpad4Key.wasPressedThisFrame)
        {
            Debug.Log("テンキーの4が押された");
        }
        if (current.numpad5Key.wasPressedThisFrame)
        {
            Debug.Log("テンキーの5が押された");
        }
        if (current.numpad6Key.wasPressedThisFrame)
        {
            Debug.Log("テンキーの6が押された");
        }
    }
}
