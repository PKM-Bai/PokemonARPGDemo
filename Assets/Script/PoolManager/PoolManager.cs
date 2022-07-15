using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefabs;
    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();

    private Queue<GameObject> soundQueue = new Queue<GameObject>();

    private void Start()
    {
        CreatePool();
    }

    private void OnEnable()
    {
        EventHandler.ParticleEffectEvent += OnParticleEffectEvent;
        EventHandler.InitSoundEffect += OnInitSoundEffect;
    }

    private void OnDisable()
    {
        EventHandler.ParticleEffectEvent -= OnParticleEffectEvent;
        EventHandler.InitSoundEffect -= OnInitSoundEffect;
    }

    /// <summary>
    ///* 生成对象池
    /// </summary>
    private void CreatePool()
    {
        foreach (GameObject item in poolPrefabs)
        {
            var parent = new GameObject(item.name).transform;
            parent.SetParent(transform);

            var newPool = new ObjectPool<GameObject>
            (
                () => Instantiate(item, parent),
                e => { e.SetActive(true); },
                e => { e.SetActive(false); },
                e => { Destroy(e); }
            );
            poolEffectList.Add(newPool);
        }
    }

    private void OnParticleEffectEvent(ParticleEffectType effectType, Vector3 pos)
    {
        //? 根据特效补全
        // 特效类型生成对应的对象池
        ObjectPool<GameObject> objPool = effectType switch
        {
            ParticleEffectType.Footstep => poolEffectList[0],
            _ => null
        };

        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        StartCoroutine(ReleaseRoutine(objPool, obj));
    }

    private IEnumerator ReleaseRoutine(ObjectPool<GameObject> objPool, GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        objPool.Release(obj);
    }

    // private void InitSoundEffect(SoundDetails soundDetails)
    // {
    //     ObjectPool<GameObject> pool = poolEffectList[1];
    //     var obj = pool.Get();
    //     obj.GetComponent<Sound>().SetSound(soundDetails);
    //     StartCoroutine(DisableRoutine(pool, obj, soundDetails));
    // }

    // private IEnumerator DisableRoutine(ObjectPool<GameObject> objPool, GameObject obj, SoundDetails soundDetails)
    // {
    //     yield return new WaitForSeconds(soundDetails.soundCilp.length);
    //     objPool.Release(obj);
    // }

    private void CreateSoundPool()
    {
        var parent = new GameObject(poolPrefabs[1].name).transform;
        parent.SetParent(transform);

        for (int i = 0; i < 20; i++)
        {
            GameObject newObj = Instantiate(poolPrefabs[1], parent);
            newObj.SetActive(false);
            soundQueue.Enqueue(newObj);
        }
    }

    private GameObject GetPoolObject()
    {
        if (soundQueue.Count < 2)
            CreateSoundPool();
        return soundQueue.Dequeue();
    }

    private void OnInitSoundEffect(SoundDetails soundDetails)
    {
        var obj = GetPoolObject();
        obj.GetComponent<Sound>().SetSound(soundDetails);
        obj.SetActive(true);
        StartCoroutine(DisableSound(obj, soundDetails.soundCilp.length));
    }

    private IEnumerator DisableSound(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
        soundQueue.Enqueue(obj);
    }

}
