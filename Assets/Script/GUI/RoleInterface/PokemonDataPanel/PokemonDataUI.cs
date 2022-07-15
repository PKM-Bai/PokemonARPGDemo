using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDataUI : MonoBehaviour
{
    public BaseInfoPanel baseInfoPanel;
    public StatisticPanel statisticPanel;
    public SkillPanel skillPanel;

    public void Init()
    {
        baseInfoPanel.gameObject.SetActive(true);
        statisticPanel.gameObject.SetActive(false);
        skillPanel.gameObject.SetActive(false);

        skillPanel.Init();
    }
}
