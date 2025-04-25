using System.Xml.Serialization;
using UnityEngine;

public class BeatBarUISystem : MonoBehaviour
{
    BeatBarSystem beatBarSystem;
    Canvas beatBarCanvas;

    void Start()
    {
        beatBarSystem = GetComponent<BeatBarSystem>();
        beatBarCanvas = GetComponent<Canvas>();

        beatBarSystem.OnEnableBeatBarAction += EnableBeatBar;
        beatBarSystem.OnDisableBeatBarAction += DisableBeatBar;
    }

    /// <summary>
    /// 캔버스를 활성화
    /// </summary>
    private void EnableBeatBar()
    {
        beatBarCanvas.enabled = true;
    }

    /// <summary>
    /// 캔버스 비활성화
    /// </summary>
    private void DisableBeatBar()
    {
        beatBarCanvas.enabled = false;
    }


    private void OnDestroy()
    {
        UnSubscribeAction();
    }

    /// <summary>
    /// Action 해제
    /// </summary>
    private void UnSubscribeAction()
    {
        beatBarSystem.OnEnableBeatBarAction -= EnableBeatBar;
        beatBarSystem.OnDisableBeatBarAction -= DisableBeatBar;    
    }
}
