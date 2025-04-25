using System.Collections.Generic;
using UnityEngine;
public class PatternManager : MonoBehaviour
{
    public static Dictionary<int, BeatPattern> BeatPatternDatabase;


    void Start()
    {
        
    }

    private void InitializeBeatPatternDatabase()
    {
        BeatPatternDatabase.Add(1, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 1") );
        BeatPatternDatabase.Add(2, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 2") );
        BeatPatternDatabase.Add(3, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 3") );
        BeatPatternDatabase.Add(4, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 4") );
        BeatPatternDatabase.Add(5, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 5") );
        BeatPatternDatabase.Add(6, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 6") );
        BeatPatternDatabase.Add(7, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 7") );
        BeatPatternDatabase.Add(8, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 8") );
        BeatPatternDatabase.Add(9, (BeatPattern)Resources.Load("KHW/BeatPatterns/BeatPattern 9") );
    }

    /// <summary>
    /// beatMargin == 0 -> UI에서 소환
    /// </summary>
    /// <param name="endBeat"></param>
    /// <param name="slotNumber"></param>
    /// <returns></returns>
    public List<Note> GetNoteListBySlotInfo(int TriggerBeat, SlotInfo slotInfo)
    {
        int endOffset = 0;
        List<Note> generatedNoteList = new List<Note>();

        for(int i = 0 ; i < slotInfo.SlotCount; i++)
        {
            int slotNumber = slotInfo.GetValue(i); //슬롯 숫자 획득.

            BeatPattern beatPattern = BeatPatternDatabase[slotNumber];
            foreach(Note note in beatPattern.NoteList)
            {
                generatedNoteList.Add(new Note(TriggerBeat + note.Beat + endOffset, note.OffsetBeat));
            }

            endOffset += beatPattern.endBeatOffset;
        }

        return generatedNoteList;
    }
}
