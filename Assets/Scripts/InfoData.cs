using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoData : MonoBehaviour
{
    public Vector3 heartRoation;
    public Button infoButton;
    public GameObject infoPanel;

    void Start()
    {
        infoButton.onClick.AddListener(ShowInfoPanel);
    }

    public void ShowInfoPanel()
    {
        for (int i = 0; i < UIManager.Instance.infoDatas.Count; i++)
        {
            UIManager.Instance.infoDatas[i].infoPanel.SetActive(false);
        }
        infoPanel.SetActive(true);

        //ARObjectPlacerManager.Instance.instantiatedObject.transform.rotation = Quaternion.Euler(heartRoation);
    }
}
