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
    public int currentBeat { get; private set; } // 비트 계속 증가
    private int lastReportedBeat = -1;
    private float currentPosition;
    private float lastMidPointTrigger = -1f;
    private float lastBeatTrigger = -1f;
    private int loopCount = 0; // 루프 횟수 추적
    private int loopBeatCount; // 한 루프의 비트 수

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

        // 루프 구간의 비트 수 계산
        loopBeatCount = Mathf.FloorToInt((loopEndTime - loopStartTime) / noteInterval);
    }

    public void StartMusic()
    {
        startTime = AudioSettings.dspTime + startDelay;
        musicSource.PlayScheduled(startTime);
    }

    private void LoopControl()
    {
        if (musicSource.timeSamples >= loopEndSamples)
        {
            Debug.Log($"Music Looped! Loop Count: {loopCount + 1}");
            musicSource.timeSamples -= loopLengthSamples;
            startTime += (loopEndTime - loopStartTime);
            loopCount++; // 루프 횟수 증가
            currentBeat += loopBeatCount; // 루프 비트 수 추가
        }
    }

    private void CheckPosition()
    {
        if (musicSource.isPlaying)
        {
            currentPosition = (float)(AudioSettings.dspTime - startTime);
            if (currentPosition < 0) currentPosition = 0;

            // 루프 비트를 포함한 비트 계산
            int newBeat = (int)(currentPosition / noteInterval) + (loopCount * loopBeatCount);

            float onPointTime = (newBeat - (loopCount * loopBeatCount)) * noteInterval;
            if (currentPosition >= onPointTime && onPointTime > lastBeatTrigger)
            {
                lastBeatTrigger = onPointTime;
                OnBeatAction?.Invoke(newBeat);
            }

            float midPointTime = (newBeat - (loopCount * loopBeatCount) + 0.5f) * noteInterval;
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

        int currentBeat = (int)(currentPosition / noteInterval) + (loopCount * loopBeatCount);
        float currentBeatTime = (currentBeat - (loopCount * loopBeatCount)) * noteInterval;
        float offset = currentPosition - currentBeatTime;

        if (offset > noteInterval / 2)
            offset -= noteInterval;
        else if (offset < -noteInterval / 2)
            offset += noteInterval;

        return offset;
    }
}