using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using System;
using Google.XR.ARCoreExtensions.Internal;
using UnityEngine.XR.ARSubsystems;
using CesiumForUnity;



public class ARcreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ARGeospatialCreatorAnchor anchor = new GameObject("CubeAnchor").AddComponent<ARGeospatialCreatorAnchor>();
        //anchor.Origin = origin;
        //ARAnchorManager anchorManager = Resources.FindObjectsOfTypeAll<ARAnchorManager>()[0];
        //anchor.AnchorManager = anchorManager;
        anchor.Latitude = 37.544577999984973;
        anchor.Longitude = 127.07258299936413;
        anchor.AltType = ARGeospatialCreatorAnchor.AltitudeType.Terrain; // AltitudeType을 Terrain으로 변경
        //ARAnchorManager anchorManager =
        //    ARCoreExtensions._instance.SessionOrigin.GetComponent<ARAnchorManager>();
        //anchor.ECEF = new double3(-3052354.7323507764, 4039953.1422214927, 3865500.2011826811);
        //anchor.ECEF.x = -3052354.7323507764;
        //anchor.ECEF.y = 4039953.1422214927;
        //anchor.ECEF.z = 3865500.2011826811;
        //anchor.EUN.x = 14.139643669128418;
        //anchor.EUN.y = 0.414009153842926;
        //anchor.EUN.z = -32.74151611328125;
        //anchor.EUS.x = 14.139643669128418;
        //anchor.EUS.y = 0.414009153842926;
        //anchor.EUS.z = 32.74151611328125;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = anchor.transform;
        cube.transform.localScale = new Vector3(30, 30, 30);
        //anchor.UseEditorOverride = true;


        //GameObject SphereAnchor = GameObject.Find("SphereAnchor");
        //ARGeospatialCreatorAnchor arGeospatialCreatorAnchor = SphereAnchor.GetComponent<ARGeospatialCreatorAnchor>();
        //arGeospatialCreatorAnchor.Latitude = 30;
        //arGeospatialCreatorAnchor.enabled = true;


        Debug.Log(anchor.Latitude + ", " + anchor.Longitude);
        GameObject UI = GameObject.Find("UI");
        Destroy(UI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
