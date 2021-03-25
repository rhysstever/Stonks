using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Upgrades
{
    ClickWeight,
    Raise,
    Inflation
}

public class UpgradesManager : MonoBehaviour
{
    public UpgradeTier<int> currentClickWeight;
    public UpgradeTier<float> currentRaise;
    public UpgradeTier<float> currentInflation;
    public int clickWeightTierNum;
    public int raiseTierNum;
    public int inflationTierNum;

    private UpgradeTier<int> startingClickWeight;
    private UpgradeTier<float> startingRaise;
    private UpgradeTier<float> startingInflation;

    // Start is called before the first frame update
    void Start()
    {
        CreateClickUpgrades();
        CreateRaiseUpgrades();
        CreateInflationUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateClickUpgrades()
	{
        UpgradeTier<int> clickWeightTier3 = new UpgradeTier<int>("Tier 3", 10, 150000.0f, null);
        UpgradeTier<int> clickWeightTier2 = new UpgradeTier<int>("Tier 2", 5, 3250.0f, clickWeightTier3);
        UpgradeTier<int> clickWeightTier1 = new UpgradeTier<int>("Tier 1", 2, 250.0f, clickWeightTier2);
        startingClickWeight = new UpgradeTier<int>("Starting Tier", 1, 0.0f, clickWeightTier1);
	}

    void CreateRaiseUpgrades()
    {
        UpgradeTier<float> raiseTier3 = new UpgradeTier<float>("Tier 3", 1.3f, 450000.0f, null);
        UpgradeTier<float> raiseTier2 = new UpgradeTier<float>("Tier 2", 1.2f, 17500.0f, raiseTier3);
        UpgradeTier<float> raiseTier1 = new UpgradeTier<float>("Tier 1", 1.1f, 1000.0f, raiseTier2);
        startingRaise = new UpgradeTier<float>("Starting Tier", 1.0f, 0.0f, raiseTier1);
    }

    void CreateInflationUpgrades()
    {
        UpgradeTier<float> inflactionTier3 = new UpgradeTier<float>("Tier 3", 1.3f, 2000000.0f, null);
        UpgradeTier<float> inflactionTier2 = new UpgradeTier<float>("Tier 2", 1.15f, 23500.0f, inflactionTier3);
        UpgradeTier<float> inflactionTier1 = new UpgradeTier<float>("Tier 1", 1.05f, 2250.0f, inflactionTier2);
        startingInflation = new UpgradeTier<float>("Starting Tier", 1.0f, 0.0f, inflactionTier1);
    }

    public void ResetUpgrades()
	{
        currentClickWeight = startingClickWeight;
        currentRaise = startingRaise;
        currentInflation = startingInflation;
    }

    public void BuyUpgrade(Upgrades upgradeType)
	{
        switch(upgradeType) {
            case Upgrades.ClickWeight:
                break;
            case Upgrades.Raise:
                break;
            case Upgrades.Inflation:
                break;
        }

  //      if(currentUpgrade.Next != null
  //          && gameObject.GetComponent<GameManager>().money >= currentUpgrade.Next.Cost) {
  //          currentUpgrade = currentUpgrade.Next;
		//}
	}
}
