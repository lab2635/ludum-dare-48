using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private UIView view;
    
    void Start()
    {
        view = GetComponent<UIView>();
        view.Hide();
    }
}
