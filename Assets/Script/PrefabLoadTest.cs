using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoadTest : MonoBehaviour
{
    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Darth_Artisan/Free_Trees/Prefabs/Palm_Tree");
        if (prefab == null)
        {
            Debug.LogError("������ �ε� ����");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
