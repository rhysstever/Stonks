using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    GameManager gm;
    
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
        //Get the static GM instance
        gm = GameManager.Instance;

        // Sets up multiplier buttons and sets their onClicks
        List<Button> multiplierButtons = new List<Button>();
        multiplierButtons.Add(canvas.transform.Find("Multiplier1").gameObject.GetComponent<Button>());
        multiplierButtons.Add(canvas.transform.Find("Multiplier5").gameObject.GetComponent<Button>());
        multiplierButtons.Add(canvas.transform.Find("Multiplier10").gameObject.GetComponent<Button>());
        multiplierButtons.Add(canvas.transform.Find("Multiplier25").gameObject.GetComponent<Button>());

        foreach(Button multiplierButton in multiplierButtons) {
            multiplierButton.onClick.AddListener(() =>
                gameObject.GetComponent<GameManager>().ChangeMultipler(
                int.Parse(multiplierButton.transform.GetChild(0).gameObject.GetComponent<Text>().text.Substring(1))));
        }
        
        // Sets up onClicks for buying stocks
        foreach(Stock stock in gm.market.StockList.Values) {
            string stockButtonName = "Button_" + stock.Name;
            canvas.transform.Find(stockButtonName).gameObject.GetComponent<Button>().onClick.AddListener(() =>
                stock.BuyStock(gameObject.GetComponent<GameManager>().multiplier));
        }
    }

    void UpdateUI()
    {
        canvas.transform.Find("Text_Money").gameObject.GetComponent<Text>().text = "$" + gameObject.GetComponent<GameManager>().money.ToString("0.00");

        foreach(Stock stock in gm.market.StockList.Values) {
            string stockTextName = "Text_" + stock.Name;
            canvas.transform.Find(stockTextName).gameObject.GetComponent<Text>().text = 
                stock.Name + " Shares: " + stock.SharesOwned;

            string stockButtonName = "Button_" + stock.Name;
            canvas.transform.Find(stockButtonName).gameObject.GetComponent<Button>().transform.GetChild(0).gameObject.GetComponent<Text>().text = 
                "Buy: $" + (stock.PricePerShare * gameObject.GetComponent<GameManager>().multiplier);
        }
    }
}
