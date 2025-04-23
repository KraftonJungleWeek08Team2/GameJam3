using Microsoft.Unity.VisualStudio.Editor;
using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private int musicIndex;
    private MusicInfo currentMusicInfo;

    private AudioSource musicSource;
    private float startDelay;
    public float noteInterval;
    private float loopStartTime;
    private float loopEndTime;

    private int loopStartSamples;
    private int loopEndSamples;
    private int loopLengthSamples;

    private double startTime;
    public int currentBeat; //onnextbeat때 1 증가.
    private int lastReportedBeat = -1;
    private float currentPosition;
    private float lastMidPointTrigger = -1f;
    private float lastBeatTrigger = -1f;
    /// <summary>
    /// 비트 사이에 중앙에 해당하는 시간에서 트리거되는 액션
    /// </summary>
    public Action<int> OnNextBeatAction;
    /// <summary>
    /// 비트에 해당하는 시간에서 트리거되는 액션
    /// </summary>
    public Action<int> OnBeatAction;  

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        musicIndex = 0;
        SetMusic(musicIndex);
        StartMusic();
    }

    private void Update()
    {
        LoopControl();
        CheckPosition();
    }

/// <summary>
/// Set Music Info Only to this clas's properties.
/// </summary>
/// <param name="index"></param>
    public void SetMusic(int index)
    {
        currentMusicInfo = Resources.Load<MusicInfo>("KHW/MusicInfos/MusicInfo " + index);
        if (currentMusicInfo == null)
        {
            return;
        }

        musicSource.clip = currentMusicInfo.music;
        startDelay = Mathf.Max(0, currentMusicInfo.startDelay);
        noteInterval = currentMusicInfo.BPM > 0 ? 60f / currentMusicInfo.BPM : 1f;
        loopStartTime = currentMusicInfo.loopStartTime;
        loopEndTime = currentMusicInfo.loopEndTime;

        loopStartSamples = (int)(loopStartTime * musicSource.clip.frequency);
        loopEndSamples = (int)(loopEndTime * musicSource.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;
    }

    public void StartMusic()
    {
        startTime = AudioSettings.dspTime + startDelay;
        musicSource.PlayScheduled(startTime); //delay start time.
    }

    private void LoopControl()
    {
        if (musicSource.timeSamples >= loopEndSamples)
        {
            Debug.Log("Music Looped!");
            musicSource.timeSamples -= loopLengthSamples;
            startTime += (loopEndTime - loopStartTime);
        }
    }

    private void CheckPosition()
    {
        if (musicSource.isPlaying)
        {
            currentPosition = (float)(AudioSettings.dspTime - startTime);
            if (currentPosition < 0) currentPosition = 0;

            int newBeat = (int)(currentPosition / noteInterval);

            float onPointTime = newBeat * noteInterval;
            if (currentPosition >= onPointTime && onPointTime > lastBeatTrigger)
            {
                lastBeatTrigger = onPointTime;
                OnBeatAction?.Invoke(newBeat);
            }

            float midPointTime = (newBeat + 0.5f) * noteInterval;
            if (currentPosition >= midPointTime && midPointTime > lastMidPointTrigger)
            {
                currentBeat = newBeat + 1; 
                lastReportedBeat = currentBeat;
                lastMidPointTrigger = midPointTime;
                OnNextBeatAction?.Invoke(currentBeat);
            }
        }
        else
        {
            currentPosition = 0f;
        }
    }

public float GetTimingOffset()
{
    if (!musicSource.isPlaying || currentPosition <= 0f)
    {
        return 0f;
    }

    // 실시간 비트 계산
    int currentBeat = (int)(currentPosition / noteInterval);
    float currentBeatTime = currentBeat * noteInterval;
    float offset = currentPosition - currentBeatTime;

    // 오프셋 정규화
    if (offset > noteInterval / 2)
        offset -= noteInterval;
    else if (offset < -noteInterval / 2)
        offset += noteInterval;

    return offset;
}
}