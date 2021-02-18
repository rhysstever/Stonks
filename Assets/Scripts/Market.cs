using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market
{
    bool marketInitialized = false;
    Dictionary<string, Stock> stockList;

    public Dictionary<string, Stock> StockList { get { return this.stockList; } }

    public Market()
    {
        InitMarket();
    }

    public void InitMarket()
    {
        if (!marketInitialized)
        {
            stockList = new Dictionary<string, Stock>();

            stockList.Add("DOGE", new Stock("DOGE", 0.05f, 50));
            stockList.Add("AMC", new Stock("AMC", 3.0f, 0));
            stockList.Add("NOK", new Stock("NOK", 4.20f, 0));
            stockList.Add("BB", new Stock("BB", 10.95f, 0));
            stockList.Add("GME", new Stock("GME", 14.0f, 0));
            stockList.Add("PLTR", new Stock("PLTR", 25.0f, 0));
            stockList.Add("TSLA", new Stock("TSLA", 200.0f, 0));
            stockList.Add("BTC", new Stock("BTC", 5000.0f, 0));

            marketInitialized = true;
        }
        else
        {
            Debug.Log("The Market has already been initialized, assuming game was relaunched.");
        }     
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
}