using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "MusicInfo", menuName = "Scriptable Objects/MusicInfo")]
public class MusicInfo : ScriptableObject
{
    public AudioClip music; //노래의 클립.
    public string songName; //노래 제목.
    public int songIndex; //노래의 인덱스.
    public float startDelay; //노래 시작 트리거부터 노래가 재생되기 시작하는 시간의 간격.
    public int BPM; //노래의 BPM.
    public float loopStartTime; //후렴구 시작 시간.
    public float loopEndTime; //후렴구 끝 시간.
}
