using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    GameManager gm;
    public Canvas canvas;
    
    [SerializeField]
    GameObject GameUIPanel;

    [SerializeField]
    TextMeshProUGUI moneyText;


    //Multiplier Buttons
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

        stockButtons = new List<Button>();
        stockTexts = new List<TextMeshProUGUI>();

        // foreach(Stock stock in gm.market.StockList.Values) {
        //     // Finds all stock button objects, adds it to a list, and sets their onClicks
        //     string stockButtonName = "Button_" + stock.Name;
        //     GameObject newButton = GameUIPanel.transform.Find(stockButtonName).gameObject;
        //     newButton.GetComponent<Button>().onClick.AddListener(() => stock.BuyStock(gameObject.GetComponent<GameManager>().multiplier));
        //     stockButtons.Add(newButton.GetComponent<Button>());

        //     // Finds all stock text objects and adds it to a list
        //     string stockTextName = "Text_" + stock.Name;
        //     GameObject newText = GameUIPanel.transform.Find(stockTextName).gameObject;
        //     stockTexts.Add(newText.GetComponent<TextMeshProUGUI>());
        // }

        //Delete the placeholders, we'll readd them with code. 
        foreach (Transform child in stockDisplays.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach(Stock stock in gm.market.StockList.Values) {
            GameObject newStock = Instantiate(stockDisplayPrefab);
            newStock.transform.SetParent(stockDisplays.transform);
            newStock.name = stock.Name;
            GameObject buyButton = newStock.transform.Find("BUY_BUTTON").gameObject;
            buyButton.GetComponent<Button>().onClick.AddListener(() => stock.BuyStock(gameObject.GetComponent<GameManager>().multipliers[0]));
            stockButtons.Add(buyButton.GetComponent<Button>());

            // Finds all stock text objects and adds it to a list
            string stockTextName = stock.Name;
            GameObject newText = newStock.transform.Find("STOCK_NAME").gameObject;
            TextMeshProUGUI tmpAsset = newText.GetComponent<TextMeshProUGUI>();
            newText.name = stockTextName;
            stockTexts.Add(tmpAsset);
        }


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
                gameObject.GetComponent<GameManager>().CalculateMaxMultiplier());

        gm.ChangeMultipler(1);
        multiply1.GetComponent<Button>().Select();
    }

    // Update the multiplied costs on each stocks' buy button
    public void UpdateMultiplicity()
    {

    }

    void UpdateUI()
    {
        // Update money display text
        moneyText.text = "$" + gameObject.GetComponent<GameManager>().money.ToString("0.00");

        for(int i = 0; i < gm.market.StockList.Values.Count; i++ )
        {
            Stock currentStock = gm.market.StockList[stockTexts[i].gameObject.name];
            stockTexts[i].text = currentStock.Name + " - Shares: " + currentStock.SharesOwned;

            // Calculates the total price of the purchase, based on the multiplier, then displays it in the currency format
            float price = currentStock.PricePerShare * gameObject.GetComponent<GameManager>().multipliers[i];
            stockButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Buy " + gameObject.GetComponent<GameManager>().multipliers[i] + " shares for " + price.ToString("C");
        }
    }
}
