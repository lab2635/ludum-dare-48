using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class UpgradeUI : MonoBehaviour
{
    public TMP_Text ItemNameText;
    public Image  RetrievedItemImage;

    public UIButton HeadlightUnlockButton;
    public UIButton SonarUnlockButton;
    public UIButton BoostUpgradeButton;
    public UIButton InkBlastUpgradeButton;
    public UIButton SightRadiusUpgradeButton;

    public TMP_Text BoostLevelText;
    public TMP_Text InkBlastUsesText;
    public TMP_Text SightRadiusText;

    public Sprite TreasureLightImage;
    public Sprite TreasureSonarImage;
    public Sprite ValuableFishImage;
    public Sprite OctopusImage;
    public Sprite WorthlessFishImage;
    public Sprite StarfishImage;
    
    private string BoostLevelBase = "cooldown: ";
    private string InkBlastBase = "uses: ";
    private string SightRadiusBase = "radius: ";

    private void Start()
    {
        this.HeadlightUnlockButton.DisableButton();
        this.SonarUnlockButton.DisableButton();
        this.BoostUpgradeButton.DisableButton();
        this.InkBlastUpgradeButton.DisableButton();
        this.SightRadiusUpgradeButton.DisableButton();

        this.SetCooldownText(UpgradeManager.Instance.boostCooldownBase);
        this.SetRadiusText(UpgradeManager.Instance.sightRadiusBase);
        this.SetUsesText(UpgradeManager.Instance.inkBlastChargesBase);
    }

    private void Update()
    {
        this.SetCooldownText(UpgradeManager.Instance.boostCooldownBase - UpgradeManager.Instance.boostCooldownModifier);
        this.SetRadiusText(UpgradeManager.Instance.sightRadiusBase + UpgradeManager.Instance.sightRadiusModifier);
        this.SetUsesText(UpgradeManager.Instance.inkBlastCharges);
    }

    public void SetSightReward()
    {
        this.RetrievedItemImage.sprite = this.StarfishImage;
        this.ItemNameText.text = "Magic Starfish!";
        this.SightRadiusUpgradeButton.EnableButton();
    }

    public void SetBoostReward()
    {
        this.RetrievedItemImage.sprite = this.ValuableFishImage;
        this.ItemNameText.text = "Valuable Fish!";
        this.BoostUpgradeButton.EnableButton();
    }

    public void SetScareReward()
    {
        this.RetrievedItemImage.sprite = this.OctopusImage;
        this.ItemNameText.text = "Gassy Octopus!";
        this.InkBlastUpgradeButton.EnableButton();
    }

    public void SetHeadlightReward()
    {
        this.RetrievedItemImage.sprite = this.TreasureLightImage;
        this.ItemNameText.text = "Ancient Treasure!";
        this.HeadlightUnlockButton.EnableButton();
    }

    public void SetSonarReward()
    {
        this.RetrievedItemImage.sprite = this.TreasureSonarImage;
        this.ItemNameText.text = "Secret Treasure!";
        this.SonarUnlockButton.EnableButton();
    }

    public void SetWorthlessReward()
    {
        this.RetrievedItemImage.sprite = this.WorthlessFishImage;
        this.ItemNameText.text = "Worthless!";
    }

    public void ClickSightRadiusUpgrade()
    {
        UpgradeManager.Instance.SightRadiusLevelUp();
        this.SightRadiusUpgradeButton.DisableButton();
    }

    public void ClickBoostUpgrade()
    {
        UpgradeManager.Instance.BoostCooldownLevelUp();
        this.BoostUpgradeButton.DisableButton();
    }

    public void ClickInkBlastUpgrade()
    {
        UpgradeManager.Instance.UnlockInkBlast();
        this.InkBlastUpgradeButton.DisableButton();
    }

    public void ClickHeadlightUpgrade()
    {
        UpgradeManager.Instance.UnlockSubLight();
        this.HeadlightUnlockButton.DisableButton();
    }

    public void ClickSonarUpgrade()
    {
        UpgradeManager.Instance.UnlockSonar();
        this.SonarUnlockButton.DisableButton();
    }

    public void ClickCloseWindow()
    {
        this.gameObject.GetComponent<UIView>().Hide();
        
        // reshow HUD
        var hud = GameObject.FindGameObjectWithTag("Hud").GetComponent<UIView>();
        hud.Show();
    }

    private void SetCooldownText(float value)
    {
        this.BoostLevelText.text = this.BoostLevelBase + value + "s";
    }

    private void SetUsesText(int value)
    {
        this.InkBlastUsesText.text = this.InkBlastBase + value;
    }

    private void SetRadiusText(float value)
    {
        this.SightRadiusText.text = this.SightRadiusBase + value + "m";
    }
}
