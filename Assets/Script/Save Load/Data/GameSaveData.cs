using System.Collections;
using System.Collections.Generic;
using MyPokemon.Inventory;
namespace MGame.Save
{
    [System.Serializable]
    public class GameSaveData
    {
        public string dataSceneName;
        public TeleportType dataSceneType;
        /// <summary>
        /// 存储人物名和人物坐标
        /// </summary>
        public Dictionary<string, SerializableVector3> characterPosDict;
        /// <summary>
        /// 存储场景物品
        /// </summary>
        public Dictionary<string, List<SceneItem>> sceneItemDict;
        /// <summary>
        /// 已接取已完成的任务
        /// </summary>
        public Dictionary<string, List<TaskProgress>> questTaskDict;
        /// <summary>
        /// 小道消息
        /// </summary>
        public Dictionary<string, List<Information>> informationDataDict;
        /// <summary>
        /// 时间
        /// </summary>
        public Dictionary<string, int> timeDict;

        /// <summary>
        /// 钱
        /// </summary>
        public Dictionary<string, long> moneyDict;
        /// <summary>
        /// 玩家背包
        /// </summary>
        public Dictionary<string, List<ItemStack>> playerBagDict;
        /// <summary>
        /// 存储队伍中所有宝可梦的数据
        /// </summary>
        public Dictionary<string, List<PokemonDetails>> pokemonTeamDict;
        // public PokemonTeam_SO pokemonTeamDict;

        //*NPC
    }
}
