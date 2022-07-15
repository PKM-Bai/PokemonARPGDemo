using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHPBar : MonoBehaviour
{
    public Slider HPValue;

    public TextMeshProUGUI level;
    public TextMeshProUGUI pkmName;
    public Image sex;
    public RectTransform typeIcon;
    public Image typeIconPrefab;

    private void Awake() {
        HPValue = GetComponent<Slider>();
    }
    
}
