using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market
{
    bool marketInitialized = false;
    List<Stock> stockList;

    public Market()
    {
        InitMarket();
    }

    public void InitMarket()
    {
        if (!marketInitialized)
        {
            stockList = new List<Stock>();

            stockList.Add(new Stock("DOGE", 0.05f, 50));
            stockList.Add(new Stock("AMC", 3.0f, 0));
            stockList.Add(new Stock("NOK", 4.20f, 0));
            stockList.Add(new Stock("BB", 10.95f, 0));
            stockList.Add(new Stock("GME", 14.0f, 0));
            stockList.Add(new Stock("PLTR", 25.0f, 0));
            stockList.Add(new Stock("TSLA", 200.0f, 0));
            stockList.Add(new Stock("BTC", 5000.0f, 0));

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
        foreach(Stock stock in stockList)
        {
            moneyAtTime += stock.CalcTotalIncome();
        }
        return moneyAtTime;
    }
}
