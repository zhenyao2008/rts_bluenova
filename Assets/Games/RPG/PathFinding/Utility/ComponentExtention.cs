///
/// @file   ComponentExtension.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{
    [System.Obsolete]
    static public class ComponentExtension
    {
        static public T GetOrAddComponent<T>(this GameObject comp) where T : Component
        {
            T result = comp.GetComponent<T>();
            if (result == null)
            {
                result = comp.AddComponent<T>();
            }
            return result;
        }
    }
}
