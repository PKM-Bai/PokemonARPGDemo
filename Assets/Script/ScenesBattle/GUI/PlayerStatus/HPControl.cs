using UnityEngine;
using TMPro;

public class HPControl : MonoBehaviour
{
    [SerializeField] Transform foreMask;
    [SerializeField] Transform backMask;
    [SerializeField] PokemonAttribute playerAttribute;

    private const float xStart = 1.75f; // 生命值100%的位置
    private float lastOffset;           // 扣血之前的位置
    public TextMeshProUGUI HPbarText;

    private void OnEnable() {
        EventHandler.FinishSceneLoadedEvent += InitHPControl;
    }

    private void OnDisable() {
        EventHandler.FinishSceneLoadedEvent -= InitHPControl;
    }

    private void Update() {
        HPbarText.text = playerAttribute.currentHP + " / " + playerAttribute.Stat.HP;
        float present = (float)playerAttribute.currentHP / (float)playerAttribute.Stat.HP;    // 生命百分比
        float offset;
        float difference;
        float speed;

        offset = present * xStart;
        foreMask.localPosition = new Vector2(offset, foreMask.localPosition.y);
    
        if(lastOffset != offset)
        {
            difference = lastOffset - offset;
            speed = difference / 0.8f;
            if (backMask.localPosition.x > offset)
            {
                backMask.localPosition -= new Vector3(Time.deltaTime * speed, 0, 0);
            }
            else if (backMask.localPosition.x <= offset)
            {
                backMask.localPosition = new Vector3(offset, backMask.localPosition.y, 0);
                lastOffset = offset;
            }
        }
        
    }

     private void InitHPControl()
    {
        playerAttribute = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().playerAttribute;
        lastOffset = backMask.localPosition.x;
    }
}
