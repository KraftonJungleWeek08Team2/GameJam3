using Microsoft.Unity.VisualStudio.Editor;
using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private GameObject noteObject;
    [SerializeField] private GameObject beatBarPanel;
    //[SerializeField] private Image centralBarImage;
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
    private int currentBeat;
    private int lastReportedBeat = -1;
    private float currentPosition;
    private float lastMidPointTrigger = -1f;
    private float lastBeatTrigger = -1f;
    public Action<int> OnNextBeatAction;
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
                Instantiate(noteObject, beatBarPanel.transform);
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
        if (!musicSource.isPlaying)
        {
            return 0f;
        }

        //currentPosition => 노래에서 현재 위치, currentbeattime -> 현재 비트의 노래기반 시간. 그 차이를 계산.
        float currentBeatTime = (currentBeat) * noteInterval;
        float offset = currentPosition - currentBeatTime;
        return offset;
    }
}