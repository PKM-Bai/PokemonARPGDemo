using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace MGame.Save
{
    public class SavaLoadManager : Singleton<SavaLoadManager>
    {

        private List<ISaveable> saveableList = new List<ISaveable>();

        // 创建三条存档槽
        public List<DataSlot> dataSlots = new List<DataSlot>(new DataSlot[3]);

        private string jsonFolder;
        private int currentDataIndex;


        protected override void Awake()
        {
            base.Awake();
            jsonFolder = Application.persistentDataPath + "/SaveData/";
            ReadSaveData();
        }

        private void OnEnable()
        {
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }
        private void OnDisable()
        {
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void OnStartNewGameEvent(int index)
        {
            currentDataIndex = index;
        }

        /// <summary>
        ///* 注册可存储的数据
        /// </summary>
        public void RegisterSaveable(ISaveable saveable)
        {
            if (!saveableList.Contains(saveable))
                saveableList.Add(saveable);
        }

        /// <summary>
        ///* 读取游戏存档数据
        /// </summary>
        private void ReadSaveData()
        {
            if (Directory.Exists(jsonFolder))
            {
                for (int i = 0; i < dataSlots.Count; i++)
                {
                    var resultPath = jsonFolder + "Save" + i + "/data.json";
                    if (File.Exists(resultPath))
                    {
                        var stringData = File.ReadAllText(resultPath);
                        var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);
                        dataSlots[i] = jsonData;
                    }
                }
            }
        }

        public void Save(int index)
        {
            // Debug.Log(Application.persistentDataPath + "/SaveData");
            if (!Directory.Exists(jsonFolder + "Save" + index + ""))
                Directory.CreateDirectory(jsonFolder + "Save" + index);

            var saveFolder = jsonFolder + "Save" + index + "/";

            DataSlot data = new DataSlot();

            foreach (var saveable in saveableList)
            {
                data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());
            }
            dataSlots[index] = data;

            var resultPath = saveFolder + "data.json";
            // 将数据存储成json文件，格式为规则缩进
            var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);
            // 文件夹是否存在
            if (!File.Exists(resultPath))
            {
                Directory.CreateDirectory(saveFolder);
            }
            File.WriteAllText(resultPath, jsonData);
        }

        public void Load(int index)
        {
            currentDataIndex = index;

            var resultPath = jsonFolder + "Save" + index + "/data.json";
            var stringData = File.ReadAllText(resultPath);

            var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);

            foreach (var saveable in saveableList)
            {
                saveable.RestoreLoadData(jsonData.dataDict[saveable.GUID]);
            }
        }
    }


}
