namespace MGame.Save
{
    public interface ISaveable
    {
        string GUID { get; }
        void RegisterSaveable()
        {
            SavaLoadManager.Instance.RegisterSaveable(this);
        }

        /// <summary>
        /// 生成并保存数据
        /// </summary>
        /// <returns></returns>
        GameSaveData GenerateSaveData();

        /// <summary>
        /// 恢复加载数据
        /// </summary>
        void RestoreLoadData(GameSaveData saveData);
    }
}
