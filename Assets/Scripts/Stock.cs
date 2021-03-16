using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock
{
	#region Fields
	private string name;
	private float pricePerShare;
	private int sharesOwned;
	private float incomePerShare;
	private float totalIncome;
	private float volatility;
	private Queue<float> lastPrices;
	private bool spiking;
	private bool falling;
	private int eventCounter;
	#endregion

	#region Properties
	public string Name { get { return name; } }
	public float PricePerShare { get { return pricePerShare; } set { this.pricePerShare = value; } }
	public int SharesOwned { get { return sharesOwned; } set { this.sharesOwned = value; } }
	public float IncomePerShare { get { return incomePerShare; } }
	public float TotalIncome { get { return totalIncome; } }
	public float Volatility { get { return volatility; } }
	public Queue<float> LastPrices { get { return this.lastPrices; } }
	#endregion

	#region Constructor
	/// <summary>
	/// Create a new Stock
	/// </summary>
	/// <param name="name">The name of the stock</param>
	/// <param name="pricePerShare">The initial amount each share of this stock counts</param>
	/// <param name="startingShares">The starting number of shares the player owns</param>
	/// <param name="incomePerShare">The amount of income each share of this stock generates passively</param>
	public Stock(string name, float pricePerShare, int startingShares)
	{
		this.name = name;
		this.pricePerShare = pricePerShare;
		sharesOwned = startingShares;
		this.incomePerShare = CalcPerShareIncome();
		totalIncome = CalcTotalIncome();
		volatility = 2;
		lastPrices = new Queue<float>();
		for(int i = 0; i < 10; i++)
        {
			lastPrices.Enqueue(pricePerShare);
        }
		spiking = false;
		falling = false;
		eventCounter = 0;
	}
	#endregion
	
	#region Methods
	/// <summary>
	/// Buys shares of the current stock
	/// </summary>
	/// <param name="sharesBought">The number of shares bought</param>
	public void BuyStock(int sharesBought)
	{
		// Check if the player has enough money to buy shares
		float totalCost = pricePerShare * sharesBought;
		float money = GameObject.Find("GameManager").GetComponent<GameManager>().money;
		if(money < totalCost) {
			Debug.Log("Cannot buy " + sharesBought + " shares of " + name + ". Cost: $" + totalCost + " | Money: $" + money);
			return;
		}

		sharesOwned += sharesBought;
		money -= totalCost;
		GameObject.Find("GameManager").GetComponent<GameManager>().money = money;
		Debug.Log(sharesBought + " shares of " + name + " bought for $" + totalCost + " at $" + pricePerShare + " per share.");

		// Recalculates the total income generated by all of the shares of the stock
		totalIncome = CalcTotalIncome();


	}

	/// <summary>
	/// Sets the stock price so the graph can see it
	/// </summary>
	/// <param name="price"></param>
	public void SetPriceForGraph(float price)
    {
		pricePerShare = price;
    }

	/// <summary>
	/// Calculates total Income from this stock
	/// </summary>
	/// <returns></returns>
	public float CalcTotalIncome()
    {
		return (CalcPerShareIncome() * sharesOwned);
    }

	/// <summary>
	/// Calculates the rate of passive income
	/// </summary>
	/// <returns></returns>
	public float CalcPerShareIncome()
	{
		return CalcSharePrice() / 100;
	}

	public float CalcSharePrice()
    {
        if (spiking)
        {
			this.pricePerShare += (Random.Range(0.3f, 1.1f) * volatility) * (this.pricePerShare / 10);
			eventCounter++;
		}
		else if (falling)
        {
			this.pricePerShare += (Random.Range(-1.0f, -0.3f) * volatility) * (this.pricePerShare / 10);
			eventCounter++;
		}
        else
        {
			this.pricePerShare += (Random.Range(-1.0f, 1.1f) * volatility) * (this.pricePerShare / 10);
		}

		if(eventCounter == 10)
        {
			eventCounter = 0;
			falling = false;
			spiking = false;
        }
		
		if(lastPrices != null)
        {
			lastPrices.Enqueue(pricePerShare);
			if (lastPrices.Count >= 15)
			{
				lastPrices.Dequeue();
			}
		}		
		return this.pricePerShare;
    }

	/// <summary>
	/// Overloaded method to buy 1 share
	/// </summary>
	public void BuyStock()
	{
		BuyStock(1);
	}

	public void EventChange(bool upOrDown)
    {
        if (upOrDown)
        {
			spiking = true;
        }
        else
        {
			falling = true;
        }
    }
	#endregion
}
