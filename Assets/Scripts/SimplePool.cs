using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimplePool : MonoBehaviour
{
    public GameObject goPrefabs;
    //对象池子
    private List<GameObject> listpool = new List<GameObject>();
    //池子里面的物体个数
    public int poolCount = 10;

    //给池子添加物体
    public void push(GameObject go)
    {
        //如果小与池子中元素的个数
        if (listpool.Count < poolCount)
        {
            //每次都让创建的物体回到原来位置
            go.transform.position = goPrefabs.transform.position;
            go.SetActive(false);
            listpool.Add(go);
        }
        else
        {
            //多了就摧毁
            Destroy(go);
        }
    }
    //将池子物体移除
    public GameObject pop()
    {
        if (listpool.Count > 0)
        {
            GameObject go = listpool[0];
            listpool.Remove(listpool[0]);
            go.SetActive(true);
            return go;
        }
        else
        {
            //如果没有物体就生成预制体
            GameObject go = Instantiate(goPrefabs);
            go.SetActive(true);
            return go;
        }
    }

    public void removeList()
    {
        for (int i = 0; i < listpool.Count; i++)
        {
            Destroy(listpool[i]);
        }
        listpool.Clear();
    }

}