using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    public JobPosition currentJob;
    public string currentTitle;
    public float buyOutCost;
    private int clicksRemaining;
    
    // Start is called before the first frame update
    void Start()
    {
        CreatePositions();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) // will be changed to a button
            WorkJob();

        if(Input.GetKeyDown(KeyCode.P)) // will be changed to a button
            PromotionBuyOut();

        CheckForPromotion();

        currentTitle = currentJob.PositionTitle;
        clicksRemaining = currentJob.ClicksToPromotion - currentJob.CurrentClicks;
        buyOutCost = clicksRemaining * currentJob.ClickBuyOutCost;
    }

    void CreatePositions()
	{
        JobPosition job5 = new JobPosition("CEO", 100000.00f, 0, 1000000, 20000.0f, null);
        JobPosition job4 = new JobPosition("Board Member", 15000.00f, 0, 100000, 3000.0f, job5);
        JobPosition job3 = new JobPosition("Regional Manager", 1000.00f, 0, 50000, 100.0f, job4);
        JobPosition job2 = new JobPosition("Franchisee", 250.00f, 0, 10000, 30.0f, job3);
        JobPosition job1 = new JobPosition("Manager", 25.00f, 0, 1000, 5.0f, job2);
        JobPosition job = new JobPosition("Frycook", 7.25f, 0, 100, 1.5f, job1);
        
        currentJob = job;
	}

    /// <summary>
    /// Adds the player's hourly pay to their total money and adds to their number of clicks
    /// </summary>
    void WorkJob()
    {
        gameObject.GetComponent<GameManager>().money += currentJob.HourlyPay;
        currentJob.AddClicks(1);
    }

    /// <summary>
    /// Checks if the number of clicks the player has is enough to be promoted
    /// </summary>
    void CheckForPromotion()
	{
        if(currentJob.NextPosition != null
            && currentJob.CurrentClicks >= currentJob.ClicksToPromotion)
            currentJob = currentJob.NextPosition;
	}

    /// <summary>
    /// If the player has enough money, adds the remaining number of clicks to the click count
    /// </summary>
    void PromotionBuyOut()
	{
        if(gameObject.GetComponent<GameManager>().money >= buyOutCost) {
            gameObject.GetComponent<GameManager>().money -= buyOutCost;
            currentJob.AddClicks(clicksRemaining);
        }
    }
}
