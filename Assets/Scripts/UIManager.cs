using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public List<InfoData> infoDatas;

    void Awake()
    {
        Instance = this;
    }

    public void OffAllPanel()
    {
          for (int i = 0; i <Instance.infoDatas.Count; i++)
        {
            Instance.infoDatas[i].infoPanel.SetActive(false);
        }
    }
}
