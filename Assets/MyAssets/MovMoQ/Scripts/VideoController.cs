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

    // StreamingAssets/mp4/の1つ目の動画URLを設定するメソッド
    private void SetVideoUrl()
    {
        if (this.videoPlayer != null)
        {
            string videoDirectoryPath = Application.streamingAssetsPath + "/mp4/";
            string[] videoFiles = System.IO.Directory.GetFiles(videoDirectoryPath, "*.mp4");  // mp4ファイルを取得

            if (videoFiles.Length > 0)
            {
                // ディレクトリ内の最初の動画を選択
                string firstVideoPath = videoFiles[0];
                this.videoPlayer.url = firstVideoPath;
            }
            else
            {
                Debug.LogError("動画ファイルが見つかりません。");
            }
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
