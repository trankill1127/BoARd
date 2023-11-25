using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameObjectCreator : MonoBehaviour
{
    void CreateGeospatialAnchor(double latitude, double longitude, string gameObjectName)
    {
        // ���ο� GameObject ����
        GameObject gameObject = new GameObject();

        // ARGeospatialCreatorAnchor Ŭ������ �ν��Ͻ� ����
        ARGeospatialCreatorAnchor geospatialAnchor = gameObject.AddComponent<ARGeospatialCreatorAnchor>();

        // Latitude, Longitude, AltType ���� ����
        geospatialAnchor.Latitude = latitude;   // ����
        geospatialAnchor.Longitude = longitude; // �浵
        geospatialAnchor.AltType = ARGeospatialCreatorAnchor.AltitudeType.Terrain; // AltitudeType�� Terrain���� ����

        // Fir_Tree.prefab �ε�
        // ������ �ε�
        GameObject palmTreePrefab = Resources.Load<GameObject>("Darth_Artisan/Free_Trees/Prefabs/Palm_Tree");
        if (palmTreePrefab == null)
        {
            Debug.LogError("�������� �ε��ϴµ� �����߽��ϴ�.");
            return; // �ε� ���� �� �Լ� ����
        }

        // palmTreePrefab�� gameObject�� �ڽ����� �߰�
        GameObject instantiatedTree = Instantiate(palmTreePrefab, gameObject.transform);
        Debug.Log("Instantiated Tree: " + instantiatedTree);

        // palmTreePrefab �� ũ������
        instantiatedTree.transform.localScale = new Vector3(10f, 10f, 10f);
        // GameObject �̸� ����
        gameObject.name = gameObjectName;
    }
    void Start()
    {
        // �Լ� ȣ��
        CreateGeospatialAnchor(37.7749, 1, "Test Component Game Object");
    }

    void Update()
    {
        
    }
}
