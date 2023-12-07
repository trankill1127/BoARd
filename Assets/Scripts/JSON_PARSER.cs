using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class JSON_PARSER : MonoBehaviour
{
    private TextAsset json_Data;
    private static JSON_PARSER parser;

    public static JSON_PARSER instance
    {
        get
        {
            if (parser == null)
            {
                parser = new JSON_PARSER();
            }
            return parser;
        }
    }

    public post_list readJSON(string filename)
    {
        try
        {
            json_Data = Resources.Load<TextAsset>(filename);
            Debug.Log(json_Data);
        }
        catch
        {
            Debug.Log("파일이 존재하지 않습니다.");
        }
        post_list tmp = JsonUtility.FromJson<post_list>(json_Data.text);
        return tmp;
    }
    private void OnDestroy()
    {
        parser = null;
    }
}
[Serializable]
public class location_data
{
    public string Name;
    public float latitude;
    public float longitude;
}
public class post_list
{
    public List<location_data> post = new List<location_data>();
}
