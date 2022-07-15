using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    public LightPattenList_SO lightData;
    public Light2D currentLight;
    public LightDetalils currentLightDetalis;

    private void Awake()
    {
        currentLight = GetComponent<Light2D>();
    }


    // 切换灯光 
    public void ChangeLightShift(Season season, LightShift lightShift, float timeDifference)
    {
        currentLightDetalis = lightData.GetLightDetalils(season, lightShift);
        if(timeDifference < Settings.lightChangeDuration)
        {
            var colorOffst = (currentLightDetalis.lightColor - currentLight.color) / Settings.lightChangeDuration * timeDifference;
            currentLight.color += colorOffst;
            // 实时切换灯光颜色的过渡颜色
            DOTween.To(() => currentLight.color, c => currentLight.color = c, currentLightDetalis.lightColor, Settings.lightChangeDuration - timeDifference);
            DOTween.To(() => currentLight.intensity, i => currentLight.intensity = i, currentLightDetalis.lightAmount, Settings.lightChangeDuration - timeDifference);
        }

        if(timeDifference >= Settings.lightChangeDuration)
        {
            currentLight.color = currentLightDetalis.lightColor;
            currentLight.intensity = currentLightDetalis.lightAmount;
        }
    }
}
