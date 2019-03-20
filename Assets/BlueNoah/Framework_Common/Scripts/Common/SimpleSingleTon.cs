using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleSingleTon<T> where T : new()
{
    private static T t = default(T);

    public static T Instance
    {
        get
        {
            if (t == null)
            {
                t = new T();
            }
            return t;
        }
    }
}
