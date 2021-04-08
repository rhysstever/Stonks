using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    GameObject LaunchScreen;

    [SerializeField]
    GameObject JobUI;

    [SerializeField]
    GameObject News;

    [SerializeField]
    GameObject Purchasing;

    [SerializeField]
    GameObject Graph;

    [SerializeField]
    GameObject DebugMenu;

    [SerializeField]
    GameObject UpgradesMenu;

    [SerializeField]
    GameObject TutorialBox;

    private GameObject tutorialText;
    private int stepNum;

    // Start is called before the first frame update
    void Start()
    {
        JobUI.SetActive(false);
        News.SetActive(false);
        Purchasing.SetActive(false);
        Graph.SetActive(false);
        DebugMenu.SetActive(false);
        TutorialBox.SetActive(false);
        UpgradesMenu.SetActive(false);

        tutorialText = TutorialBox.transform.GetChild(1).gameObject;
        stepNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Step()
    {
        switch (stepNum)
        {
            case 1:
                LaunchScreen.SetActive(false);
                TutorialBox.SetActive(true);
                TutorialBox.transform.position = new Vector3(960, 540, 0);
                tutorialText.GetComponent<TextMeshProUGUI>().text = "Welcome to Stonks, the game where Stonks Only Go Up!";
                tutorialText.GetComponent<TextMeshProUGUI>().fontSize = 47;
                stepNum = 2;
                break;
            case 2:
                JobUI.SetActive(true);
                TutorialBox.transform.position = new Vector3(1400, 700, 0);
                tutorialText.GetComponent<TextMeshProUGUI>().text = "This is where it all starts: clicking that button to work a job and make enough money to buy stocks.";
                tutorialText.GetComponent<TextMeshProUGUI>().fontSize = 34;
                stepNum = 3;
                break;
            case 3:
                JobUI.SetActive(false);
                Purchasing.SetActive(true);
                TutorialBox.transform.position = new Vector3(1400, 540, 0);
                tutorialText.GetComponent<TextMeshProUGUI>().text = "Over here you can see how much money you have. You can also buy stocks and see their current prices.";
                tutorialText.GetComponent<TextMeshProUGUI>().fontSize = 36;
                stepNum = 4;
                break;
            case 4:
                Purchasing.SetActive(false);
                News.SetActive(true);
                TutorialBox.transform.position = new Vector3(960, 540, 0);
                tutorialText.GetComponent<TextMeshProUGUI>().text = "Up top is the news ticker, where you'll hear about stocks rapidly rising or falling.";
                tutorialText.GetComponent<TextMeshProUGUI>().fontSize = 40;
                stepNum = 5;
                break;
            case 5:
                News.SetActive(false);
                Graph.SetActive(true);
                TutorialBox.transform.position = new Vector3(700, 540, 0);
                tutorialText.GetComponent<TextMeshProUGUI>().text = "This graph displays a stock graph of the currently selected stock.";
                tutorialText.GetComponent<TextMeshProUGUI>().fontSize = 44;
                stepNum = 6;
                break;
            case 6:
                Graph.SetActive(false);
                TutorialBox.transform.position = new Vector3(960, 540, 0);
                tutorialText.GetComponent<TextMeshProUGUI>().text = "That's it! Hit continue to start your journey in the stonk market.";
                tutorialText.GetComponent<TextMeshProUGUI>().fontSize = 44;
                stepNum = 7;
                break;
            case 7:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
                break;
            default:
                Debug.LogError("Reached an invalid point of the tutorial");
                break;
        }
    }
}
