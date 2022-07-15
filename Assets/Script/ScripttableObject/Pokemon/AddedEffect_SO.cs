using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AddedEffect_SO", menuName = "Pokemon/AddedEffect_SO")]
public class AddedEffect_SO : ScriptableObject {
    
    [LabelText("附加效果")] public AddedEffectType addedEffect;
    public StatisticType influenceStat;
    public StatusCondition_SO influenceStatus;
    public UseObjectType useObject;

}
