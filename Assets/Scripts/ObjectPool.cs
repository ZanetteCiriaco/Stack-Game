using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public int MaximumOfObjects = 10;
    public GameObject ObjectPrefab;
    private List<GameObject> poolObjects = new List<GameObject>();
   
    void Start()
    {
        CreatePool();
    }

    void CreatePool() 
    {
        for (int i = 0; i < MaximumOfObjects; i++)
        {
            var newObj = Instantiate(ObjectPrefab);
            newObj.name = "Block " + i;
            newObj.SetActive(false);
            poolObjects.Add(newObj);
        }
    }

    public GameObject GetGameObjectFromPool()
    {
        for (int i = 0; i < MaximumOfObjects; i++)
        {
            if(!poolObjects[i].activeInHierarchy) {
                return poolObjects[i];
            }
        }

        return null;
    }

    public void ReturnObjectToPool(int index) {
        if (index >= 0 && index < poolObjects.Count) {
            poolObjects[index].SetActive(false);
        }
    }

    public void ReturnObjectToPool(Block block) {
        block.gameObject.SetActive(false);
    }
}
