using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : SingletonBehaviour<UpgradeManager>
{
    public float sightRadiusBase;
    public float sightRadiusModifier;

    public float boostCooldownBase;
    public float boostCooldownModifier;

    public bool spotlightUnlocked;
    public bool sonarUnlocked;
    
    public bool inkBlastUnlocked;
    public int inkBlastChargesBase;
    public int inkBlastCharges;
    public int currentInkBlastCharges;
    
    private GameObject playerObject;
    private Light sightLight;
    

    // Start is called before the first frame update
    void Start()
    {
        this.spotlightUnlocked = false;
        this.inkBlastUnlocked = false;
        this.sonarUnlocked = false;

        this.playerObject = GameObject.FindGameObjectWithTag("Player");
        this.sightLight = GameObject.FindGameObjectWithTag("SightLight").GetComponent<Light>();
        this.sightLight.spotAngle = this.sightRadiusBase;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SightRadiusLevelUp()
    {
        sightRadiusModifier += 2;
        sightLight.spotAngle = sightRadiusBase + sightRadiusModifier;
    }

    public void BoostCooldownLevelUp()
    {
        boostCooldownModifier += 0.2f;
    }

    public void UnlockSubLight()
    {
        this.spotlightUnlocked = true;
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
        playerMovement.EnableSpotlight();
    }

    public void UnlockSonar()
    {
        this.sonarUnlocked = true;
    }

    public void UnlockInkBlast()
    {
        this.inkBlastUnlocked = true;
        InkBlastLevelUp();
    }

    public void InkBlastLevelUp()
    {
        this.inkBlastCharges++;
        currentInkBlastCharges = inkBlastCharges;
    }

    public bool HasInkBlastCharge()
    {
        return currentInkBlastCharges > 0;
    }

    public void ConsumeInkCharge()
    {
        currentInkBlastCharges--;
        if (currentInkBlastCharges < 0)
        {
            currentInkBlastCharges = 0;
        }
    }

    public void RefillInkCharges()
    {
        this.currentInkBlastCharges = this.inkBlastCharges;
    }
}
