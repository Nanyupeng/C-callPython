using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class StringCopy
{
    internal static string currCopy;
    internal static string copyBuffer;

    public static string GetSytemCopy()
    {
        copyBuffer = GUIUtility.systemCopyBuffer;
        if (currCopy != copyBuffer)
        {
            currCopy = copyBuffer;
            //Debug.Log(currCopy);
            return currCopy;
        }
        return null;
    }
}
