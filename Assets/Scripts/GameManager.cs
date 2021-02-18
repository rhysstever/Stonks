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
    GraphManager graph;

    //Store the current time whenever we generate income so we can calculate how much
    //To give the player in passive income when they come back.
    string currentTimeString;
    string lastTimeString;
    System.DateTime currentTime;
    System.DateTime lastTime;

    // Start is called before the first frame update
    void Start()
    {
        graph = GameObject.Find("Window_Graph").GetComponent<GraphManager>();
        market = new Market();
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        GenerateAndDisplayIncome();
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
    void GenerateAndDisplayIncome()
    {
        if(timer > 1.0f) {
            timer = 0.0f;

            float moneyGenerated = market.CalcMoney();

            money += moneyGenerated;

            graph.cleanupPrevious();

            graph.ShowGraph(market.CompilePriceList(moneyGenerated));
        }
    }

    /// <summary>
    /// Triggers a Save whenever the app is closed.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    void LoadGame()
    {
        //Check if there is a local save to load
        if (PlayerPrefs.HasKey("lastTime"))
        {
            Debug.Log("Loading Game From Save");

            lastTimeString = PlayerPrefs.GetString("lastTime");
            money = PlayerPrefs.GetFloat("money");

            Debug.Log("Player has $" + money + " before offline calculations.");

            foreach (KeyValuePair<string, Stock> stock in market.StockList)
            {
                market.SetStockAmount(stock.Key, PlayerPrefs.GetInt(stock.Key + "Amount"));
                market.SetStockPrice(stock.Key, PlayerPrefs.GetFloat(stock.Key + "Price"));
            }

            //Calculate the difference in time since last login to calculate passive money earned
            lastTime = System.DateTime.Parse(lastTimeString);
            currentTime = System.DateTime.Now;
            int offlineSeconds = (int)((currentTime - lastTime).TotalSeconds);

            Debug.Log("Recorded Loaded Time: " + lastTimeString);
            Debug.Log("Parsed Time: " + lastTime);
            Debug.Log("Current Time: " + currentTime);
            Debug.Log("Recorded Comparison Time: " + offlineSeconds);

            float passiveMoney = market.CalcOfflineMoney(offlineSeconds);

            Debug.Log("Generated $" + passiveMoney + " while offline.");

            money += passiveMoney;
        }
        else
        {
            Debug.Log("No save found, creating new game");

            money = 500.0f;
            multiplier = 1;
            market = new Market();
        }
    }

    void SaveGame()
    {
        currentTimeString = System.DateTime.Now.ToString();

        //Save the time and money
        PlayerPrefs.SetString("lastTime", currentTimeString);
        PlayerPrefs.SetFloat("money", money);

        //Save individual stocks
        foreach(KeyValuePair<string, Stock> stock in market.StockList)
        {
            PlayerPrefs.SetInt(stock.Key + "Amount", stock.Value.SharesOwned);
            PlayerPrefs.SetFloat(stock.Key + "Price", stock.Value.PricePerShare);
        }      
    }
}
