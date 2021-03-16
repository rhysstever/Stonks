using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    string stockTickerText;
    Dictionary<string, string> positiveStockTickerTexts;
    Dictionary<string, string> negativeStockTickerTexts;
    GameManager gm;
    Market m;
    public string StockTickerText { get { return this.stockTickerText; } }
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        m = gm.market;
        foreach(KeyValuePair<string, Stock> s in m.StockList)
        {
            positiveStockTickerTexts.Add(s.Key, s.Key + " is spiking!");
            negativeStockTickerTexts.Add(s.Key, s.Key + " is tumbling!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEvent();
    }

    void CheckForEvent()
    {
        int trigger = (int)Random.Range(0, 100);
        if(trigger == 1)
        {
            PlayRandomNegativeEvent();
        }
        else if(trigger == 100)
        {
            PlayRandomPositiveEvent();
        }
    }

    void PlayRandomPositiveEvent()
    {
        int eventNum = (int)Random.Range(0, positiveStockTickerTexts.Count - 1);
        int counter = 0;

        Stock s = m.StockList["GME"];
        foreach (KeyValuePair<string, Stock> stock in m.StockList)
        {
            if(counter == eventNum)
            {
                s = stock.Value;
                break;
            }
        }

        s.EventChange(true);
        this.stockTickerText = positiveStockTickerTexts[s.Name];
    }

    void PlayRandomNegativeEvent()
    {
        int eventNum = (int)Random.Range(0, negativeStockTickerTexts.Count - 1);
        int counter = 0;

        Stock s = m.StockList["GME"];
        foreach (KeyValuePair<string, Stock> stock in m.StockList)
        {
            if (counter == eventNum)
            {
                s = stock.Value;
                break;
            }
        }

        s.EventChange(true);
        this.stockTickerText = negativeStockTickerTexts[s.Name];
    }
}
