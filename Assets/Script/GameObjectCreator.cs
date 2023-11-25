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
        // 새로운 GameObject 생성
        GameObject gameObject = new GameObject();

        // ARGeospatialCreatorAnchor 클래스의 인스턴스 생성
        ARGeospatialCreatorAnchor geospatialAnchor = gameObject.AddComponent<ARGeospatialCreatorAnchor>();

        // Latitude, Longitude, AltType 변수 설정
        geospatialAnchor.Latitude = latitude;   // 위도
        geospatialAnchor.Longitude = longitude; // 경도
        geospatialAnchor.AltType = ARGeospatialCreatorAnchor.AltitudeType.Terrain; // AltitudeType을 Terrain으로 변경

        // Fir_Tree.prefab 로드
        // 프리팹 로드
        GameObject palmTreePrefab = Resources.Load<GameObject>("Darth_Artisan/Free_Trees/Prefabs/Palm_Tree");
        if (palmTreePrefab == null)
        {
            Debug.LogError("프리팹을 로드하는데 실패했습니다.");
            return; // 로드 실패 시 함수 종료
        }

        // palmTreePrefab을 gameObject의 자식으로 추가
        GameObject instantiatedTree = Instantiate(palmTreePrefab, gameObject.transform);
        Debug.Log("Instantiated Tree: " + instantiatedTree);

        // palmTreePrefab 의 크기조정
        instantiatedTree.transform.localScale = new Vector3(10f, 10f, 10f);
        // GameObject 이름 설정
        gameObject.name = gameObjectName;
    }
    void Start()
    {
        // 함수 호출
        CreateGeospatialAnchor(37.7749, 1, "Test Component Game Object");
    }

    void Update()
    {
        
    }
}
