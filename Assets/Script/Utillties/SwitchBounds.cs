using UnityEngine;
using Cinemachine;

public class SwitchBounds : MonoBehaviour
{
    //TODO:切换场景》切换地图边界
    private void OnEnable()
    {
        EventHandler.FinishSceneLoadedEvent += SwitchConfinerShape;
    }

    private void OnDisable()
    {
        EventHandler.FinishSceneLoadedEvent -= SwitchConfinerShape;
    }

    private void SwitchConfinerShape()
    {
        if (GameObject.FindGameObjectWithTag("BoundsConfiner") != null)
        {
            PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

            CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
            confiner.m_BoundingShape2D = confinerShape;

            confiner.InvalidatePathCache(); // 每次在运行时切换边界 都需要清除一次缓存
        }
    }
}
