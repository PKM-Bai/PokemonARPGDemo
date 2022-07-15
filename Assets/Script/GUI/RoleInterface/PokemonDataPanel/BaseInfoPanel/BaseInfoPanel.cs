using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseInfoPanel : MonoBehaviour
{
    private RoleInterface roleInterface;
    public PokemonType_SO pokemonTypes;

    public PokemonBaseInfoUI baseInfoUI;
    public HoldingsUI holdingsUI;


    private void Awake()
    {
        roleInterface = FindObjectOfType<RoleInterface>();
    }

    private void OnEnable()
    {
        if(roleInterface.curSelectPokemon != null)
            OnShowPokemonBaseInfo(roleInterface.curSelectPokemon);
        EventHandler.SelectPokemonRole += OnShowPokemonBaseInfo;
    }

    private void OnDisable()
    {
        EventHandler.SelectPokemonRole -= OnShowPokemonBaseInfo;
    }


    private void OnShowPokemonBaseInfo(PokemonAttribute pokemon)
    {
        baseInfoUI.pokédexID.text = pokemon.pokédexID;
        baseInfoUI.pokemonName.text = pokemon.pokmeonName;

        var firstType = pokemonTypes.pokemonTypeList.Find(i => i.pokemonType == pokemon.firstType);
        baseInfoUI.firstType.GetComponent<Image>().sprite = firstType.typeImage;
        baseInfoUI.firstType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = firstType.pokemonType.ToString();
        if (pokemon.secondType != PokemonType.None)
        {
            var secondType = pokemonTypes.pokemonTypeList.Find(i => i.pokemonType == pokemon.secondType);
            baseInfoUI.secondType.GetComponent<Image>().sprite = secondType.typeImage;
            baseInfoUI.secondType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = secondType.pokemonType.ToString();
            baseInfoUI.secondType.SetActive(true);
        }
        else baseInfoUI.secondType.SetActive(false);

        baseInfoUI.currentExp.text = pokemon.currentExperience.ToString();
        baseInfoUI.expBar.value = pokemon.currentExperience;
        baseInfoUI.expBar.maxValue = pokemon.upLevelExperience;
        baseInfoUI.surplusExp.text = (pokemon.upLevelExperience - pokemon.currentExperience).ToString();


        //* 持有物
        if (pokemon.holdingsItem != null)
        {
            holdingsUI.gameObject.SetActive(true);
            // holdingsUI.itemImg.sprite = pokemon.holdingsItem.itemImage;
            // holdingsUI.itemName.text = pokemon.holdingsItem.itemName;
            // holdingsUI.itemInfo.text = pokemon.holdingsItem.itemInfo;
        }
        else holdingsUI.gameObject.SetActive(false);

    }
}
