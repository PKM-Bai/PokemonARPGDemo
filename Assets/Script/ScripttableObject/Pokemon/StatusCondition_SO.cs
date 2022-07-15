using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "StatusCondition_SO", menuName = "Pokemon/StatusCondition_SO")]
public class StatusCondition_SO : ScriptableObject {
    
    [LabelText("状态")]public StatusConditionType status;
    [LabelText("持续时间")]public int duration;
    [LabelText("效果说明"), TextArea]public string statusInfo;
}