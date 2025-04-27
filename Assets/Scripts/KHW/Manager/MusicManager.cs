using System;
using UnityEngine;

/// <summary>
/// 노래를 설정하고 BPM에 따른 비트가 발생할 때 마다 액션을 트리거하는 클래스입니다.
/// </summary>
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Song's Default Information")]
    private MusicInfo currentMusicInfo; //Set Music()으로 저장되는 노래정보를 담는 클래스.
    [SerializeField] private int musicIndex;
    [SerializeField] private AudioSource musicSource;
    private float startDelay; // 노래 세팅부터 시작까지 걸리는 시간 float.
    public float beatInterval; // 60 / RPM의 값. 비트간 사이 시간.
    private float loopStartTime; // 원본 노래파일의 반복구간 시작 시간 float.
    private float loopEndTime; // 원본 노래파일의 반복구간 종료 시간 float.

    [Header("Int Time Samples")]
    private int loopStartSamples; //48000을 노래 시작시간에 곱한 비트레이트.
    private int loopEndSamples; //48000을 노래 종료시간에 곱한 비트레이트.
    private int loopLengthSamples; //루프 구간의 비트레이트.

    [Header("Playing Information")]
    private double startTime; //현재 루프 기준 노래의 시작 시간....
    public int currentBeat { get; private set; }
    public float currentPosition;
    private float lastBeatTrigger = -1f;
    private int loopCount = 0;
    private int loopBeatCount;
    [Header("Actions")]

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
        CheckPosition();
        LoopControl();
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
        loopLengthSamples = loopEndSamples - loopStartSamples; //루프 구간의 비트레이트. 이 값을 비트레이트에서 빼주는 것.

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

    /// <summary>
    /// 프레임마다 루프 지점을 넘겼는지 계산하고 넘었으면 StartTimeUpdate.
    /// </summary>
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
            //currentBeat = loopCount * loopBeatCount;
            lastBeatTrigger = -1f; // 비트 트리거 초기화

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
            currentBeat = newBeat;
        }
    }

    /// <summary>
    /// 비트와의 타이밍 오차를 계산. offsetFromCurrentBeat을 적용해 비트에서 떨어진 위치를 기준으로 오차를 반환.
    /// </summary>
    /// <param name="offsetFromCurrentBeat">비트 시작으로부터의 오프셋(초). 양수면 비트 이후, 음수면 비트 이전.</param>
    /// <returns>노트의 예상 타이밍과의 오차(초). 양수는 늦음, 음수는 빠름.</returns>
    public float GetTimingOffset(float beat)
    {
        if (!musicSource.isPlaying || currentPosition <= 0f)
        {
            return 0f;
        }

        // 현재 음악의 상대적 위치 (루프를 고려)
        float relativePosition = currentPosition + (loopCount * (loopEndTime - loopStartTime));
        
        // 현재 비트 계산
        int currentBeat = Mathf.FloorToInt(relativePosition / beatInterval);
        float noteExpectedTime = beat * beatInterval;
        
        // 현재 위치와 노트 예상 타이밍의 오차
        float offset = relativePosition - noteExpectedTime;

        return offset;
    }
}