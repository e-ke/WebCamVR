using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer; // VideoPlayerコンポーネント
    [SerializeField] private string videoName;        // 再生する動画のファイル名[xxx].mp4

    void Start()
    {
        SetVideoUrl();
    }

    // 動画URLを設定するメソッド
    private void SetVideoUrl()
    {
        if (this.videoPlayer != null)
        {
            this.videoPlayer.url = Application.dataPath + "/MyFileIO/mp4/" + this.videoName;
        }
    }

    // 動画の再生を行うメソッド
    public void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    // 動画の停止を行うメソッド
    public void StopVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }
}
