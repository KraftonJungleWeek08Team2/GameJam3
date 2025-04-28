using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public Dictionary<string, BeatPattern> BeatPatternDatabase { get; private set; }

    public int stageIndex = 1; //1 -> 1st stage, 2 -> 2nd stage.
    private void Awake()
    {
        Debug.Log($"index : {stageIndex}");
        InitializeBeatPatternDatabase();
    }

    private void InitializeBeatPatternDatabase()
    {
        if (BeatPatternDatabase != null)
        {
            Debug.Log("BeatPatternDatabase already initialized.");
            return;
        }

        BeatPatternDatabase = new Dictionary<string, BeatPattern>();
        string[] enemyTypes = { "Normal", "Elite", "Boss" };
        string[] enemyTypePrefixes = { "N", "E", "B" }; // 파일명 접두사: Normal -> N, Elite -> E, Boss -> B

        foreach (var (enemyType, prefix) in enemyTypes.Zip(enemyTypePrefixes, (a, b) => (a, b)))
        {
            for (int i = 1; i <= 9; i++)
            {
                // Resources 경로: Stage{stageIndex}/{enemyType}/BeatPattern_{prefix}{stageIndex}_{i}
                //Assets/Resources/KHW/BeatPatterns/Stage1/Normal/BeatPattern_N1_1.asset
                string resourcePath = $"KHW/BeatPatterns/Stage{stageIndex}/{enemyType}/BeatPattern_{prefix}{stageIndex}_{i}";
                string key = $"{enemyType}_{stageIndex}_{i}"; // 키 형식: Normal_1_1, Elite_1_2 등

                var pattern = Resources.Load<BeatPattern>(resourcePath);
                if (pattern != null)
                {
                    if (!BeatPatternDatabase.ContainsKey(key))
                    {
                        BeatPatternDatabase.Add(key, pattern);
                        Debug.Log($"Loaded BeatPattern {key} from {resourcePath}");
                    }
                    else
                    {
                        Debug.LogWarning($"Duplicate key {key} in BeatPatternDatabase.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Failed to load BeatPattern from {resourcePath}");
                }
            }
        }
    }

    public List<Note> GetNoteListBySlotInfo(int triggerBeat, SlotInfo slotInfo)
    {
        EnemyType currentEnemyType = Managers.TurnManager.CurrentEnemy.enemyType;

        if (BeatPatternDatabase == null)
        {
            Debug.LogError("BeatPatternDatabase is not initialized! Initializing now.");
            InitializeBeatPatternDatabase();
        }

        // EnemyType을 문자열로 변환 (Normal, Elite, Boss)
        string enemyTypeStr = currentEnemyType.ToString();
        int endOffset = 0;
        List<Note> generatedNoteList = new List<Note>();

        for (int i = 0; i < slotInfo.SlotCount; i++)
        {
            int slotNumber = slotInfo.GetValue(i);
            string key = $"{enemyTypeStr}_{stageIndex}_{slotNumber}"; // 예: Normal_1_1, Elite_1_2, Boss_1_3

            if (!BeatPatternDatabase.ContainsKey(key))
            {
                Debug.LogError($"BeatPattern for key {key} not found!");
                continue;
            }

            BeatPattern beatPattern = BeatPatternDatabase[key];
            if (beatPattern == null || beatPattern.NoteList == null)
            {
                Debug.LogError($"BeatPattern {key} or its NoteList is null!");
                continue;
            }

            foreach (Note note in beatPattern.NoteList)
            {
                int adjustedBeat = triggerBeat + note.Beat + endOffset;
                generatedNoteList.Add(new Note(adjustedBeat, note.OffsetBeat, note.IsLast));
                Debug.Log($"Generated Note: Beat={adjustedBeat}, OffsetBeat={note.OffsetBeat}, Key={key}");
            }

            endOffset += Mathf.Max(0, beatPattern.endBeatOffset); // 음수 방지
        }

        // 마지막 노트 플래그 설정
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