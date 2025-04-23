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
    public int currentBeat { get; private set; }
    private int lastReportedBeat = -1;
    private float currentPosition;
    private float lastMidPointTrigger = -1f;
    private float lastBeatTrigger = -1f;
    private int loopCount = 0;
    private int loopBeatCount;

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
            //Debug.LogError($"MusicInfo {index} not found!");
            return;
        }

        musicSource.clip = currentMusicInfo.music;
        if (musicSource.clip == null)
        {
            //Debug.LogError("Music clip is null!");
            return;
        }

        startDelay = Mathf.Max(0, currentMusicInfo.startDelay);
        noteInterval = currentMusicInfo.BPM > 0 ? 60f / currentMusicInfo.BPM : 1f;
        loopStartTime = currentMusicInfo.loopStartTime;
        loopEndTime = currentMusicInfo.loopEndTime;

        if (loopEndTime <= loopStartTime)
        {
            //Debug.LogWarning("Invalid loop times: loopEndTime <= loopStartTime. Disabling loop.");
            loopEndTime = musicSource.clip.length;
            loopStartTime = 0;
        }

        loopStartSamples = (int)(loopStartTime * musicSource.clip.frequency);
        loopEndSamples = (int)(loopEndTime * musicSource.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;

        loopBeatCount = Mathf.FloorToInt((loopEndTime - loopStartTime) / noteInterval);
        //Debug.Log($"SetMusic: clip={musicSource.clip.name}, noteInterval={noteInterval}, loopBeatCount={loopBeatCount}, loopStartTime={loopStartTime}, loopEndTime={loopEndTime}");
    }

    public void StartMusic()
    {
        if (musicSource.clip == null)
        {
            //ebug.LogError("Cannot start music: Audio clip is null!");
            return;
        }
        startTime = AudioSettings.dspTime + startDelay;
        musicSource.PlayScheduled(startTime);
        //Debug.Log($"StartMusic: startTime={startTime}, clip={musicSource.clip.name}, isPlaying={musicSource.isPlaying}");
    }

    private void LoopControl()
    {
        if (musicSource.timeSamples >= loopEndSamples)
        {
            //Debug.Log($"Music Looped! Loop Count: {loopCount + 1}, timeSamples: {musicSource.timeSamples}, currentBeat: {currentBeat}");
            musicSource.timeSamples -= loopLengthSamples;
            startTime += (loopEndTime - loopStartTime);
            loopCount++;
            currentBeat += loopBeatCount;
            currentPosition = (float)(AudioSettings.dspTime - startTime);
            if (currentPosition < 0) currentPosition = 0;
            //Debug.Log($"LoopControl: Adjusted currentPosition={currentPosition}");
        }
    }

    private void CheckPosition()
    {
        if (musicSource.isPlaying)
        {
            currentPosition = (float)(AudioSettings.dspTime - startTime);
            if (currentPosition < 0) currentPosition = 0;

            int newBeat = (int)(currentPosition / noteInterval) + (loopCount * loopBeatCount);
            //Debug.Log($"CheckPosition: currentPosition={currentPosition}, newBeat={newBeat}, loopCount={loopCount}, loopBeatCount={loopBeatCount}");

            float beatStartTime = (newBeat - (loopCount * loopBeatCount)) * noteInterval;
            if (currentPosition >= beatStartTime && beatStartTime > lastBeatTrigger)
            {
                lastBeatTrigger = beatStartTime;
                Debug.Log($"OnBeatAction Invoked: newBeat={newBeat}");
                OnBeatAction?.Invoke(newBeat);
            }

            float midPointTime = beatStartTime + (noteInterval * 0.5f);
            if (currentPosition >= midPointTime && midPointTime > lastMidPointTrigger)
            {
                currentBeat = newBeat + 1;
                lastReportedBeat = currentBeat;
                lastMidPointTrigger = midPointTime;
                Debug.Log($"OnNextBeatAction Invoked: currentBeat={currentBeat}");
                OnNextBeatAction?.Invoke(currentBeat);
            }
        }
        else
        {
            currentPosition = 0f;
            Debug.Log("MusicSource is not playing!");
        }
    }

    public float GetTimingOffset()
    {
        if (!musicSource.isPlaying || currentPosition <= 0f)
        {
            return 0f;
        }

        int currentBeat = (int)(currentPosition / noteInterval) + (loopCount * loopBeatCount);
        float beatStartTime = (currentBeat - (loopCount * loopBeatCount)) * noteInterval;
        float offset = currentPosition - beatStartTime;

        if (offset > noteInterval / 2)
            offset -= noteInterval;
        else if (offset < -noteInterval / 2)
            offset += noteInterval;

        return offset;
    }
}