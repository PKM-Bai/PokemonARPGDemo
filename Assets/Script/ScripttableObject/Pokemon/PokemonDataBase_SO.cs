using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonDataBase_SO", menuName = "Pokemon/PokemonDataBase", order = 0)]
public class PokemonDataBase_SO : ScriptableObject
{
    public List<PokemonAttribute> pokemons;

    public PokemonAttribute GetPokemon(string PUID)
    {
        return pokemons.Find(p => p.PUID == PUID);
    }
}
