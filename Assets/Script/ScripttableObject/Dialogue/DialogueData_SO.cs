using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData_SO", menuName = "Dialogue/DialogueData_SO")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces;
    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

    public QuestData_SO GetQuestData()
    {
        QuestData_SO data = null;
        foreach (var item in dialoguePieces)
        {
            if (item.quest != null)
                data = item.quest;
        }
        return data;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if(piece.id != string.Empty)
            {
                if (!dialogueIndex.ContainsKey(piece.id))
                    dialogueIndex.Add(piece.id, piece);
            }
        }
    }
#else
    private void Awake() {
        dialogueIndex.Clear();

        foreach (var piece in dialoguePieces)
        {
            if(!dialogueIndex.ContainsKey(piece.id))
                dialogueIndex.Add(piece.id, piece);
        }
    }
#endif

}