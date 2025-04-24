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
            Debug.LogError($"MusicInfo {index} not found!");
            return;
        }

        musicSource.clip = currentMusicInfo.music;
        if (musicSource.clip == null)
        {
            Debug.LogError("Music clip is null!");
            return;
        }

        startDelay = Mathf.Max(0, currentMusicInfo.startDelay);
        noteInterval = currentMusicInfo.BPM > 0 ? 60f / currentMusicInfo.BPM : 1f;
        loopStartTime = currentMusicInfo.loopStartTime;
        loopEndTime = currentMusicInfo.loopEndTime;

        if (loopEndTime <= loopStartTime)
        {
            Debug.LogWarning("Invalid loop times: loopEndTime <= loopStartTime. Disabling loop.");
            loopEndTime = musicSource.clip.length;
            loopStartTime = 0;
        }

        loopStartSamples = (int)(loopStartTime * musicSource.clip.frequency);
        loopEndSamples = (int)(loopEndTime * musicSource.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;

        loopBeatCount = Mathf.FloorToInt((loopEndTime - loopStartTime) / noteInterval);
        Debug.Log($"SetMusic: clip={musicSource.clip.name}, noteInterval={noteInterval}, loopBeatCount={loopBeatCount}, loopStartTime={loopStartTime}, loopEndTime={loopEndTime}");
    }

    public void StartMusic()
    {
        if (musicSource.clip == null)
        {
            Debug.LogError("Cannot start music: Audio clip is null!");
            return;
        }
        startTime = AudioSettings.dspTime + startDelay;
        musicSource.PlayScheduled(startTime);
        Debug.Log($"StartMusic: startTime={startTime}, clip={musicSource.clip.name}, isPlaying={musicSource.isPlaying}");
    }

    private void LoopControl()
    {
        if (musicSource.timeSamples >= loopEndSamples)
        {
            // 루프 발생
            loopCount++;
            musicSource.timeSamples -= loopLengthSamples;
            startTime += (loopEndTime - loopStartTime);
            currentPosition = (float)(AudioSettings.dspTime - startTime);

            // 루프 후 비트 상태 동기화
            currentBeat = loopCount * loopBeatCount;
            lastBeatTrigger = -1f; // 비트 트리거 초기화
            lastMidPointTrigger = -1f; // 중간점 트리거 초기화

            // 루프 시작 지점에서 첫 비트 강제 호출
            OnBeatAction?.Invoke(currentBeat);
            OnNextBeatAction?.Invoke(currentBeat + 1);

            Debug.Log($"Music Looped! Loop Count: {loopCount}, timeSamples: {musicSource.timeSamples}, currentBeat: {currentBeat}, currentPosition: {currentPosition}");
        }
    }

    private void CheckPosition()
    {
        if (!musicSource.isPlaying)
        {
            currentPosition = 0f;
            Debug.Log("MusicSource is not playing!");
            return;
        }

        currentPosition = (float)(AudioSettings.dspTime - startTime);
        if (currentPosition < 0) currentPosition = 0;

        // 루프 내에서의 상대적 시간 계산
        float relativePosition = currentPosition + (loopCount * (loopEndTime - loopStartTime));
        int newBeat = Mathf.FloorToInt(relativePosition / noteInterval);

        // 비트 시작 시간
        float beatStartTime = (newBeat * noteInterval) - (loopCount * (loopEndTime - loopStartTime));
        if (relativePosition >= newBeat * noteInterval && beatStartTime > lastBeatTrigger)
        {
            lastBeatTrigger = beatStartTime;
            OnBeatAction?.Invoke(newBeat);
        }

        // 비트 중간점 (다음 비트 예측)
        float midPointTime = beatStartTime + (noteInterval * 0.5f);
        if (relativePosition >= (newBeat * noteInterval + noteInterval * 0.5f) && midPointTime > lastMidPointTrigger)
        {
            currentBeat = newBeat + 1;
            lastReportedBeat = currentBeat;
            lastMidPointTrigger = midPointTime;
            OnNextBeatAction?.Invoke(currentBeat);
        }
    }

    public float GetTimingOffset()
    {
        if (!musicSource.isPlaying || currentPosition <= 0f)
        {
            return 0f;
        }

        float relativePosition = currentPosition + (loopCount * (loopEndTime - loopStartTime));
        int currentBeat = Mathf.FloorToInt(relativePosition / noteInterval);
        float beatStartTime = (currentBeat * noteInterval) - (loopCount * (loopEndTime - loopStartTime));
        float offset = currentPosition - beatStartTime;

        if (offset > noteInterval / 2)
            offset -= noteInterval;
        else if (offset < -noteInterval / 2)
            offset += noteInterval;

        return offset;
    }
}