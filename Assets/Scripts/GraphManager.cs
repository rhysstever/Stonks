using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour
{
    [SerializeField] public Sprite circleSprite;
    private RectTransform graphContainer;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

        //CreateCircle(new Vector2(20, 20));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer.GetChild(1), false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.localScale = new Vector3(0.5f, 1, 1);
        rectTransform.sizeDelta = new Vector2(10, 10);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    public void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = 50f;
        float yMaximum = 100f;
        for (int i = 0; i < valueList.Count; i++)
        {       
            float xPosition = i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            CreateCircle(new Vector2(xPosition, yPosition));
        }
    }

    public void cleanupPrevious()
    {
        foreach(Transform child in graphContainer.GetChild(1))
        {
            Destroy(child.gameObject);
        }
    }
}
