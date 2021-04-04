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

    // Work Buttons
    [SerializeField]
    GameObject workButton;

    [SerializeField]
    GameObject buyPromotionButton;

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

    // Upgrade Buttons
    [SerializeField]
    GameObject upgradeClickWeightButton;

    [SerializeField]
    GameObject upgradeRaiseButton;

    [SerializeField]
    GameObject upgradeInflationButton;

    // Upgrade Display Text
    [SerializeField]
    TextMeshProUGUI currentClickWeightText;

    [SerializeField]
    TextMeshProUGUI currentRaiseText;

    [SerializeField]
    TextMeshProUGUI currentInflationText;

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

        buyPromotionButton.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<WorkManager>().PromotionBuyOut());

        upgradeClickWeightButton.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<UpgradesManager>().BuyUpgrade<int>(gameObject.GetComponent<UpgradesManager>().currentClickWeight));

        upgradeRaiseButton.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<UpgradesManager>().BuyUpgrade<float>(gameObject.GetComponent<UpgradesManager>().currentRaise));

        upgradeInflationButton.GetComponent<Button>().onClick.AddListener(() =>
                gameObject.GetComponent<UpgradesManager>().BuyUpgrade<float>(gameObject.GetComponent<UpgradesManager>().currentInflation));

        gm.ChangeMultipler(1);
        multiply1.GetComponent<Button>().Select();

        GraphName = GameObject.Find("GraphName");
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

            if(price > gm.money)
            {
                stockButtons[i].interactable = false;
                stockButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Not enough money!";
            }
            else
            {
                stockButtons[i].interactable = true;

                stockButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Buy " + gm.multipliers[i] + " shares for " + price.ToString("C");
            }

        }

        if(gameObject.GetComponent<WorkManager>().buyoutCost > gm.money)
        {
            buyPromotionButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            buyPromotionButton.GetComponent<Button>().interactable = true;

        }

        // Update job display text
        currentPosition.text = "Current Position: " + gameObject.GetComponent<WorkManager>().currentJob.PositionTitle;
        payRate.text = "Pay Rate: " + gameObject.GetComponent<WorkManager>().currentJob.HourlyPay.ToString("C");
        int clicksRemainingCount = (int)gameObject.GetComponent<WorkManager>().currentJob.ClicksToPromotion - gameObject.GetComponent<WorkManager>().currentClicks;
        clicksRemaining.text = "Clicks Remaining: " + clicksRemainingCount;
        buyOutCost.text = "Buyout Cost: " + gameObject.GetComponent<WorkManager>().buyoutCost.ToString("C");

        // Update Upgrade button text
        if(gameObject.GetComponent<UpgradesManager>().currentClickWeight.Next != null) {
            upgradeClickWeightButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                "Upgrade Click to " + 
                gameObject.GetComponent<UpgradesManager>().currentClickWeight.Next.Data + 
                "x \nfor " + 
                gameObject.GetComponent<UpgradesManager>().currentClickWeight.Next.Cost.ToString("C");

                if(gameObject.GetComponent<UpgradesManager>().currentClickWeight.Next.Cost > gm.money)
                {
                    upgradeClickWeightButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    upgradeClickWeightButton.GetComponent<Button>().interactable = true;
                }
		} else
        {
            upgradeClickWeightButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Upgrade Maxed";
            upgradeClickWeightButton.GetComponent<Button>().interactable = false;

        }

        if(gameObject.GetComponent<UpgradesManager>().currentRaise.Next != null) {
            upgradeRaiseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "Upgrade Raise to " +
                ((gameObject.GetComponent<UpgradesManager>().currentRaise.Next.Data - 1.0f) * 100).ToString("N0") + 
                "% \nfor " +
                gameObject.GetComponent<UpgradesManager>().currentRaise.Next.Cost.ToString("C");

                if(gameObject.GetComponent<UpgradesManager>().currentRaise.Next.Cost > gm.money)
                {
                    upgradeRaiseButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    upgradeRaiseButton.GetComponent<Button>().interactable = true;
                }
        }
        else
        {
            upgradeRaiseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Upgrade Maxed";
            upgradeRaiseButton.GetComponent<Button>().interactable = false;
        }

        if(gameObject.GetComponent<UpgradesManager>().currentInflation.Next != null) {
            upgradeInflationButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "Upgrade to " +
                ((gameObject.GetComponent<UpgradesManager>().currentInflation.Next.Data - 1.0f) * 100).ToString("N0") +
                "% \nfor " +
                gameObject.GetComponent<UpgradesManager>().currentInflation.Next.Cost.ToString("C");

                if(gameObject.GetComponent<UpgradesManager>().currentInflation.Next.Cost > gm.money)
                {
                    upgradeInflationButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    upgradeInflationButton.GetComponent<Button>().interactable = true;
                }
        }
        else
        {
            upgradeInflationButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Upgrade Maxed";
            upgradeInflationButton.GetComponent<Button>().interactable = false;
        }

        // Update Upgrade display text
        currentClickWeightText.text = "Work Click Amount: " + gameObject.GetComponent<UpgradesManager>().currentClickWeight.Data + "x";
        currentRaiseText.text = "Pay Rate Multiplier: " + ((gameObject.GetComponent<UpgradesManager>().currentRaise.Data - 1.0f) * 100).ToString("N0") + "%";
        currentInflationText.text = "Stock Income Multiplier: " + ((gameObject.GetComponent<UpgradesManager>().currentInflation.Data - 1.0f) * 100).ToString("N0") + "%";
    }
}
