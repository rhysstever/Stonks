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

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer.GetChild(1), false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.localScale = new Vector3(0.25f, .5f, 1);
        rectTransform.sizeDelta = new Vector2(10, 10);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    public void ShowGraph(List<float> valueList)
    {
        float currentMin = float.MaxValue;
        float currentMax = 0;

        foreach(float f in valueList)
        {
            if (f > currentMax) currentMax = f;
            if (f < currentMin) currentMin = f;
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = 10f;
        float yMaximum = 100f;
        float xMaximum = 100f;

        GameObject lastCircleGameObject = null;

        for (int i = 1; i < valueList.Count; i++)
        {       
            float xPosition = i * xSize;

            //float mx = (xMaximum) / valueList.Count;
            //float xPosition = xMaximum - (mx * i);

            float m = (yMaximum) / (currentMax - currentMin);
            float yPosition = m * valueList[i] - (m * currentMin);

            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if(lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    public void cleanupPrevious()
    {
        foreach(Transform child in graphContainer.GetChild(1))
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in graphContainer.GetChild(2))
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer.GetChild(2), false);

        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(10, 3f);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(angle >= 0)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 1, 0, .5f);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(1, 0, 0, .5f);
        }

        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
