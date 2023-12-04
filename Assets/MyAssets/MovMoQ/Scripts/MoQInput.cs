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
    [SerializeField] private HMDLogger _HMDLogger;
    [SerializeField] private VideoController _VideoController;

    
    // Update is called once per frame
    void Update()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;
        // キーボードが接続されていない場合
        if (current == null) return;

        // 上下左右キー
        if (current.upArrowKey.wasPressedThisFrame) _Lights_T.Run();
        if (current.downArrowKey.wasPressedThisFrame) _Lights_B.Run();
        if (current.leftArrowKey.wasPressedThisFrame) _Lights_L.Run();
        if (current.rightArrowKey.wasPressedThisFrame) _Lights_R.Run();
        
        // スペースキー
        if (current.spaceKey.wasPressedThisFrame)
        {
            _HMDLogger.StartHMDLog();
            _VideoController.PlayVideo();

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
