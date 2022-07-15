using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MGame.Save;

namespace MyPokemon.Transition
{
    public class TransitionManager : Singleton<TransitionManager>, ISaveable
    {
        [Header("当前场景类型")]
        public TeleportType localSceneType;
        public GameObject townSceneBase;
        [SceneName] public string startSceneName = string.Empty;
        // [SceneName] public string[] townBaseScenes;
        [SceneName] public string battleBaseScenes;
        public CanvasGroup loadCanvasGroup;
        public CanvasGroup gameOverCanvasGroup;
        bool isFade;

        public string GUID => GetComponent<DataGUID>().guid;

        protected override void Awake()
        {
            base.Awake();
            localSceneType = TeleportType.Menu;
            //* 游戏一开始首先加载开始游戏菜单并设置激活状态
            StartCoroutine(LoadSceneSetActive("01.StartMenuUI"));

            // 游戏帧率
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
            EventHandler.LoadStartMenuSceneEvent += OnLoadStartMenuSceneEvent;
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
            EventHandler.GameOverEvent += OnGameOverEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
            EventHandler.LoadStartMenuSceneEvent -= OnLoadStartMenuSceneEvent;
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
            EventHandler.GameOverEvent -= OnGameOverEvent;
        }

        private void Start()
        {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }

        private void OnStartNewGameEvent(int index)
        {
            StartCoroutine(Transition("T01.LittlerootTown", new Vector3(), TeleportType.TownScene));
        }

        private void OnTransitionEvent(string sceneName, Vector3 position, TeleportType targetScene)
        {

            if (!isFade)
                StartCoroutine(Transition(sceneName, position, targetScene));
        }

        private void OnLoadStartMenuSceneEvent(string sceneName)
        {
            StartCoroutine(LoadStartMenuScene(sceneName));
        }

        private void OnGameOverEvent()
        {
            StartCoroutine(Fade(gameOverCanvasGroup, 1));
        }

        /// <summary>
        ///* 场景切换
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="targetPosition">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPosition, TeleportType targetSceneType)
        {
            yield return Fade(loadCanvasGroup, 1);

            EventHandler.CallBeforeSceneUnloadEvent();

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());  // 卸载当前正在激活状态的场景

            if (localSceneType != targetSceneType)
            {
                //* 加载城镇基础场景
                if (targetSceneType == TeleportType.TownScene && localSceneType != TeleportType.Menu)
                {
                    townSceneBase.SetActive(true);
                    // 卸载战斗基础场景
                    yield return SceneManager.UnloadSceneAsync(battleBaseScenes);
                }
                //* 加载战斗基础场景
                if (targetSceneType == TeleportType.BattleScene)
                {
                    townSceneBase.SetActive(false);
                    yield return SceneManager.LoadSceneAsync(battleBaseScenes, LoadSceneMode.Additive);
                }
                localSceneType = targetSceneType;
            }

            // Debug.Log("加载传送场景");
            yield return LoadSceneSetActive(sceneName);

            if (targetPosition != new Vector3())
                EventHandler.CallMoveToPosition(targetPosition);

            EventHandler.CallFinishSceneLoadedEvent();      // 场景加载完毕

            gameOverCanvasGroup.blocksRaycasts = false;
            gameOverCanvasGroup.alpha = 0;

            yield return Fade(loadCanvasGroup, 0);

            EventHandler.CallAfterSceneLoadedEvent();       // 场景加载完毕之后
        }

        private IEnumerator LoadStartMenuScene(string sceneName)
        {
            EventHandler.CallBeforeSceneUnloadEvent();

            townSceneBase.SetActive(true);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());  // 卸载当前正在激活状态的场景

            if (localSceneType == TeleportType.BattleScene)
                // 卸载战斗基础场景
                yield return SceneManager.UnloadSceneAsync(battleBaseScenes);

            localSceneType = TeleportType.Menu;

            yield return LoadSceneSetActive(sceneName);

            gameOverCanvasGroup.blocksRaycasts = false;
            gameOverCanvasGroup.alpha = 0;
            EventHandler.CallFinishSceneLoadedEvent();
        }

        /// <summary>
        ///* 加载场景并设置为激活状态
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);     // 加载场景方式：叠加场景

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);

            EventHandler.CallFinishSceneLoadedEvent();      // 场景加载完毕
            EventHandler.CallAfterSceneLoadedEvent();
        }

        /// <summary>
        ///* 淡入淡出加载场景
        /// </summary>
        /// <param name="targetAlpha">修改的透明度 1是黑 0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(CanvasGroup fadeCanvasGroup, float targetAlpha)
        {
            if (fadeCanvasGroup.name == "GameOver Panel")
                yield return new WaitForSeconds(1);
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.loadFadeDuration;

            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            if (fadeCanvasGroup.name != "GameOver Panel")
                fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }

        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.dataSceneName = SceneManager.GetActiveScene().name;
            saveData.dataSceneType = localSceneType;
            return saveData;
        }

        public void RestoreLoadData(GameSaveData saveData)
        {
            // 加载游戏进度场景
            StartCoroutine(Transition(saveData.dataSceneName, new Vector3(), saveData.dataSceneType));
        }
    }
}