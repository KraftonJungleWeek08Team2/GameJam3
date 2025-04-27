using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public static Dictionary<int, BeatPattern> BeatPatternDatabase { get; private set; }

    private void Awake()
    {
        InitializeBeatPatternDatabase();
    }

    private void InitializeBeatPatternDatabase()
    {
        if (BeatPatternDatabase != null)
        {
            Debug.Log("BeatPatternDatabase already initialized.");
            return;
        }

        BeatPatternDatabase = new Dictionary<int, BeatPattern>();
        for (int i = 1; i <= 9; i++)
        {
            var pattern = Resources.Load<BeatPattern>($"KHW/BeatPatterns/BeatPattern {i}");
            if (pattern != null)
            {
                if (!BeatPatternDatabase.ContainsKey(i))
                {
                    BeatPatternDatabase.Add(i, pattern);
                    Debug.Log($"Loaded BeatPattern {i}");
                }
                else
                {
                    Debug.LogWarning($"Duplicate key {i} in BeatPatternDatabase.");
                }
            }
            else
            {
                Debug.LogWarning($"Failed to load BeatPattern {i} from Resources.");
            }
        }
    }

    public List<Note> GetNoteListBySlotInfo(int triggerBeat, SlotInfo slotInfo)
    {
        if (BeatPatternDatabase == null)
        {
            Debug.LogError("BeatPatternDatabase is not initialized! Initializing now.");
            InitializeBeatPatternDatabase();
        }

        int endOffset = 0;
        List<Note> generatedNoteList = new List<Note>();

        for (int i = 0; i < slotInfo.SlotCount; i++)
        {
            int slotNumber = slotInfo.GetValue(i);
            if (!BeatPatternDatabase.ContainsKey(slotNumber))
            {
                Debug.LogError($"BeatPattern for slot {slotNumber} not found!");
                continue;
            }

            BeatPattern beatPattern = BeatPatternDatabase[slotNumber];
            if (beatPattern == null || beatPattern.NoteList == null)
            {
                Debug.LogError($"BeatPattern {slotNumber} or its NoteList is null!");
                continue;
            }

            foreach (Note note in beatPattern.NoteList)
            {
                int adjustedBeat = triggerBeat + note.Beat + endOffset;
                generatedNoteList.Add(new Note(adjustedBeat, note.OffsetBeat, note.IsLast));
                Debug.Log($"KHW : {adjustedBeat} , {note.OffsetBeat}");
            }

            endOffset += Mathf.Max(0, beatPattern.endBeatOffset); // 음수 방지
        }

        if (generatedNoteList.Count > 0)
        {
            generatedNoteList[generatedNoteList.Count - 1] = new Note(
                generatedNoteList[generatedNoteList.Count - 1].Beat,
                generatedNoteList[generatedNoteList.Count - 1].OffsetBeat,
                true
            );
        }
        return generatedNoteList;
    }
}