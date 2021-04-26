using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Slider healthSlider;
    public CooldownImage boost;
    public CooldownImage light;
    public CooldownImage sonar;
    public CooldownImage ink;
    public CooldownImage unhook;
    public TMPro.TMP_Text depthText;

    private PlayerMovement player;
    private Hook hook;
    private HealthManager health => HealthManager.Instance;
    private UpgradeManager upgrades => UpgradeManager.Instance;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        hook = GameObject.FindWithTag("Hook").GetComponent<Hook>();
    }
    
    void Update()
    {
        // health bar
        var healthValue = health.currentHealth / (float) health.maxHealth;
        healthSlider.DOValue(healthValue, 0.1f);
        
        // cooldowns
        boost.cooldown = GetCooldownDuration(player.boostTimer, player.boostCooldown);
        sonar.cooldown = GetCooldownDuration(
            upgrades.sonarUnlocked ? player.sonarPulseTimer : 0f, 
            player.sonarCooldownTime);
        ink.cooldown = GetCooldownDuration(
            upgrades.currentInkBlastCharges > 0 ? player.blastTimer : 0f, 
            player.blastCooldown);
        ink.count = upgrades.currentInkBlastCharges;
        light.cooldown = player.isSpotlightEnabled ? 1f : 0f;
        unhook.cooldown = hook.IsHooked() ? 1f : 0f;
        
        // depth meter
        depthText.text = $"{player.GetDepth()} m";
    }

    private float GetCooldownDuration(float cooldown, float duration)
    {
        var value = cooldown / duration;
        return Mathf.Clamp(value, 0f, 1f);
    }
}
