using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public int clickWeight;
    public float workMultiplier;
    public float overallStockIncomeMultiplier;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetUpgrades()
	{
        clickWeight = 1;
        workMultiplier = 1.0f;
        overallStockIncomeMultiplier = 1.0f;
    }
}
