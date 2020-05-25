using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;
public class ReadStream : MonoBehaviour
{
    internal string currCopy;
    internal string copyBuffer;
    internal Button search_btn;
    public SimplePool smallpool;
    internal Transform smallpoolParent;
    internal Text copyTxt;
    internal Toggle ccstog;
    internal Toggle csdtog;
    internal Toggle nodenametog;
    internal string arg = "1";
    private void Awake()
    {
        search_btn = transform.Find("search_btn").GetComponent<Button>();
        search_btn.onClick.AddListener(() =>
        {
            for (int i = 0; i < gamesList.Count; i++)
            {
                smallpool.push(gamesList[i]);
            }
            gamesList.Clear();
            GetSytemCopy();
        });
        smallpoolParent = transform.Find("Scroll View/Viewport/Content");
        copyTxt = transform.Find("copyTxt").GetComponent<Text>();

        ccstog = transform.Find("toggleParent/Toggle").GetComponent<Toggle>();
        ccstog.onValueChanged.AddListener(boo => { if (boo) arg = "1"; });
        csdtog = transform.Find("toggleParent/Toggle (1)").GetComponent<Toggle>();
        csdtog.onValueChanged.AddListener(boo => { if (boo) arg = "2"; });
        nodenametog = transform.Find("toggleParent/Toggle (2)").GetComponent<Toggle>();
        nodenametog.onValueChanged.AddListener(boo => { if (boo) arg = "3"; });


        InvokeRepeating("UpdateCopy", 1, 1);
    }
    /// <summary>
    /// 获取系统拷贝内容
    /// </summary>
    void GetSytemCopy()
    {
        var workdir = Environment.CurrentDirectory + "/python/";
        var results = EdtUtil.RunCmd("python", workdir + "HelloWorld.py " + ReadTargetDirectory() + " " + arg, workdir);
        //Debug.Log("python output: " + results[0]);
        //Debug.Log("python error: " + results[1]);
        if (results[1].Contains("Specified clipboard format is not available"))
        {
            copyTxt.text = "亲 复制失败，再试一次吧！";
        }
        else if (string.Empty == results[1])
        {
            copyTxt.text = "搜索完毕！找到" + (results[0].Split('\n').Length - 1) + "条数据";
        }
        for (int i = 0; i < results[0].Split('\n').Length - 1; i++)
        {
            //Debug.Log(i + "::::" + results[0].Split('\n')[i].Replace("\'", "\""));
            string rec = results[0].Split('\n')[i];
            GameObject games = smallpool.pop();
            ItemInfo json = JsonUtility.FromJson<ItemInfo>(rec.Replace("\'", "\""));
            games.GetComponent<ListPre>().InitInfo(json.name, json.path);
            games.transform.SetParent(smallpoolParent);
            gamesList.Add(games);
        }

        // c#抛出异常
        if (results[0].LastIndexOf("Error") > 0)
        {
            copyTxt.text = "python Error";
            throw new System.Exception("python Error");
        }
    }

    void UpdateCopy()
    {
        copyBuffer = GUIUtility.systemCopyBuffer;
        if (currCopy != copyBuffer)
        {
            currCopy = copyBuffer;
            copyTxt.text = currCopy;
        }
    }

    /// <summary>
    /// 读取目标目录
    /// </summary>
    /// <returns></returns>
    #region
    public string ReadTargetDirectory()
    {
        string path = Environment.CurrentDirectory;
        string parentDire = Directory.GetParent(path).FullName;
        Debug.Log("查看目标目录" + parentDire); //查看目标目录
        return parentDire;
    }
    #endregion

    /// <summary>
    /// 查询目标目录下所有文件---------关闭
    /// </summary>
    #region
    public void QueryAllFile()
    {

        DirectoryInfo directoryInfo = new DirectoryInfo(ReadTargetDirectory());
        FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i].Directory.Name == "cocosstudio")
                return;
            if (fileInfos[i].Name.EndsWith(".ccs"))
            {
                //Debug.Log("全部工程路径：" + fileInfos[i].FullName);
                //OpenCCS(fileInfos[i].FullName, fileInfos[i].Name);
            }
        }
    }
    #endregion

    /// <summary>
    /// 根据内容搜索目标工程路径---------关闭
    /// </summary>
    /// <param name="path"></param>
    #region
    private List<GameObject> gamesList = new List<GameObject>();
    void OpenCCS(string path, string rootname)
    {
        string[] readtext = File.ReadAllLines(path);
        for (int i = 0; i < readtext.Length; i++)
        {
            if (readtext[i].Contains(currCopy))//进行内容对比
            {
                //Debug.Log("目标工程路径" + path);
                GameObject games = smallpool.pop();
                games.GetComponent<ListPre>().InitInfo(readtext[i].Split('"')[1], path.Substring(0, path.Length - rootname.Length));
                games.transform.SetParent(smallpoolParent);
                games.SetActive(true);
                gamesList.Add(games);
            }
            else
            {
                //fail.SetActive(true);
                //Debug.Log("不存在(---" + currCopy + "---)次画布");
            }
        }
    }
    #endregion
}
[Serializable]
public class ItemInfo
{
    public string name;
    public string path;
}
