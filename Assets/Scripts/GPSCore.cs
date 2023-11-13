using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GPSCore : MonoBehaviour
{
    public float Lat;
    public float Lon;
    private double distance;
    private Vector3 TargetPosition;
    private Vector3 OriginalPosition;
    public float Radius = 5f;
    public float TimeUpdate = 3f;
    private string newlat;
    private string newlon;
    float lat;
    float lon;
    public GameObject TargetPopUp;
    public bool TargetPopUpOneTime = false;

    private void Start()
    {
        Input.location.Start();
        StartCoroutine("GPSProcess");
    }

    public IEnumerator GPSProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeUpdate);

            if (Input.location.isEnabledByUser == true)
            {
                Input.location.Start();

                lat = Input.location.lastData.latitude;
                newlat = lat.ToString();

                lon = Input.location.lastData.longitude;
                newlon = lon.ToString();

                Calculate(Lat, Lon, lat, lon);
            }
        }
    }

    public void Calculate(float lat1, float lon1, float lat2, float lon2)
    {
        var R = 6378.137;
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2)
            + Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180)
            * Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        distance = R * c;
        distance = distance * 1000f;
        float distanceFloat = (float)distance;
        
        TargetPosition = OriginalPosition - new Vector3(0, 0, distanceFloat * 12);

        if (distance < Radius)
        {
            if (TargetPopUpOneTime == false)
            {
                TargetPopUp.SetActive(true);
            }
        }
        if (distance > Radius)
        {
            TargetPopUp.SetActive(false);
        }
    }

    public void HideTargetPopUp()
    {
        TargetPopUp.SetActive(false);
        TargetPopUpOneTime = true;
    }
}
