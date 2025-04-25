using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatPattern", menuName = "Scriptable Objects/BeatPattern")]
public class BeatPattern : ScriptableObject
{
    [SerializeField] public List<Note> noteList; // 직렬화 가능한 노트 리스트
    [SerializeField] public int endBeatOffset; 

    public List<Note> NoteList => noteList;
}
