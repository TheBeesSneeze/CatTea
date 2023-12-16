/*******************************************************************************
* File Name :         WebVideoPlayer.cs
* Author(s) :         Toby Schamberger
* Creation Date :     12/16/2023
*
* Brief Description : why are you reading this. unless youre toby from the future
* or something
*****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WebVideoPlayer : MonoBehaviour
{
    public string VideoName;
    private VideoPlayer videoPlayer;
    
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, VideoName+".mp4");
    }
}
