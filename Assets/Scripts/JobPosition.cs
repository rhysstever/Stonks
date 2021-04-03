using System;
using System.Collections;
using System.Collections.Generic;

public class JobPosition
{
	#region Fields
	private string positionTitle;
	private float hourlyPay;
	private float clicksToPromotion;
	private float perClickBuyoutCost;
	private JobPosition nextPosition;
	#endregion

	#region Properties
	public string PositionTitle { get { return positionTitle; } }
	public float HourlyPay { get { return hourlyPay; } }
	public float ClicksToPromotion { get { return (int)clicksToPromotion; } }
	public float PerClickBuyoutCost { get { return perClickBuyoutCost; } }
	public JobPosition NextPosition { get { return nextPosition; } }
	#endregion

	#region Constructor
	/// <summary>
	/// Creates a job position the player can have
	/// </summary>
	/// <param name="positionTitle">The name of the position</param>
	/// <param name="tierNum">The tier of the job (used for scaling)</param>
	/// <param name="nextPosition">The next job after this current one</param>
	public JobPosition(string positionTitle, int tierNum, JobPosition nextPosition)
	{
		this.positionTitle = positionTitle;
		this.nextPosition = nextPosition;
		hourlyPay = 7.8f * (tierNum * tierNum) - 16.25f * tierNum + 8.55f;
		clicksToPromotion = Round((float)(Math.Pow(2.5, tierNum) * 25.0f), 1);
		perClickBuyoutCost = Round((float)Math.Pow(3.2f, tierNum), 2);
	}
	#endregion

	#region Methods
	/// <summary>
	/// A helper method to round a number to a given tens place
	/// </summary>
	/// <param name="number">The number being rounded</param>
	/// <param name="placesToRound">The tens place to round too</param>
	/// <returns>The number rounded. Ex: Round(115, 1) -> 100 but Round(115, 2) -> 120</returns>
	float Round(float number, int placesToRound)
	{
		if(number < 1)
			return 0;

		double tensPlace = Math.Floor(Math.Log10(number) - (placesToRound - 1));
		return (float)(Math.Round(number / (Math.Pow(10, tensPlace))) * (Math.Pow(10, tensPlace)));
	}
	#endregion
}