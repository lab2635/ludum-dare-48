using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownImage : MonoBehaviour
{
    public Image.FillMethod fillMethod;
    public TMP_Text countTextComponent;
    public Image imageComponent;

    public float cooldown = 0f;
    public bool countEnabled;
    public int count;
    
    void Start()
    {
        imageComponent.fillMethod = fillMethod;
        
        if (!countEnabled)
        {
            countTextComponent.enabled = false;
        }
    }

    void Update()
    {
        imageComponent.fillAmount = Mathf.Clamp(cooldown, 0f, 1f);
        countTextComponent.text = count.ToString();
    }
 
}
