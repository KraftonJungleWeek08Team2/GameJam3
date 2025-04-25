using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private int musicIndex;
    private MusicInfo currentMusicInfo;

    private AudioSource musicSource;
    private float startDelay;
    public float beatInterval; //60 / RPM의 값. 비트간 사이 시간.
    private float loopStartTime;
    private float loopEndTime;

    private int loopStartSamples; //48000을 노래 시작시간에 곱한 비트레이트.
    private int loopEndSamples; //48000을 노래 종료시간에 곱한 비트레이트.
    private int loopLengthSamples; //루프 구간의 비트레이트.

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
        //SetMusic -> StartMusic.
        SetMusic(musicIndex); //노래를 세팅. 함수 내에서 지연시간 이후 재생. 
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

        startDelay = Mathf.Max(0, currentMusicInfo.startDelay); //MusicInfo 의 그것이 음수면 0.
        beatInterval = currentMusicInfo.BPM > 0 ? 60f / currentMusicInfo.BPM : 1f; //60을 BPM으로 나눈 비트간 사이 인터벌.
        loopStartTime = currentMusicInfo.loopStartTime;
        loopEndTime = currentMusicInfo.loopEndTime;

        if (loopEndTime <= loopStartTime) //MusicInfo의 루프종료시간이 시작시간보다 앞...
        {
            Debug.LogWarning("Invalid loop times: loopEndTime <= loopStartTime. Disabling loop.");
            loopEndTime = musicSource.clip.length;
            loopStartTime = 0;
        }

        loopStartSamples = (int)(loopStartTime * musicSource.clip.frequency); //48000을 노래 시작시간에 곱한 비트레이트.
        loopEndSamples = (int)(loopEndTime * musicSource.clip.frequency); //48000을 노래 종료시간에 곱한 비트레이트.
        loopLengthSamples = loopEndSamples - loopStartSamples; //루프 구간의 비트레이트.

        loopBeatCount = Mathf.FloorToInt((loopEndTime - loopStartTime) / beatInterval); //루프 사이의 비트 갯수? 왜필요하지....
        Debug.Log($"SetMusic: clip={musicSource.clip.name}, noteInterval={beatInterval}, loopBeatCount={loopBeatCount}, loopStartTime={loopStartTime}, loopEndTime={loopEndTime}");

        //노래시작.
        StartMusic();
    }

/// <summary>
/// Start Time 이후 노래 재생.
/// </summary>
    public void StartMusic()
    {
        if (musicSource.clip == null) ///클립이 null.
        {
            Debug.LogError("Cannot start music: Audio clip is null!");
            return;
        }
        startTime = AudioSettings.dspTime + startDelay; //시작 시간 = 현재 오디오시스템 시간 + 시작 딜레이 시간.
        musicSource.PlayScheduled(startTime);
        Debug.Log($"StartMusic: startTime={startTime}, clip={musicSource.clip.name}, isPlaying={musicSource.isPlaying}");
    }

    private void LoopControl()
    {
        if (musicSource.timeSamples >= loopEndSamples)
        {
            // 루프 발생
            loopCount++;
            musicSource.timeSamples -= loopLengthSamples; //음악의 timesample을 감소시켜서 이동.
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
        int newBeat = Mathf.FloorToInt(relativePosition / beatInterval);

        // 비트 시작 시간
        float beatStartTime = (newBeat * beatInterval) - (loopCount * (loopEndTime - loopStartTime));
        if (relativePosition >= newBeat * beatInterval && beatStartTime > lastBeatTrigger)
        {
            lastBeatTrigger = beatStartTime;
            OnBeatAction?.Invoke(newBeat);
        }

        // 비트 중간점 (다음 비트 예측)
        float midPointTime = beatStartTime + (beatInterval * 0.5f);
        if (relativePosition >= (newBeat * beatInterval + beatInterval * 0.5f) && midPointTime > lastMidPointTrigger)
        {
            currentBeat = newBeat + 1;
            lastReportedBeat = currentBeat;
            lastMidPointTrigger = midPointTime;
            OnNextBeatAction?.Invoke(currentBeat);
        }
    }

/// <summary>
/// 현재비트와의 시간적 차이를 반환. 
/// </summary>
/// <returns></returns>
    public float GetTimingOffset()
    {
        if (!musicSource.isPlaying || currentPosition <= 0f)
        {
            return 0f;
        }

        float relativePosition = currentPosition + (loopCount * (loopEndTime - loopStartTime));
        int currentBeat = Mathf.FloorToInt(relativePosition / beatInterval);
        float beatStartTime = (currentBeat * beatInterval) - (loopCount * (loopEndTime - loopStartTime));
        float offset = currentPosition - beatStartTime;

        if (offset > beatInterval / 2)
            offset -= beatInterval;
        else if (offset < -beatInterval / 2)
            offset += beatInterval;

        return offset;
    }
}