using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMask : MonoBehaviour
{
    public List<GameObject> objectsToEnable = new List<GameObject>();

    private void Start()
    {
        GameManager.instance.IsObjectToFinish(gameObject);
    }

    public void OnTriggerEvent()
    {
        foreach (var obj in objectsToEnable)
            obj.SetActive(true);

        GameManager.instance.ObjectFinish(gameObject);
    }
}
