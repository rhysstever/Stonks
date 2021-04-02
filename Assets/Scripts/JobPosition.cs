using System;
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
	/// <param name="nextPosition">The next job after this current one</param>
	public JobPosition(string positionTitle, int tierNum, JobPosition nextPosition)
	{
		this.positionTitle = positionTitle;
		this.nextPosition = nextPosition;
		hourlyPay = 7.8f * (tierNum * tierNum) - 16.25f * tierNum + 8.55f;
		clicksToPromotion = (int)(Math.Pow(4, tierNum) * 10);
		perClickBuyoutCost = (float)Math.Pow(3.2f, tierNum);
	}
	#endregion

	#region Methods
	#endregion
}
