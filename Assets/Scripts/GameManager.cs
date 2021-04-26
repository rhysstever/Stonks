using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public float money;
    public int[] multipliers;
    public Market market;
    public NewsTicker nt;
    float timer;
    bool isMaxMultiplier;

    [SerializeField]
    GraphManager graph;

    [SerializeField]
    GameObject launchScreen;

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
        nt = this.gameObject.GetComponent<NewsTicker>();
        isMaxMultiplier = false;
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        GenerateAndDisplayIncome();
        timer += Time.deltaTime;

        if(isMaxMultiplier)
            ChangeMultipler(CalculateMaxMultiplier());

        // Hit ESC to close the game
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    /// <summary>
    /// Calculates the maximum number of stocks the player can buy of each stock
    /// </summary>
    /// <returns>Returns an array of integers where each integer is the maximum number 
    /// of stocks the player can buy with their current money amount</returns>
    public int[] CalculateMaxMultiplier()
    {
        int[] newMultipliers = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };

        // If the player does not have any money, there's no need to calculate the max number of shares they can buy
        if(money <= 0.0f)
            return newMultipliers;

        int stockCount = 0;
        foreach(Stock stock in market.StockList.Values) {
            newMultipliers[stockCount] = (int)(money / stock.PricePerShare);
            stockCount++;
		}

        isMaxMultiplier = true;
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
        isMaxMultiplier = false;
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
    /// Also saves the game every second to fix WebGL's problems
    /// </summary>
    void GenerateAndDisplayIncome()
    {
        if(timer > 1.0f) {
            timer = 0.0f;

            float moneyGenerated = market.CalcMoney() * gameObject.GetComponent<UpgradesManager>().currentInflation.Data;

            money += moneyGenerated;

            graph.CleanupPrevious();

            graph.ShowGraph(market.StockList[market.CurrentActiveStock].LastPrices.ToList());

            SaveGame();
            PlayerPrefs.Save();

            // Debug.Log(money);
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

            string passiveGenString = "You earned $" + passiveMoney.ToString("0.00") + " while away!";
            launchScreen.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = passiveGenString;

            money += passiveMoney;

            // Load Job Data
            string jobName = PlayerPrefs.GetString("currentPosition");
            int currentClicks = PlayerPrefs.GetInt("currentClicks");
            gameObject.GetComponent<WorkManager>().SetCurrentJob(jobName, currentClicks);

            // Load Upgrades Data
            gameObject.GetComponent<UpgradesManager>().clickWeightTierNum = PlayerPrefs.GetInt("clickWeightTierNum");
            gameObject.GetComponent<UpgradesManager>().raiseTierNum = PlayerPrefs.GetInt("raiseTierNum");
            gameObject.GetComponent<UpgradesManager>().inflationTierNum = PlayerPrefs.GetInt("inflationTierNum");
        }
        else
        {
            Debug.Log("No save found!");

            RestartGame();
        }
    }

    public void RestartGame(){
        Debug.Log("Creating new game");

        Transform t = launchScreen.transform.GetChild(3);
        t.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Stonks only go up!";

        money = 0.0f;
        multipliers = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
        ChangeMultipler(multipliers);
        market = new Market();
            
        gameObject.GetComponent<WorkManager>().ResetJob();
        gameObject.GetComponent<UpgradesManager>().ResetUpgrades();
        gameObject.GetComponent<UIManager>().ResetUI();
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

        // Save job data
        PlayerPrefs.SetString("currentPosition", gameObject.GetComponent<WorkManager>().currentJob.PositionTitle);
        PlayerPrefs.SetInt("currentClicks", gameObject.GetComponent<WorkManager>().currentClicks);

        // Save upgrades data
        PlayerPrefs.SetInt("clickWeightTierNum", gameObject.GetComponent<UpgradesManager>().clickWeightTierNum);
        PlayerPrefs.SetInt("raiseTierNum", gameObject.GetComponent<UpgradesManager>().raiseTierNum);
        PlayerPrefs.SetInt("inflationTierNum", gameObject.GetComponent<UpgradesManager>().inflationTierNum);
    }
}
