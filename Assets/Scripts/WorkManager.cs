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
		JobPosition job7 = new JobPosition("Space Entrepreneur",                7, null);
		JobPosition job6 = new JobPosition("CEO",			                    6, job7);
		JobPosition job5 = new JobPosition("Owner",				                5, job6);
		JobPosition job4 = new JobPosition("Assistant to the Regional Manager", 4, job5);
		JobPosition job3 = new JobPosition("Office Job",                        3, job4);
		JobPosition job2 = new JobPosition("Food Driver",                       2, job3);
			 startingJob = new JobPosition("Reddit Scroller",                   1, job2);
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
		SetCurrentJob(startingJob.PositionTitle, 0);
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
			AddClicks((int)currentJob.ClicksToPromotion - currentClicks);
		} else
			Debug.Log("Not enough money to buy promotion!");
	}
}
