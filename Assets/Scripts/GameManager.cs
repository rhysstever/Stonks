using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float money;
    public int[] multipliers;
    float timer;
    public Market market;

    [SerializeField]
    GraphManager graph;

    //private static GameManager _instance;

    //public static GameManager Instance { get { return _instance; } }

    //Store the current time whenever we generate income so we can calculate how much
    //To give the player in passive income when they come back.
    string currentTimeString;
    string lastTimeString;
    System.DateTime currentTime;
    System.DateTime lastTime;

    // Start is called before the first frame update
    void Start()
    {
        multipliers = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
        graph = graph.GetComponent<GraphManager>();
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


    public int[] CalculateMaxMultiplier()
    {
        int[] newMultipliers = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };

        // If the player does not have any money, there's no need to calculate the max number of shares they can buy
        if(money <= 0.0f)
            return newMultipliers;

        foreach(Stock stock in market.StockList.Values) {
            
                
		}

        return newMultipliers;
    }

    /// <summary>
    /// A helper method for changing the number of stocks you can buy with each click
    /// Sets each multiplier value to the same integer
    /// </summary>
    /// <param name="newMultiplier">The new multiplier for every stock</param>
    public void ChangeMultipler(int newMultiplier)
    {
        for(int i = 0; i < multipliers.Length; i++)
            multipliers[i] = newMultiplier;
    }

    /// <summary>
    /// A helper method for changing the number of stocks you can buy with each click
    /// Sets each multiplier value to the integer of the given array
    /// </summary>
    /// <param name="newMultipliers">The new multiplier for every stock</param>
    public void ChangeMultipler(int[] newMultipliers)
    {
        // If there aren't the right number of integers in the list, the function ends
        if(newMultipliers.Length != multipliers.Length)
            return;

        for(int i = 0; i < multipliers.Length; i++)
            multipliers[i] = newMultipliers[i];
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

            graph.ShowGraph(market.CompilePriceList(money));

            Debug.Log(money);
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
                Debug.Log("Stock Generated: " + stock.Key + " at price " + stock.Value.PricePerShare + " with " + stock.Value.SharesOwned + " shares owned.");
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
            multipliers = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
            ChangeMultipler(1);
            market = new Market();
        }
    }

    void SaveGame()
    {
        currentTimeString = System.DateTime.Now.ToString();

        //Save the time and money
        PlayerPrefs.SetString("lastTime", currentTimeString);
        PlayerPrefs.SetFloat("money", 10000);

        //Save individual stocks
        foreach(KeyValuePair<string, Stock> stock in market.StockList)
        {
            PlayerPrefs.SetInt(stock.Key + "Amount", stock.Value.SharesOwned);
            PlayerPrefs.SetFloat(stock.Key + "Price", stock.Value.PricePerShare);
        }      
    }
}
