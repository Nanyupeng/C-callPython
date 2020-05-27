using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
public class LayoutManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    private const int SW_HIDE = 2;  //hied task bar

    private IntPtr window;

    public void HideTaskBar()//最小化到托盘
    {
        try
        {
            window = GetForegroundWindow();

            ShowWindow(window, SW_HIDE);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}

