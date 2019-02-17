﻿using UnityEngine;
using System.Collections;

public class SimpleSingleMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T t;

    public static T Instance
    {
        get
        {
            if (t == null)
            {
                t = FindObjectOfType(typeof(T)) as T;
            }
            return t;
        }
    }

    protected virtual void Awake()
    {
        if (t == null)
        {
            t = gameObject.GetComponent<T>();
        }
    }

    protected bool isInited;


}
