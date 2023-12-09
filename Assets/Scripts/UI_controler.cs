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
    public void univ()
    {
        Application.OpenURL("https://www.all-con.co.kr/view/contest/462840");
    }
    public void Sejong()
    {
        Application.OpenURL("http://www.sejong.ac.kr/");
    }
    public void ssafy()
    {
        Application.OpenURL("https://www.ssafy.com/ksp/servlet/swp.content.controller.SwpContentServlet?p_process=select-content-view&p_menu_cd=M0306&p_content_cd=C0306");
    }
    public void Tosc()
    {
        Application.OpenURL("http://www.tosc.kr/");
    }
    public void adogen()
    {
        Application.OpenURL("https://m.facebook.com/sejongIME/posts/2308840419385593/?locale=ms_MY");
    }
    public void soyung()
    {
        Application.OpenURL("http://sejongsc.co.kr/");
    }
    public void Pic()
    {
        Application.OpenURL("https://home.sejong.ac.kr/bbs/bbsview.do?bbsid=2049&pkid=16869&wslID=scc&currentPage=1&searchValue=&searchField=");
    }
    public void yasic()
    {
        Application.OpenURL("https://m.facebook.com/SJCESC/photos/a.312053695611127.1073741829.143385535811278/452719481544547/?locale=el_GR");
    }
}
