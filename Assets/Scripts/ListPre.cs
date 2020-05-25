using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ListPre : MonoBehaviour
{
    internal Text targetname;
    internal Text targetpath;
    internal Button btn;
    void Awake()
    {
        targetname = transform.Find("targetname").GetComponent<Text>();
        targetpath = transform.Find("targetpath").GetComponent<Text>();
        btn = transform.Find("btn").GetComponent<Button>();
    }

   public void InitInfo(string name, string path)
    {
        targetname.text = "画布名称：" + name;
        targetpath.text = "画布路径：" + path;
        btn.onClick.AddListener(() => OpenPath(path));
    }

    void OpenPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return;
        path = path.Replace("/", "\\");
        Process.Start("explorer.exe", path);
    }
}
