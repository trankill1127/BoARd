using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class angle : MonoBehaviour
{
    // Start is called before the first frame update\

    void Start()
    {
        ARSession arSession = FindObjectOfType<ARSession>();
        // 예제 좌표
        double lat1 = 37.544867, lon1 = 127.072417;  // 출발지 좌표
        double lat2 = 37.545209, lon2 = 127.072545;  // 도착지 좌표

        // 방위각 계산
        double result = CalculateBearing(lat1, lon1, lat2, lon2);

        Debug.Log($"방위각: {result}도");
        if (arSession != null)
        {
            Debug.Log("arSession NULL@@@@@@@");
        }

        //Debug.Log(Quaternion.Euler(0, -Input.compass.magneticHeading, 0));

    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(0, -Input.compass.magneticHeading, 0);
    }


    static double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
    {
        // 위도 및 경도를 라디안으로 변환
        lat1 = ToRadians(lat1);
        lon1 = ToRadians(lon1);
        lat2 = ToRadians(lat2);
        lon2 = ToRadians(lon2);

        // 두 지점 간의 상대적인 경도 계산
        double deltaLon = lon2 - lon1;

        // 방위각 계산
        double x = Math.Atan2(Math.Sin(deltaLon), Math.Cos(lat1) * Math.Tan(lat2) - Math.Sin(lat1) * Math.Cos(deltaLon));
        double bearing = (ToDegrees(x) + 360) % 360;

        return bearing;
    }

    static double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }

    static double ToDegrees(double radians)
    {
        return radians * (180 / Math.PI);
    }
}
