using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float money;
    public Dictionary<string, Stock> stocks;
    public int multiplier;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        money = 500.0f;
        multiplier = 1;
        LoadStocks();
    }

    // Update is called once per frame
    void Update()
    {
        GenerateIncome();
        timer += Time.deltaTime;

        // Hit ESC to close the game
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    /// <summary>
    /// Creates the stocks Dictionary and adds stocks to it
    /// </summary>
    void LoadStocks()
    {
        stocks = new Dictionary<string, Stock>();
        stocks.Add("Dogecoin", new Stock("Dogecoin", 5.0f, 1, 0.02f));
        stocks.Add("AMC", new Stock("AMC", 30.0f, 0, 0.5f));
        stocks.Add("GME", new Stock("GME", 250.0f, 0, 2.5f));
    }

    /// <summary>
    /// A helper method for changing the number of stocks you can buy with each click
    /// </summary>
    /// <param name="newMultiplier"></param>
    public void ChangeMultipler(int newMultiplier)
    {
        multiplier = newMultiplier;
    }

    /// <summary>
    /// Adds money to the player every second, based on the shares owned
    /// </summary>
    void GenerateIncome()
    {
        if(timer > 1.0f) {
            timer = 0.0f;
            // Loops through each stock, adding the income generated based on the number of shares owned
            foreach(string name in stocks.Keys) {
                money += stocks[name].IncomePerShare * stocks[name].SharesOwned;
            }
        }
    }
}
