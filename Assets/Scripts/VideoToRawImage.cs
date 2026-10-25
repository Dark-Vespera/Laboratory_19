using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoToRawImage : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;

    void Update()
    {
        if (videoPlayer.isPrepared)
        {
            rawImage.texture = videoPlayer.texture;
        }

        if (!videoPlayer.isPlaying && videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
    }
}