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

    public post_data readJSON(string filename)
    {
        Debug.Log(filename);
        try
        {
            json_Data = Resources.Load<TextAsset>(filename);
        }
        catch
        {
            Debug.Log("파일이 존재하지 않습니다.");
        }
        post_data tmp = JsonUtility.FromJson<post_data>(json_Data.text);
        return tmp;
    }
    private void OnDestroy()
    {
        parser = null;
    }
}
[Serializable]
public class post_data
{
    public string title;
    public string contents;
}
