using UnityEngine;
using UnityEngine.UI;

public class DispIPAddress : MonoBehaviour
{
    public Text ipText;

    void Start()
    {
        if (ipText != null)
        {
            ipText.text = "IP Addr: " + MyNetworkUtils.GetLocalIPAddress();
        }
    }
}
