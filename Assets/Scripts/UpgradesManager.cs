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
        UpgradeTier<int> clickWeightTier3 = new UpgradeTier<int>(Upgrades.ClickWeight, 10, 150000.0f, null);
        UpgradeTier<int> clickWeightTier2 = new UpgradeTier<int>(Upgrades.ClickWeight, 5, 12500.0f, clickWeightTier3);
        UpgradeTier<int> clickWeightTier1 = new UpgradeTier<int>(Upgrades.ClickWeight, 2, 1250.0f, clickWeightTier2);
        startingClickWeight = new UpgradeTier<int>(Upgrades.ClickWeight, 1, 0.0f, clickWeightTier1);
	}

    void CreateRaiseUpgrades()
    {
        UpgradeTier<float> raiseTier3 = new UpgradeTier<float>(Upgrades.Raise, 1.3f, 450000.0f, null);
        UpgradeTier<float> raiseTier2 = new UpgradeTier<float>(Upgrades.Raise, 1.2f, 17500.0f, raiseTier3);
        UpgradeTier<float> raiseTier1 = new UpgradeTier<float>(Upgrades.Raise, 1.1f, 1000.0f, raiseTier2);
        startingRaise = new UpgradeTier<float>(Upgrades.Raise, 1.0f, 0.0f, raiseTier1);
    }

    void CreateInflationUpgrades()
    {
        UpgradeTier<float> inflactionTier3 = new UpgradeTier<float>(Upgrades.Inflation, 1.3f, 2000000.0f, null);
        UpgradeTier<float> inflactionTier2 = new UpgradeTier<float>(Upgrades.Inflation, 1.15f, 23500.0f, inflactionTier3);
        UpgradeTier<float> inflactionTier1 = new UpgradeTier<float>(Upgrades.Inflation, 1.05f, 2250.0f, inflactionTier2);
        startingInflation = new UpgradeTier<float>(Upgrades.Inflation, 1.0f, 0.0f, inflactionTier1);
    }

    public void ResetUpgrades()
	{
        currentClickWeight = startingClickWeight;
        currentRaise = startingRaise;
        currentInflation = startingInflation;
    }

    public void BuyUpgrade<T>(UpgradeTier<T> currentUpgrade)
	{
        if(currentUpgrade.Next != null
			&& gameObject.GetComponent<GameManager>().money >= currentUpgrade.Next.Cost) {
            gameObject.GetComponent<GameManager>().money -= currentUpgrade.Next.Cost;
            switch(currentUpgrade.Type) {
                case Upgrades.ClickWeight:
                    currentClickWeight = currentClickWeight.Next;
                    break;
                case Upgrades.Raise:
                    currentRaise = currentRaise.Next;
                    break;
                case Upgrades.Inflation:
                    currentInflation = currentInflation.Next;
                    break;
            }

            Debug.Log(currentUpgrade.Type + " Upgraded");
		} else {
            Debug.Log("No next upgrade available or not enough money!");
		}
	}
}
