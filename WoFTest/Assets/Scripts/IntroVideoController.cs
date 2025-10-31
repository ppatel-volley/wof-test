using UnityEngine;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    [Header("Video Setup")]
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private Camera videoCamera;

    [SerializeField]
    private Camera gameplayCamera;

    [Header("Options")]
    [SerializeField]
    private bool playOnStart = true;

    [SerializeField]
    private bool disableGameplayCameraDuringVideo = true;

    [SerializeField]
    private bool disableVideoCameraAfterPlayback = true;

    [SerializeField]
    private Behaviour[] disableWhileVideo;

    private bool hasStarted;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer == null)
        {
            Debug.LogError("IntroVideoController requires a VideoPlayer reference.", this);
            enabled = false;
            return;
        }

        if (videoCamera != null)
        {
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.targetCamera = videoCamera;
            videoPlayer.targetCameraAlpha = 1f;
            videoPlayer.aspectRatio = VideoAspectRatio.FitInside;
        }
    }

    private void OnEnable()
    {
        if (videoPlayer == null)
        {
            return;
        }

        videoPlayer.loopPointReached += HandleVideoFinished;
        videoPlayer.prepareCompleted += HandleVideoPrepared;
    }

    private void OnDisable()
    {
        if (videoPlayer == null)
        {
            return;
        }

        videoPlayer.loopPointReached -= HandleVideoFinished;
        videoPlayer.prepareCompleted -= HandleVideoPrepared;
    }

    private void Start()
    {
        if (!playOnStart)
        {
            return;
        }

        StartVideoSequence();
    }

    public void StartVideoSequence()
    {
        if (videoPlayer == null)
        {
            return;
        }

        if (hasStarted)
        {
            return;
        }

        hasStarted = true;
        SetupForVideo();

        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
            return;
        }

        if (videoPlayer.clip != null || !string.IsNullOrEmpty(videoPlayer.url))
        {
            videoPlayer.Prepare();
        }
        else
        {
            Debug.LogWarning("IntroVideoController has no VideoClip assigned. Skipping intro.", this);
            FinishVideoPlayback();
        }
    }

    private void SetupForVideo()
    {
        if (disableGameplayCameraDuringVideo && gameplayCamera != null)
        {
            gameplayCamera.enabled = false;
        }

        if (videoCamera != null)
        {
            videoCamera.enabled = true;
        }

        foreach (Behaviour behaviour in disableWhileVideo)
        {
            if (behaviour == null)
            {
                continue;
            }

            behaviour.enabled = false;
        }
    }

    private void HandleVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    private void HandleVideoFinished(VideoPlayer vp)
    {
        FinishVideoPlayback();
    }

    private void FinishVideoPlayback()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        if (disableVideoCameraAfterPlayback && videoCamera != null)
        {
            videoCamera.enabled = false;
        }

        if (gameplayCamera != null)
        {
            gameplayCamera.enabled = true;
        }

        foreach (Behaviour behaviour in disableWhileVideo)
        {
            if (behaviour == null)
            {
                continue;
            }

            behaviour.enabled = true;
        }
    }
}

