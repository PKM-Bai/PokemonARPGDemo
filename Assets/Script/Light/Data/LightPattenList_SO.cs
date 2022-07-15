using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LightPattenList_SO", menuName = "Light/Light Patten")]
public class LightPattenList_SO : ScriptableObject {
    
    public List<LightDetalils> lightPattenList;

    /// <summary>
    ///* 根据季节和时间周期
    /// </summary>
    /// <param name="season">季节</param>
    /// <param name="lightShift">时间周期</param>
    /// <returns></returns>
    public LightDetalils GetLightDetalils(Season season, LightShift lightShift)
    {
        return lightPattenList.Find(l => l.season == season && l.lightShift == lightShift);
    }

}
[System.Serializable]
public class LightDetalils
{
    public Season season;
    public LightShift lightShift;
    public Color lightColor;
    public float lightAmount;
}