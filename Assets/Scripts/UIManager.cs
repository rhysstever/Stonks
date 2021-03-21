using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    GameManager gm;
    public Canvas canvas;
    private GameObject GraphName;

    [SerializeField]
    GameObject GameUIPanel;

    [SerializeField]
    TextMeshProUGUI moneyText;

    // Multiplier Buttons
    [SerializeField]
    GameObject multiply1;

    [SerializeField]
    GameObject multiply10;

    [SerializeField]
    GameObject multiply100;

    [SerializeField]
    GameObject maxBuy;

    [SerializeField]
    GameObject stockDisplayPrefab;

    [SerializeField]
    GameObject stockDisplays;

    List<Button> stockButtons;
    List<TextMeshProUGUI> stockTexts;

    // Work Click Button
    [SerializeField]
    GameObject workButton;

    // Work Display Text
    [SerializeField]
    TextMeshProUGUI currentPosition;

    [SerializeField]
    TextMeshProUGUI payRate;

    [SerializeField]
    TextMeshProUGUI clicksRemaining;

    [SerializeField]
    TextMeshProUGUI buyOutCost;

    [SerializeField]
    TextMeshProUGUI graphLabel;

    // Start is called before the first frame update
    void Start()
    {
        SetupUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void SetupUI()
    {
        // Get the static GM instance
        //gm = GameManager.Instance;
        gm = gameObject.GetComponent<GameManager>();

        ResetUI();

        // Sets up multiplier buttons and sets their onClicks
        List<Button> multiplierButtons = new List<Button>();
        multiplierButtons.Add(multiply1.gameObject.GetComponent<Button>());
        multiplierButtons.Add(multiply10.gameObject.GetComponent<Button>());
        multiplierButtons.Add(multiply100.gameObject.GetComponent<Button>());
        multiplierButtons.Add(maxBuy.gameObject.GetComponent<Button>());

        multiply1.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<GameManager>().ChangeMultipler(1));

        multiply10.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<GameManager>().ChangeMultipler(10));

        multiply100.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<GameManager>().ChangeMultipler(100));

        maxBuy.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<GameManager>().ChangeMultipler(
                    gameObject.GetComponent<GameManager>().CalculateMaxMultiplier()));

        workButton.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<WorkManager>().WorkJob());

        gm.ChangeMultipler(1);
        multiply1.GetComponent<Button>().Select();

        GraphName = GameObject.Find("GraphName");
    }

    // Update the multiplied costs on each stocks' buy button
    public void UpdateMultiplicity()
    {

    }

    public void ResetUI()
    {
        stockButtons = new List<Button>();
        stockTexts = new List<TextMeshProUGUI>();

        //Delete the placeholders, we'll readd them with code. 
        foreach (Transform child in stockDisplays.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach(Stock stock in gm.market.StockList.Values) {
            GameObject newStock = Instantiate(stockDisplayPrefab);
            newStock.transform.SetParent(stockDisplays.transform);
            newStock.name = stock.Name;
            GameObject buyButton = newStock.transform.Find("BUY_BUTTON").gameObject;
            buyButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                stock.BuyStock(gameObject.GetComponent<GameManager>().multipliers[0]);
                gm.market.CurrentActiveStock = stock.Name;
                graphLabel.text = stock.Name + " stock graph";
            });
            stockButtons.Add(buyButton.GetComponent<Button>());

            // Finds all stock text objects and adds it to a list
            string stockTextName = stock.Name;
            GameObject newText = newStock.transform.Find("STOCK_NAME").gameObject;
            TextMeshProUGUI tmpAsset = newText.GetComponent<TextMeshProUGUI>();
            newText.name = stockTextName;
            stockTexts.Add(tmpAsset);
        }
    }

    void UpdateUI()
    {
        // Update money display text
        moneyText.text = gameObject.GetComponent<GameManager>().money.ToString("C");

        for(int i = 0; i < gm.market.StockList.Values.Count; i++ )
        {
            Stock currentStock = gm.market.StockList[stockTexts[i].gameObject.name];
            stockTexts[i].text = currentStock.Name + " - Shares: " + currentStock.SharesOwned;

            // Shows the subtext of the current price of a single stock
            string singleStockValue = "Current price for 1 share: " + currentStock.PricePerShare.ToString("C");
            stockButtons[i].transform.parent.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = singleStockValue;

            // Calculates the total price of the purchase, based on the multiplier, then displays it in the currency format
            float price = currentStock.PricePerShare * gameObject.GetComponent<GameManager>().multipliers[i];
            stockButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Buy " + gm.multipliers[i] + " shares for " + price.ToString("C");
        }

        // Update job display text
        currentPosition.text = "Current Position: " + gameObject.GetComponent<WorkManager>().currentJob.PositionTitle;
        payRate.text = "Pay Rate: " + gameObject.GetComponent<WorkManager>().currentJob.HourlyPay.ToString("C");
        int clicksRemainingCount = gameObject.GetComponent<WorkManager>().currentJob.ClicksToPromotion - gameObject.GetComponent<WorkManager>().currentClicks;
        clicksRemaining.text = "Clicks Remaining: " + clicksRemainingCount;
        buyOutCost.text = "Buyout Cost: " + gameObject.GetComponent<WorkManager>().buyoutCost.ToString("C");
    }
}
