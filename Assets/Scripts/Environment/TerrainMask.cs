using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMask : MonoBehaviour
{
    public List<GameObject> objectsNeedFinish = new List<GameObject>();

    public List<GameObject> objectsToEnable = new List<GameObject>();

    public bool canTrigger { get; set; }

    private void Start()
    {
        GameManager.instance.IsObjectToFinish(gameObject);
    }

    private void Update()
    {
        foreach (var obj in objectsNeedFinish)
        {
            if (obj.activeInHierarchy)
            {
                return;
            }
        }
        canTrigger = true;
    }

    public void OnTriggerEvent()
    {
        foreach (var obj in objectsToEnable)
            obj.SetActive(true);

        GameManager.instance.ObjectFinish(gameObject);
    }
}
