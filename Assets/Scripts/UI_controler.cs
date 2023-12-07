using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_controler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI contents1;
    [SerializeField]
    TextMeshProUGUI contents2;
    private JSON_PARSER parser;
    private string F_NAME = "tmp";
    /*private void Start()
    {
        Debug.Log("start");
        parser = JSON_PARSER.instance;
        post_list data = parser.readJSON(F_NAME);
    }*/
    public void OnButtonClick()
    {
        Application.OpenURL("https://www.all-con.co.kr/view/contest/462840");
    }
    public void Sejong()
    {
        Application.OpenURL("http://www.sejong.ac.kr/");
    }
}
