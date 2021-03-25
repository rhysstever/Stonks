using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    public JobPosition currentJob;
    public float buyoutCost;
    public int currentClicks;
    JobPosition startingJob;
    
    // Start is called before the first frame update
    void Start()
    {
        CreatePositions();

		if(currentJob == null)
			currentJob = startingJob;
	}

    // Update is called once per frame
    void Update()
    {
        CheckForPromotion();
        buyoutCost = (currentJob.ClicksToPromotion - currentClicks) * currentJob.PerClickBuyoutCost;
    }
    
    void CreatePositions()
	{
        JobPosition job5 = new JobPosition("CEO",              100000.00f, 1000000, 200000.0f, null);
        JobPosition job4 = new JobPosition("Board Member",      15000.00f,  100000,  30000.0f, job5);
        JobPosition job3 = new JobPosition("Regional Manager",   1000.00f,   50000,   2500.0f, job4);
        JobPosition job2 = new JobPosition("Franchisee",          250.00f,   10000,    300.0f, job3);
        JobPosition job1 = new JobPosition("Manager",              25.00f,    1000,     50.0f, job2);
             startingJob = new JobPosition("Frycook",               7.25f,     100,     15.0f, job1);
	}

    /// <summary>
    /// Adds the player's hourly pay to their total money and adds to their number of clicks
    /// </summary>
    public void WorkJob()
    {
        int clickWeight = gameObject.GetComponent<UpgradesManager>().currentClickWeight.Data;
        gameObject.GetComponent<GameManager>().money += 
            currentJob.HourlyPay * 
            gameObject.GetComponent<UpgradesManager>().currentRaise.Data *
            clickWeight;
        AddClicks(clickWeight);
        gameObject.GetComponent<NewsTicker>().UpdateText("Update the text to this! " + currentClicks);
    }

    public void ResetJob()
	{
        currentJob = startingJob;
        currentClicks = 0;
    }

    public void SetCurrentJob(string currentJobName, int currentClicks)
    {
        this.currentClicks = currentClicks;
        if(currentJobName == null) {
            currentJob = startingJob;
            return;
		}

        JobPosition tempJob = startingJob;
        while(tempJob != null) {
            if(tempJob.PositionTitle == currentJobName) {
                currentJob = tempJob;
                break;
            }
            else
                tempJob = tempJob.NextPosition;
		}

        currentJob = startingJob;
    }

    void AddClicks(int numOfClicks)
	{
        currentClicks += numOfClicks;
    }

    /// <summary>
    /// Checks if the number of clicks the player has is enough to be promoted
    /// </summary>
    void CheckForPromotion()
	{
        if(currentJob.NextPosition != null
            && currentClicks >= currentJob.ClicksToPromotion)
            currentJob = currentJob.NextPosition;
	}

    /// <summary>
    /// If the player has enough money, adds the remaining number of clicks to the click count
    /// </summary>
    public void PromotionBuyOut()
	{
        if(gameObject.GetComponent<GameManager>().money >= buyoutCost) {
            gameObject.GetComponent<GameManager>().money -= buyoutCost;
            AddClicks(currentJob.ClicksToPromotion - currentClicks);
        } else
            Debug.Log("Not enough money to buy promotion!");
    }
}
