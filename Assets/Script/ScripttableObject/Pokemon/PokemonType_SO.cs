using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PokemonTypes_SO", menuName = "Pokemon/PokemonTypes_SO")]
public class PokemonType_SO : ScriptableObject {
    
    public List<PokemonTypeDetails> pokemonTypeList;

    public List<TypeIcon> typeIconList;
    

    //* 获取属性伤害倍率
    public float GetHurtMagnification(PokemonType atkType, PokemonType defType)
    {
        float typeHurt = 1f;
        PokemonTypeDetails typeDetails = pokemonTypeList.Find(t => t.pokemonType == atkType);
        // 攻击防御方的属性是否是效果绝佳
        foreach(PokemonType type in typeDetails.atkAdvantageType)
        {
            if(type == defType)
                typeHurt = 2f;
        }
        // 攻击防御方的属性是否是效果不好
        foreach(PokemonType type in typeDetails.atkInferiorityType)
        {
            if(type == defType)
                typeHurt = .5f;
        }
        // 攻击防御方的属性是否是无效
        foreach(PokemonType type in typeDetails.atkInvalidType)
        {
            if(type == defType)
                typeHurt = 0;
        }

        return typeHurt;
    }

    //* 获得属性图标
    public Sprite GetTypeIcon(PokemonType type)
    {
        return typeIconList.Find(t => t.type == type).icon;
    }

}

[System.Serializable]
public class TypeIcon
{
    public PokemonType type;
    public Sprite icon;
}


