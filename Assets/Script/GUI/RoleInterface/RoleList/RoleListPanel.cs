using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class RoleListPanel : MonoBehaviour
{
    public List<GameObject> roleList;

    private void Awake() {
        roleList.Add(transform.GetChild(0).gameObject);
        roleList.Add(transform.GetChild(1).gameObject);
        roleList.Add(transform.GetChild(2).gameObject);
        roleList.Add(transform.GetChild(3).gameObject);
        roleList.Add(transform.GetChild(4).gameObject);
        roleList.Add(transform.GetChild(5).gameObject);

        UpdatePokemonTeamData();
    }
    private void OnEnable()
    {
        UpdatePokemonTeamData();
    }

    //* 更新宝可梦队伍信息
    private void UpdatePokemonTeamData()
    {
        if (PokemonManager.Instance.pokemonTeam.pokemons.Count != 0)
        {
            var team = PokemonManager.Instance.pokemonTeam.pokemons;
            for (int i = 0; i < team.Count; i++)
            {
                //? 计算能力值
                PokemonManager.Instance.CalculationStatistic(team[i]);
                if(i == 0)
                {
                    roleList[i].GetComponent<RoleListData>().thisPokemon = team[i];
                }
                else {
                    roleList[i].GetComponent<RoleListData>().thisPokemon = team[i];
                }

                //? 显示所有存在角色的分块区域
                for (int k = 0; k < roleList[i].transform.childCount; k++)
                {
                    roleList[i].transform.GetChild(k).gameObject.SetActive(true);
                }
            }
            
        }

    }

    
}
