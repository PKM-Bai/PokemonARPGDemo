using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyPokemon.Transition;

namespace MGame.Save
{
    public class  DataSlot
    {
        /// <summary>
        /// 存档数据
        /// </summary>
        /// <typeparam name="string">GUID</typeparam>
        /// <typeparam name="GameSaveData">该存档所有保存的数据</typeparam>
        /// <returns></returns>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();

        #region 显示UI进度详情
        public string DataTime
        {
            get
            {
                var key = TimeManager.Instance.GUID;

                if (dataDict.ContainsKey(key))
                {
                    var timeData = dataDict[key];
                    // return (Season)timeData.timeDict["gameSeason"] + timeData.timeDict["gameYear"] + "/" + timeData.timeDict["gameMonth"] + "/" + timeData.timeDict["gameDay"];
                    return timeData.timeDict["gameHour"] + ":" + timeData.timeDict["gameMinute"] + ":" + timeData.timeDict["gameSecode"];
                }
                else
                    return string.Empty;
            }
        }
        #endregion

        public string DataScene
        {
            get
            {
                var key = TransitionManager.Instance.GUID;
                if(dataDict.ContainsKey(key))
                {
                    var transitionData = dataDict[key];
                    return transitionData.dataSceneName switch
                    {
                        "T01.LittlerootTown" => "未白镇",
                        "H01.Pokemon Center" => "宝可梦中心",
                        "B01.Guide01" => "野外1",
                        "B01.Guide02" => "野外2",
                        _ => string.Empty
                    };
                }
                return string.Empty;
            }
        }
    }
}