using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class NewsTicker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textObject;

    [SerializeField]
    private GameObject tickerImage;

    private List<string> textQueue = new List<string>();

    private RectTransform  m_textRectTransform;
    private TextMeshProUGUI m_cloneTextObj;

    private string currentText = "";

    [SerializeField]
    private float scrollSpeed = 10;
    void Awake()
    {
        m_textRectTransform = textObject.GetComponent<RectTransform>();

    }
    
    IEnumerator Start(){
        float width = tickerImage.GetComponent<RectTransform>().rect.width + textObject.preferredWidth;
        //Debug.Log("Width: " + width);
        Vector3 startPosition = m_textRectTransform.position;

        float scrollPosition = 0;

        while (true){
            //Changing text doesnt work - 420 isnt working and i dont know how to tell exactly how to tell when the text is exactly off screen to the left.
            if(textQueue.Count > 0 && (int)(scrollPosition % width) == 0)
            {
                Debug.Log("Changing ticker text to new text...");
                textObject.text = textQueue[0];
                textQueue.RemoveAt(0);
                width = tickerImage.GetComponent<RectTransform>().rect.width + textObject.preferredWidth;
                scrollPosition = 0;
                
            }

            m_textRectTransform.position = new Vector3((-scrollPosition % width - 50) + width, startPosition.y, startPosition.z);
            //Debug.Log((scrollPosition % width) + "                " + textQueue.Count);
            scrollPosition += scrollSpeed * 30 * Time.deltaTime;
            //Debug.Log("Scroll Pos: " + (-scrollPosition % width));
            yield return null;
        }
    }

    //Use this method to update the ticker text
    public void UpdateText(string toText){
        Debug.Log("Added text to ticker queue. String is " + toText);
        textQueue.Add(toText);
    }

    // [SerializeField]
    // private GameObject tickerImage;

    // [SerializeField]
    // private float scrollSpeed = 10;

    // private RectTransform  m_textRectTransform;
    // private TextMeshProUGUI m_cloneTextObj;


    // private string currentText = "";

    // void Awake(){
    //     m_textRectTransform = textObject.GetComponent<RectTransform>();

    //     m_cloneTextObj = Instantiate(textObject);
    //     RectTransform cloneRectTransform = m_cloneTextObj.GetComponent<RectTransform>();
    //     cloneRectTransform.SetParent(m_textRectTransform);
    //     cloneRectTransform.anchorMin = new Vector2(1, .5f);
    //     cloneRectTransform.localScale = new Vector3(1,1,1);


    // }

    // IEnumerator Start(){
    //     RectTransform tickerRect = tickerImage.GetComponent<RectTransform>();
    //     float width = tickerRect.rect.width;
    //     Vector3 startPosition = new Vector3(tickerRect.position.x + tickerRect.rect.width, tickerRect.position.y, tickerRect.position.z);

    //     float scrollPosition = 0;

    //     while (true){
    //         if(textObject.text != currentText)
    //         {
    //             width = textObject.preferredWidth;
    //             m_cloneTextObj.text = textObject.text;
    //             currentText = textObject.text;
    //         }

    //         m_textRectTransform.position = new Vector3(-scrollPosition % width, startPosition.y, startPosition.z);
    //         scrollPosition += scrollSpeed * 20 * Time.deltaTime;

    //         yield return null;
    //     }
    // }
    // void Update(){

    // }
}
