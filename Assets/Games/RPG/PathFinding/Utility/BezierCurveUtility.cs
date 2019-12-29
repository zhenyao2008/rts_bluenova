using UnityEngine;
///
/// @file  BezierCurveUtility.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public static class BezierCurveUtility
    {
        //三つポイント。
        public static Vector3[] GetPositions(Vector3 startPos, Vector3 middlePos, Vector3 endPos,int splitCount)
        {
            Vector3[] positions = new Vector3[splitCount];
            float detalSplit = 1f / splitCount;
            for (int i = 0;i < splitCount;i++)
            {
                positions[i] = GetPosition(startPos,middlePos,endPos,i * detalSplit);
            }
            return positions;
        }
        //四つポイント
        public static Vector3[] GetPositions(Vector3 startPos, Vector3 middlePos1, Vector3 middlePos2, Vector3 endPos, int splitCount)
        {
            Vector3[] positions = new Vector3[splitCount];
            float detalSplit = 1f / splitCount;
            for (int i = 0; i < splitCount; i++)
            {
                positions[i] = GetPosition(startPos, middlePos1, middlePos2, endPos, i * detalSplit);
            }
            return positions;
        }
        //五つポイント
        public static Vector3[] GetPositions(Vector3 startPos, Vector3 middlePos1, Vector3 middlePos2, Vector3 middlePos3, Vector3 endPos, int splitCount)
        {
            Vector3[] positions = new Vector3[splitCount];
            float detalSplit = 1f / splitCount;
            for (int i = 0; i < splitCount; i++)
            {
                positions[i] = GetPosition(startPos, middlePos1, middlePos2, middlePos3, endPos, i * detalSplit);
            }
            return positions;
        }
        //三つポイントのCurve
        public static Vector3 GetPosition(Vector3 startPos, Vector3 middlePos, Vector3 endPos, float t)
        {
            Vector3 pos0 = Vector3.Lerp(startPos, middlePos, t);
            Vector3 pos1 = Vector3.Lerp(middlePos, endPos, t);
            return Vector3.Lerp(pos0, pos1, t);
        }
        //四つポイントのCurve
        public static Vector3 GetPosition(Vector3 startPos , Vector3 middlePos1,Vector3 middlePos2,Vector3 endPos,float t)
        {
            Vector3 pos0 = Vector3.Lerp(startPos,middlePos1,t);
            Vector3 pos1 = Vector3.Lerp(middlePos1,middlePos2,t);
            Vector3 pos2 = Vector3.Lerp(middlePos2,endPos,t);
            return GetPosition(pos0,pos1,pos2,t);
        }
        //五つポイントのCurve
        public static Vector3 GetPosition(Vector3 startPos, Vector3 middlePos1, Vector3 middlePos2, Vector3 middlePos3, Vector3 endPos, float t)
        {
            Vector3 pos0 = Vector3.Lerp(startPos, middlePos1, t);
            Vector3 pos1 = Vector3.Lerp(middlePos1, middlePos2, t);
            Vector3 pos2 = Vector3.Lerp(middlePos2, middlePos3, t);
            Vector3 pos3 = Vector3.Lerp(middlePos3, endPos, t);
            return GetPosition(pos0, pos1, pos2, pos3,t);
        }
    }
}