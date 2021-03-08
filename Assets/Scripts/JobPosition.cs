using System.Collections;
using System.Collections.Generic;

public class JobPosition
{
	#region Fields
	private string positionTitle;
	private float hourlyPay;
	private int clicksToPromotion;
	private float perClickBuyoutCost;
	private JobPosition nextPosition;
	#endregion

	#region Properties
	public string PositionTitle { get { return positionTitle; } }
	public float HourlyPay { get { return hourlyPay; } }
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
	/// <param name="clicksToPromotion">How many total clicks the player needs to click to move to the next job</param>
	/// <param name="perClickBuyoutCost">How much each click costs to be bought</param>
	/// <param name="nextPosition">The next job after this current one</param>
	public JobPosition(string positionTitle, float hourlyPay, int clicksToPromotion, float perClickBuyoutCost, JobPosition nextPosition)
	{
		this.positionTitle = positionTitle;
		this.hourlyPay = hourlyPay;
		this.clicksToPromotion = clicksToPromotion;
		this.perClickBuyoutCost = perClickBuyoutCost;
		this.nextPosition = nextPosition;
	}
	#endregion

	#region Methods
	#endregion
}
