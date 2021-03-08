using System.Collections;
using System.Collections.Generic;

public class JobPosition
{
	#region Fields
	private string positionTitle;
	private float hourlyPay;
	private int currentClicks;
	private int clicksToPromotion;
	private float perClickBuyoutCost;
	private JobPosition nextPosition;
	#endregion

	#region Properties
	public string PositionTitle { get { return positionTitle; } }
	public float HourlyPay { get { return hourlyPay; } }
	public int CurrentClicks { get { return currentClicks; } }
	public int ClicksToPromotion { get { return clicksToPromotion; } }
	public float PerClickBuyoutCost { get { return perClickBuyoutCost; } }
	public JobPosition NextPosition { get { return nextPosition; } }
	#endregion

	#region Constructor
	/// <summary>
	/// Creates a job position the player can have
	/// </summary>
	/// <param name="positionTitle">The name of the position</param>
	/// <param name="hourlyPay">The amount per click the player makes</param>
	/// <param name="currentClicks">How many clicks the player has clicked as this job position</param>
	/// <param name="clicksToPromotion">How many total clicks the player needs to click to move to the next job</param>
	/// <param name="clickBuyOutCost">How much each click costs to be bought</param>
	/// <param name="nextPosition">The next job after this current one</param>
	public JobPosition(string positionTitle, float hourlyPay, int currentClicks, int clicksToPromotion, float clickBuyOutCost, JobPosition nextPosition)
	{
		this.positionTitle = positionTitle;
		this.hourlyPay = hourlyPay;
		this.currentClicks = currentClicks;
		this.clicksToPromotion = clicksToPromotion;
		this.perClickBuyoutCost = clickBuyOutCost;
		this.nextPosition = nextPosition;
	}
	#endregion

	#region Methods
	/// <summary>
	/// Adds clicks to the current click count
	/// </summary>
	/// <param name="numClicks">The number of clicks being added</param>
	public void AddClicks(int numClicks)
	{
		currentClicks += numClicks;
	}
	#endregion
}
