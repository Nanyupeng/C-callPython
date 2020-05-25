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
    internal SimplePool simplePool;
    void Awake()
    {
        targetname = transform.Find("targetname").GetComponent<Text>();
        targetpath = transform.Find("targetpath").GetComponent<Text>();
        btn = transform.Find("btn").GetComponent<Button>();
    }

    public void InitInfo(string name, string path, SimplePool simple)
    {
        this.simplePool = simple;
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

    public void pushthis()
    {
        //UnityEngine.Debug.Log("进入方法");
        if (simplePool != null)
        {
            //UnityEngine.Debug.Log("simplePool不为空");
            btn.onClick.RemoveAllListeners();
            simplePool.push(this.gameObject);
        }
    }
}
