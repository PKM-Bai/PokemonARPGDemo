using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInterface : MonoBehaviour
{

    private GameObject keyTit;
    private GameObject pokemonDataPanel;


    [Header("当前选择的宝可梦")]
    public PokemonAttribute curSelectPokemon;

    private void Awake()
    {
        keyTit = transform.GetChild(1).gameObject;

        curSelectPokemon = PokemonManager.Instance.pokemonTeam.pokemons[0];

    }



}
