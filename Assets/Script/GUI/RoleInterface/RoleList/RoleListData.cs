using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoleListData : MonoBehaviour
{
    private RoleInterface roleInterface;
    // [HideInInspector]
    public PokemonAttribute thisPokemon;
    //* 每只宝可梦列表背景图片，第一只作为首发背景图和其他的不一样。 0：是没有宝可梦在列表中的显示，1：有宝可梦且未选中状态，2：有宝可梦为选中状态
    public Sprite[] firstBgImg;
    public Sprite[] prepareBgImg;

    public Image bgImg;
    public int roleNum;

    public Image pokemonImg;
    public Animator imgAnimator;
    public TextMeshProUGUI pokemonName;
    public Image sex;
    public RectTransform hpBlood;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI levelText;

    private int maxHPHeiget = 362;

    private bool isSelect;

    private void Awake()
    {
        roleInterface = FindObjectOfType<RoleInterface>();
        bgImg = transform.GetComponent<Image>();
        pokemonImg = transform.GetChild(0).GetComponent<Image>();
        imgAnimator = pokemonImg.GetComponent<Animator>();
        pokemonName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        sex = transform.GetChild(2).GetComponent<Image>();
        hpBlood = transform.GetChild(3).GetChild(0).GetComponent<RectTransform>();
        hpText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        levelText = transform.GetChild(5).GetComponent<TextMeshProUGUI>();

        if (roleNum == 1)
            SelectPokemonRole();
    }

    private void OnEnable()
    {
        if (roleNum == 1)
        {
            SelectPokemonRole();
        }
        else
        {
            CancelSelectPokemonRole();
        }

    }

    private void UpdataPokemonData(PokemonAttribute pokemon)
    {
        if (pokemon != null)
        {
            transform.GetComponent<Button>().interactable = true;
            if (!isSelect)
            {
                bgImg.sprite = roleNum == 1 ? firstBgImg[1] : prepareBgImg[1];
            }
            else
            {
                bgImg.sprite = roleNum == 1 ? firstBgImg[2] : prepareBgImg[2];
            }
            pokemonImg.sprite = pokemon.pokemonRoleImage.Count > 1 ? pokemon.sex == RoleSexType.雄性 ? pokemon.pokemonRoleImage[1] : pokemon.pokemonRoleImage[0] : pokemon.pokemonRoleImage[0];
            pokemonName.text = pokemon.pokmeonName;
            sex.sprite = pokemon.sex != RoleSexType.无性别 ? pokemon.sex == RoleSexType.雄性 ? PokemonManager.Instance.sexSprites[1] : PokemonManager.Instance.sexSprites[0] : null;
            hpBlood.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHPHeiget * (float)pokemon.currentHP / (float)pokemon.Stat.HP);
            hpText.text = pokemon.currentHP + "/" + pokemon.Stat.HP;
            levelText.text = "Lv." + pokemon.level;
        }
        else
        {
            transform.GetComponent<Button>().interactable = false;
            bgImg.sprite = roleNum == 1 ? firstBgImg[0] : prepareBgImg[0];
        }
    }

    public void SelectPokemonRole()
    {
        if (thisPokemon != null)
        {
            isSelect = true;
            roleInterface.curSelectPokemon = thisPokemon;
            UpdataPokemonData(thisPokemon);

            if (thisPokemon.pokemonRoleImage.Count > 1)
                imgAnimator.SetBool("sex", thisPokemon.sex == RoleSexType.雄性 ? true : false);
            imgAnimator.SetBool("isSelect", true);

            EventHandler.CallSelectPokemonRole(thisPokemon);
        }
    }

    public void CancelSelectPokemonRole()
    {
        if (thisPokemon != null)
        {
            isSelect = false;
            imgAnimator.SetBool("isSelect", false);
            UpdataPokemonData(thisPokemon);
        }
    }

}
