using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InformationData_SO", menuName = "Quest/InformationData")]
public class InformationData_SO : ScriptableObject
{
    public List<Information> informations;

    //* 获取当前列表中存在的城镇类型
    public List<TownNameEnum> GetTownNameTypeList()
    {
        List<TownNameEnum> townNames = new List<TownNameEnum>();
        if (informations != null)
        {
            var townNameTemp = new List<TownNameEnum>();
            foreach (var info in informations)
            {
                townNameTemp.Add(info.townName);
            }

            //* 列表去重
            for (int i = 0; i < townNameTemp.Count; i++)
            {
                if (i == 0)
                    townNames.Add(townNameTemp[i]);
                else if (!townNames.Contains(townNameTemp[i]))
                    townNames.Add(townNameTemp[i]);
            }
        }
        return townNames;
    }

    //* 使用类型返回该城镇下所有小道消息
    public List<string> GetInformationByTownType(TownNameEnum towntype)
    {
        List<string> infoList = new List<string>();
        foreach (var item in informations)
        {
            if (item.townName == towntype)
                infoList.Add(item.info);
        }
        return infoList;
    }

    //* 返回该信息是否已经存在
    public bool IsContainsInfo(Information information)
    {
        return informations.Any(i => i.info == information.info);
    }


}
