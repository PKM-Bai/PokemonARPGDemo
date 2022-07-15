using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticPanel : MonoBehaviour
{
    private RoleInterface roleInterface;
    private PokemonManager pokemonManager;

    public StatUI statUI;
    public IndividualUI individualUI;
    public BasePointsUI basePointsUI;


    private void Awake()
    {
        roleInterface = FindObjectOfType<RoleInterface>();
        pokemonManager = FindObjectOfType<PokemonManager>();
    }

    private void OnEnable()
    {
        if(roleInterface.curSelectPokemon != null)
            OnShowPokemonStatistic(roleInterface.curSelectPokemon);
        EventHandler.SelectPokemonRole += OnShowPokemonStatistic;
    }

    private void OnDisable()
    {
        EventHandler.SelectPokemonRole -= OnShowPokemonStatistic;
    }


    private void OnShowPokemonStatistic(PokemonAttribute pokemon)
    {

        //* 能力值
        statUI.natureUI.natureName.text = pokemon.nature.ToString();
        string[] natureCorrection = pokemonManager.NatureCorrection(pokemon.nature);

        statUI.HP.text = "HP：";
        statUI.Attack.text = "攻击：";
        statUI.Defense.text = "防御：";
        statUI.SpecialAttack.text = "特攻：";
        statUI.SpecialDefense.text = "特防：";
        statUI.Speed.text = "速度：";
        if (natureCorrection[1] == null)
        {
            statUI.natureUI.natureStat.text = "(<color=green>" + natureCorrection[0] + "</color>)";
        }
        else
        {
            statUI.natureUI.natureStat.text = "(<color=red>" + natureCorrection[0] + "</color>," + "<color=blue>" + natureCorrection[1] + "</color>)";

            switch (natureCorrection[0])
            {
                case "攻击+":
                    statUI.Attack.text = "<color=red>攻击：";
                    break;
                case "防御+":
                    statUI.Defense.text = "<color=red>防御：";
                    break;
                case "特攻+":
                    statUI.SpecialAttack.text = "<color=red>特攻：";
                    break;
                case "特防+":
                    statUI.SpecialDefense.text = "<color=red>特防：";
                    break;
                case "速度+":
                    statUI.Speed.text = "<color=red>速度：";
                    break;
                default:

                    break;
            }

            switch (natureCorrection[1])
            {
                case "攻击-":
                    statUI.Attack.text = "<color=blue>攻击：";
                    break;
                case "防御-":
                    statUI.Defense.text = "<color=blue>防御：";
                    break;
                case "特攻-":
                    statUI.SpecialAttack.text = "<color=blue>特攻：";
                    break;
                case "特防-":
                    statUI.SpecialDefense.text = "<color=blue>特防：";
                    break;
                case "速度-":
                    statUI.Speed.text = "<color=blue>速度：";
                    break;
                default:
                    break;
            }
        }

        statUI.HP.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.Stat.HP.ToString();
        statUI.Attack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.Stat.Attack.ToString();
        statUI.Defense.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.Stat.Defense.ToString();
        statUI.SpecialAttack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.Stat.SpecialAttack.ToString();
        statUI.SpecialDefense.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.Stat.SpecialDefense.ToString();
        statUI.Speed.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.Stat.Speed.ToString();

        //* 个体值
        individualUI.HP.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.individual.HPIV.ToString();
        individualUI.Attack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.individual.AttackIV.ToString();
        individualUI.Defense.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.individual.DefenseIV.ToString();
        individualUI.SpecialAttack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.individual.SpecialAttackIV.ToString();
        individualUI.SpecialDefense.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.individual.SpecialDefenseIV.ToString();
        individualUI.Speed.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.individual.SpeedIV.ToString();

        //* 努力值
        basePointsUI.basePointsSum.text = "总和："+(pokemon.basePoints.HP + pokemon.basePoints.Attack + pokemon.basePoints.Defense + pokemon.basePoints.SpecialAttack + pokemon.basePoints.SpecialDefense + pokemon.basePoints.Speed).ToString() + "/510";
        basePointsUI.HP.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.basePoints.HP.ToString();
        basePointsUI.Attack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.basePoints.Attack.ToString();
        basePointsUI.Defense.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.basePoints.Defense.ToString();
        basePointsUI.SpecialAttack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.basePoints.SpecialAttack.ToString();
        basePointsUI.SpecialDefense.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.basePoints.SpecialDefense.ToString();
        basePointsUI.Speed.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.basePoints.Speed.ToString();

    }
}
