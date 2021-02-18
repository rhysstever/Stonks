using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float money;
    public Dictionary<string, Stock> stocks;
    public int multiplier;
    float timer;
    Market market;

    //Store the current time whenever we generate income so we can calculate how much
    //To give the player in passive income when they come back.
    string currentTime;

    // Start is called before the first frame update
    void Start()
    {
        money = 500.0f;
        multiplier = 1;
        market = new Market();
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
    /// A helper method for changing the number of stocks you can buy with each click
    /// </summary>
    /// <param name="newMultiplier"></param>
    public void ChangeMultipler(int newMultiplier)
    {
        multiplier = newMultiplier;
    }

    public float CalcTimeSinceLastUpdate()
    {
        return 0;
    }

    /// <summary>
    /// Adds money to the player every second, based on the shares owned
    /// </summary>
    void GenerateIncome()
    {
        if(timer > 1.0f) {
            timer = 0.0f;

            money += market.CalcMoney();
        }
    }

    /// <summary>
    /// Triggers a Save whenever the app is closed.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    void SaveGame()
    {
        currentTime = System.DateTime.UtcNow.ToString();

        //Save the time and money
        PlayerPrefs.SetString("lastTime", currentTime);
        PlayerPrefs.SetFloat("money", money);

        //Save individual stocks
        foreach(KeyValuePair<string, Stock> stock in market.StockList)
        {
            PlayerPrefs.SetFloat(stock.Key + "Amount", stock.Value.SharesOwned);
            PlayerPrefs.SetFloat(stock.Key + "Price", stock.Value.PricePerShare);
        }      
    }
}
