using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Slider))]
public class VideoTrack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public VideoPlayer video;
    [SerializeField] Slider track;
    bool slide = false;

    void Start()
    {
        track = GetComponent<Slider>();
    }

    void Update()
    {
        if (!slide)
        {
            track.value = video.frame / (float)video.frameCount;
        }
    }

    public void OnPointerDown(PointerEventData e)
    {
        slide = true;
    }

    public void OnPointerUp(PointerEventData e)
    {
        float frame = (float)track.value * video.frameCount;

        video.frame = (long)frame;
        slide = false;
    }
}
