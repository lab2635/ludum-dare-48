using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeArea : MonoBehaviour
{
    public UIView UpgradeCanvasObject;

    // Start is called before the first frame update
    void Start()
    {
        this.UpgradeCanvasObject = GameObject.FindGameObjectWithTag("UpgradeMenu").GetComponent<UIView>();
        this.UpgradeCanvasObject.Hide();
    }

    private void HideHud()
    {
        var hud = GameObject.FindGameObjectWithTag("Hud").GetComponent<UIView>();
        hud.Hide();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Hookable hookable = other.GetComponent<Hookable>();
        if (!hookable || !hookable.hooked) return;

        UpgradeCanvasObject.Show();
        UpgradeUI uiScript = UpgradeCanvasObject.gameObject.GetComponent<UpgradeUI>();

        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;

        HideHud();
        
        switch (hookable.upgradeType)
        {
            case UpgradeType.Sight:
                uiScript.SetSightReward();
                break;
            case UpgradeType.Boost:
                uiScript.SetBoostReward();
                break;
            case UpgradeType.Scare:
                uiScript.SetScareReward();
                break;
            case UpgradeType.Spotlight:
                uiScript.SetHeadlightReward();
                break;
            case UpgradeType.Sonar:
                uiScript.SetSonarReward();
                break;
            case UpgradeType.Goal:
                GameManager.Instance.WinGame();
                uiScript.enabled = false;
                break;
            default:
                uiScript.SetWorthlessReward();
                break;
        }

        hookable.Hook.Breakaway();
        Destroy(other.gameObject);
    }
}
