using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPSManager : MonoBehaviour
{
    public Text text_ui;
    public GameObject welcome_popUp;
    public bool isFirst = false;

    public double[] lats;
    public double[] longs;

    IEnumerator Start()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            yield return null;
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start(10, 1);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);


            
            int index = 0;
            while (true)
            {
                yield return null;



                if (SystemInfo.supportsGyroscope)
                {
                    //현재위치 위도,경도
                    double myLat = Input.location.lastData.latitude;
                    double myLong = Input.location.lastData.longitude;
                    //현재위치로부터 lats[0] 와 long[0] 까지의 거리
                    double remainDistance = distance(myLat, myLong, lats[0], longs[0]);
                    if (remainDistance <= 10f) // 10m
                    {
                        if (lats.Length - 1 > index)
                        {
                            lats[0] = lats[++index];
                            longs[0] = longs[index];
                        }
                        else
                        {
                            welcome_popUp.SetActive(false);
                        }
                    }


                    double lat1 = myLat, lon1 = myLong;  // 출발지 좌표
                    double lat2 = lats[0], lon2 = longs[0];  // 도착지 좌표
                                                                 // 방위각 계산
                    double result = CalculateBearing(lat1, lon1, lat2, lon2);
                    Gyroscope gyro = Input.gyro;
                    gyro.enabled = true;
                    Input.compass.enabled = true;

                    transform.rotation = gyro.attitude;
                    //Quaternion.Euler(0, -Input.compass.magneticHeading, 0);
                    text_ui.text = Input.location.lastData.latitude + " / " + Input.location.lastData.longitude + "\n" +
                                " Compass: " + Input.compass.trueHeading + "\n" +
                                "목적지: " + lat2 + "," + lon2 + "\n" +
                                "remaindistance: " + remainDistance + "\n" +
                                "화살표방향:" + (float)(Input.compass.trueHeading + result) % 360 + "\n" +
                                "방위각:" + result;
                    welcome_popUp.transform.rotation = Quaternion.Euler(0f, 0f, (float)(Input.compass.trueHeading+result)%360);

                }
            }
        }
    }

    void Update()
    {
        //if (Input.location.status == LocationServiceStatus.Running)
        //{
        //    //현재위치 위도,경도
        //    double myLat = Input.location.lastData.latitude;
        //    double myLong = Input.location.lastData.longitude;

        //    double remainDistance = distance(myLat, myLong, lats[0], longs[0]);

        //    if (remainDistance <= 215f) // 215m
        //    {
        //        if (!isFirst)
        //        {
        //            isFirst = true;
        //            welcome_popUp.SetActive(true);
        //        }
        //    }
        //}
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
    // 지표면 거리 계산 공식(하버사인 공식)
    private double distance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta = lon1 - lon2;

        double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));

        dist = Math.Acos(dist);

        dist = Rad2Deg(dist);

        dist = dist * 60 * 1.1515;

        dist = dist * 1609.344; // 미터 변환

        return dist;
    }

    private double Deg2Rad(double deg)
    {
        return (deg * Mathf.PI / 180.0f);
    }

    private double Rad2Deg(double rad)
    {
        return (rad * 180.0f / Mathf.PI);
    }



}