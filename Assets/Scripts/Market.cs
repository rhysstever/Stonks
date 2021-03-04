using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Market
{
    Dictionary<string, Stock> stockList;
    Queue<float> graphValues = new Queue<float>();
    private string currentActiveStock;

    public Dictionary<string, Stock> StockList { get { return this.stockList; } }
    public string CurrentActiveStock { get { return currentActiveStock; } set { this.currentActiveStock = value; } }

    public Market()
    {
        InitMarket();
        currentActiveStock = "DOGE";
    }

    public void InitMarket()
    {
        stockList = new Dictionary<string, Stock>();

        stockList.Add("DOGE", new Stock("DOGE", 0.05f, 0));
        stockList.Add("AMC", new Stock("AMC", 3.00f, 0));
        stockList.Add("NOK", new Stock("NOK", 4.20f, 0));
        stockList.Add("BB", new Stock("BB", 10.95f, 0));
        stockList.Add("GME", new Stock("GME", 14.00f, 0));
        stockList.Add("PLTR", new Stock("PLTR", 25.00f, 0));
        stockList.Add("TSLA", new Stock("TSLA", 200.00f, 0));
        stockList.Add("BTC", new Stock("BTC", 5000.00f, 0));  
    }

    /// <summary>
    /// Helper method for when the game loads
    /// </summary>
    /// <param name="newPrice"></param>
    public void SetStockPrice(string stockName, float newPrice)
    {
        stockList[stockName].SetPriceForGraph(newPrice);
    }

    /// <summary>
    /// Helper method for when the game loads
    /// </summary>
    /// <param name="newAmount"></param>
    public void SetStockAmount(string stockName, int newAmount)
    {
        stockList[stockName].SharesOwned = newAmount;
    }

    public float CalcMoney()
    {
        // Loops through each stock, adding the income generated based on the number of shares owned
        float moneyAtTime = 0.0f;
        foreach(KeyValuePair<string, Stock> stock in stockList)
        {
            moneyAtTime += stock.Value.CalcTotalIncome();
        }
        return moneyAtTime;
    }

    public List<float> CompilePriceList(float newPricePoint)
    {
        graphValues.Enqueue(newPricePoint);
        if(graphValues.Count > 9)
        {
            graphValues.Dequeue();
        }

        Debug.Log(graphValues.Peek());

        return graphValues.ToList();
    }

    public float CalcOfflineMoney(int seconds)
    {
        return CalcMoney() * seconds;
    }
}
