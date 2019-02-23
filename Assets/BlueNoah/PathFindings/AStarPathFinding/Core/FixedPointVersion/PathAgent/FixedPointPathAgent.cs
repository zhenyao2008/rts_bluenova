using UnityEngine;
using System.Collections.Generic;
using BlueNoah.Math.FixedPoint;
using System.Collections;
using UnityEngine.Events;

namespace BlueNoah.PathFinding.FixedPoint
{
    public class FixedPointPathAgent
    {
        static long agentIndex = 0;
        FixedPointGrid mGrid;
        FixedPointNode mTargetNode;
        public bool isSmooth = true;
        //public PathFinding.PathModifier pathModifier;
        List<FixedPointNode> mOpenList = new List<FixedPointNode>(1000);
#if UNITY_EDITOR
        List<FixedPointNode> mCloseList = new List<FixedPointNode>(1000);
#endif
        //これで計算する、リストのRemoveの方法を使わない、早く、CGがない。
        int mCurrentIndex = 0;

        public FixedPointPathAgent(FixedPointGrid grid)
        {
            this.mGrid = grid;
            agentIndex = long.MinValue;
        }

        void AddToOpenList(FixedPointNode node)
        {
            node.isOpen = agentIndex;
            AddOpenList(node);
        }

        FixedPointNode RemoveFirstFromOpenList()
        {
            FixedPointNode node = mOpenList[mCurrentIndex];
            mCurrentIndex++;
            return node;
        }

        void AddToCloseList(FixedPointNode node)
        {
#if UNITY_EDITOR
            mCloseList.Add(node);
#endif
            node.isClose = agentIndex;
        }

        public List<FixedPointNode> StartFind(FixedPointVector3 startPos, FixedPointVector3 targetPos)
        {

            FixedPointNode startNode = mGrid.GetNode(startPos);

            FixedPointNode targetNode = mGrid.GetNode(targetPos);
#if UNITY_EDITOR
            if (startNode == null)
            {
                Debug.Log(startPos);
            }
            if (targetNode == null)
            {
                Debug.Log(targetPos);
            }
#endif
            return StartFind(startNode, targetNode);
        }

        public static int nodeCountSearched;
        //同期
        public List<FixedPointNode> StartFind(FixedPointNode currentNode, FixedPointNode targetNode)
        {
            if (currentNode == null || targetNode == null)
            {
#if UNITY_EDITOR
                Debug.LogError("currentNode:" + currentNode + ";targetNode:" + targetNode);
#endif
                return new List<FixedPointNode>();
            }

            //if (currentNode.IsBlock || !currentNode.Enable)
            //{
            //    currentNode = GetAvailableNode(currentNode);
            //}

            //if (targetNode.IsBlock || !targetNode.Enable)
            //{
            //    targetNode = GetAvailableNode(targetNode);
            //}

            agentIndex++;
            mCurrentIndex = 0;
            mTargetNode = targetNode;
            bool searched = false;
            mOpenList.Clear();
#if UNITY_EDITOR
            mCloseList.Clear();
#endif
            //Clear the G value for first node.
            currentNode.G = 0;

            currentNode.H = 0;

            currentNode.F = 0;

            mOpenList.Add(currentNode);
            while ((mOpenList.Count > mCurrentIndex && !searched))
            {
                //リスト中にF値一番小さいのノード
                FixedPointNode node = RemoveFirstFromOpenList();
                if (node != mTargetNode)
                {
                    AddToCloseList(node);
                    float t = Time.realtimeSinceStartup;
                    for (int i = 0; i < node.neighbors.Count; i++)
                    {
                        if (node.neighbors[i].isClose != agentIndex && !node.neighbors[i].IsBlock && node.Enable)
                        {

                            FixedPoint64 G = node.G + node.consumes[i] + node.neighbors[i].consumeRoadSizePlus;

                            FixedPoint64 H = Mathf.Abs(mTargetNode.x - node.neighbors[i].x) * mGrid.NodeSize + Mathf.Abs(mTargetNode.z - node.neighbors[i].z) * mGrid.NodeSize;

                            FixedPoint64 F = G + H;

                            if (node.neighbors[i].isOpen == agentIndex)
                            {
                                if (node.neighbors[i].F > F)
                                {
                                    node.neighbors[i].G = G;

                                    node.neighbors[i].H = H;

                                    node.neighbors[i].F = F;

                                    node.neighbors[i].previous = node;

                                    //mOpenList.Remove(node.neighbors[i]);

                                    //AddToOpenList(node.neighbors[i]);
                                }
                            }
                            else
                            {
                                node.neighbors[i].G = G;

                                node.neighbors[i].H = H;

                                node.neighbors[i].F = F;

                                node.neighbors[i].previous = node;

                                AddToOpenList(node.neighbors[i]);

                            }

                            ////Calculate G
                            //node.neighbors[i].G = node.G + node.consumes[i] + node.neighbors[i].consumeRoadSizePlus; ;
                            ////Calculate H
                            //node.neighbors[i].H = Mathf.Abs(mTargetNode.x - node.neighbors[i].x) + Mathf.Abs(mTargetNode.z - node.neighbors[i].z);
                            ////Calculate F
                            //node.neighbors[i].F = node.neighbors[i].G + node.neighbors[i].H;
                            ////insert openlist order by F;
                            //AddToOpenList(node.neighbors[i]);

                            //node.neighbors[i].previous = node;

                            nodeCountSearched++;
                        }
                    }
                }
                else
                {
                    searched = true;
                }
            }
            List<FixedPointNode> resultPath = new List<FixedPointNode>();
            if (searched)
            {
                resultPath = GetMovePath(mTargetNode);
            }
            else
            {
                Debug.Log("target can't be reached!");
            }
            List<FixedPointNode> realPath = new List<FixedPointNode>(resultPath);
            if (agentIndex == long.MaxValue)
            {
                ResetAgentIndex();
            }
            foreach (FixedPointNode node in mOpenList)
            {
                node.G = 0;

                node.H = 0;

                node.F = 0;
            }
            return realPath;
        }


        public List<FixedPointNode> StartFindFast(FixedPointNode currentNode, FixedPointNode targetNode)
        {
            if (currentNode == null || targetNode == null)
            {
#if UNITY_EDITOR
                Debug.LogError("currentNode:" + currentNode + ";targetNode:" + targetNode);
#endif
                return new List<FixedPointNode>();
            }

            //if (currentNode.IsBlock || !currentNode.Enable)
            //{
            //    currentNode = GetAvailableNode(currentNode);
            //}

            //if (targetNode.IsBlock || !targetNode.Enable)
            //{
            //    targetNode = GetAvailableNode(targetNode);
            //}

            agentIndex++;
            mCurrentIndex = 0;
            mTargetNode = targetNode;
            bool searched = false;
            mOpenList.Clear();
#if UNITY_EDITOR
            mCloseList.Clear();
#endif
            //Clear the G value for first node.
            currentNode.G = 0;

            currentNode.H = 0;

            currentNode.F = 0;

            mOpenList.Add(currentNode);
            while ((mOpenList.Count > mCurrentIndex && !searched))
            {
                //リスト中にF値一番小さいのノード
                FixedPointNode node = RemoveFirstFromOpenList();
                if (node != mTargetNode)
                {
                    AddToCloseList(node);
                    float t = Time.realtimeSinceStartup;
                    for (int i = 0; i < node.neighbors.Count; i++)
                    {
                        //Open List not search , Fast.
                        if (node.neighbors[i].isOpen != agentIndex && node.neighbors[i].isClose != agentIndex && !node.neighbors[i].IsBlock && node.Enable)
                        {

                            //Calculate G
                            node.neighbors[i].G = node.G + node.consumes[i] + node.neighbors[i].consumeRoadSizePlus; ;
                            //Calculate H
                            node.neighbors[i].H = Mathf.Abs(mTargetNode.x - node.neighbors[i].x) + Mathf.Abs(mTargetNode.z - node.neighbors[i].z);
                            //Calculate F
                            node.neighbors[i].F = node.neighbors[i].G + node.neighbors[i].H;
                            //insert openlist order by F;
                            AddToOpenList(node.neighbors[i]);

                            node.neighbors[i].previous = node;

                            AddToOpenList(node.neighbors[i]);

                            nodeCountSearched++;
                        }
                    }
                }
                else
                {
                    searched = true;
                }
            }
            List<FixedPointNode> resultPath = new List<FixedPointNode>();
            if (searched)
            {
                resultPath = GetMovePath(mTargetNode);
            }
            else
            {
                Debug.Log("target can't be reached!");
            }
            List<FixedPointNode> realPath = new List<FixedPointNode>(resultPath);
            if (agentIndex == long.MaxValue)
            {
                ResetAgentIndex();
            }
            return realPath;
        }


        void ResetAgentIndex()
        {
            agentIndex = long.MinValue;
            for (int i = 0; i < mGrid.NodeList.Count; i++)
            {
                mGrid.NodeList[i].isClose = agentIndex;
                mGrid.NodeList[i].isOpen = agentIndex;
            }
        }

        FixedPointNode mNextNode;
        public bool next;
        //Test only,single running only.
        public IEnumerator _StartFind(FixedPointVector3 startPos, FixedPointVector3 targetPos, UnityAction<List<FixedPointNode>> onComplete)
        {
            Debug.Log("_StartFind");

            FixedPointNode currentNode = mGrid.GetNode(startPos);

            FixedPointNode targetNode = mGrid.GetNode(targetPos);

            //if (currentNode.IsBlock || !currentNode.Enable)
            //{
            //    currentNode = GetAvailableNode(currentNode);
            //}

            //if (targetNode.IsBlock || !targetNode.Enable)
            //{
            //    targetNode = GetAvailableNode(targetNode);
            //}

            agentIndex++;
            mCurrentIndex = 0;
            mTargetNode = targetNode;
            bool searched = false;
            mOpenList.Clear();
#if UNITY_EDITOR
            mCloseList.Clear();
#endif
            //grid.Reset();
            //mOpenList.Add(currentNode);
            AddToOpenList(currentNode);
            while ((mOpenList.Count > mCurrentIndex && !searched))
            {
                next = true;
                if (!next)
                {
                    //next = false;
                }
                else
                {
                    next = false;
                    //リスト中にF値一番小さいのノード
                    FixedPointNode node = RemoveFirstFromOpenList();
                    mNextNode = node;
                    if (node != mTargetNode)
                    {
                        AddToCloseList(node);
                        float t = Time.realtimeSinceStartup;
                        for (int i = 0; i < node.neighbors.Count; i++)
                        {
                            if (node.neighbors[i].isClose != agentIndex && !node.neighbors[i].IsBlock && node.Enable)
                            {

                                FixedPoint64 G = node.G + node.consumes[i] + node.neighbors[i].consumeRoadSizePlus;

                                FixedPoint64 H = Mathf.Abs(mTargetNode.x - node.neighbors[i].x) * mGrid.NodeSize + Mathf.Abs(mTargetNode.z - node.neighbors[i].z) * mGrid.NodeSize;

                                FixedPoint64 F = G + H;

                                if (node.neighbors[i].isOpen == agentIndex)
                                {
                                    if (node.neighbors[i].F > F)
                                    {
                                        node.neighbors[i].G = G;

                                        node.neighbors[i].H = H;

                                        node.neighbors[i].F = F;

                                        node.neighbors[i].previous = node;
                                    }
                                }
                                else
                                {
                                    node.neighbors[i].G = G;

                                    node.neighbors[i].H = H;

                                    node.neighbors[i].F = F;

                                    node.neighbors[i].previous = node;

                                    AddToOpenList(node.neighbors[i]);

                                }

                                ////Calculate G
                                //node.neighbors[i].G = node.G + node.consumes[i] + node.neighbors[i].consumeRoadSizePlus;
                                ////Calculate H
                                //node.neighbors[i].H = Mathf.Abs(mTargetNode.x - node.neighbors[i].x) * mGrid.NodeSize + Mathf.Abs(mTargetNode.z - node.neighbors[i].z) * mGrid.NodeSize;
                                ////Calculate F
                                //node.neighbors[i].F = node.neighbors[i].G + node.neighbors[i].H;
                                ////Debug.Log(node.neighbors[i].G + "||" + node.neighbors[i].H.AsFloat() + "||" +  node.neighbors[i].F.AsFloat());
                                ////insert openlist order by F;

                                //node.neighbors[i].previous = node;

                                nodeCountSearched++;

                                if (node == mTargetNode)
                                {
                                    searched = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        searched = true;
                    }
                }
                yield return null;
            }
            List<FixedPointNode> resultPath = new List<FixedPointNode>();
            if (searched)
            {
                resultPath = GetMovePath(mTargetNode);
            }
            else
            {
                Debug.Log("target can't be reached!");
            }
            List<FixedPointNode> realPath = new List<FixedPointNode>(resultPath);
            if (onComplete != null)
                onComplete(realPath);
            if (agentIndex == long.MaxValue)
            {
                ResetAgentIndex();
            }
            //return realPath;
        }

        //ノードをF（H）で順番で openlist へ置いて
        //付きキュー
        void AddOpenList(FixedPointNode node)
        {
            mOpenList.Add(node);
            return;

            bool added = false;
            for (int i = mCurrentIndex; i < mOpenList.Count; i++)
            {
                if (mOpenList[i].F > node.F)
                {
                    mOpenList.Insert(i, node);
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                mOpenList.Insert(mOpenList.Count, node);
            }
        }

        bool IsPathBlock()
        {
            foreach (FixedPointNode node in resultPath)
            {
                if (node.IsBlock)
                {
                    return true;
                }
            }
            return false;
        }

        List<FixedPointNode> resultPath = new List<FixedPointNode>(1000);
        //経路を探索する。
        public List<FixedPointNode> GetMovePath(FixedPointNode node)
        {
            resultPath.Clear();
            FixedPointNode currentNode = node;
            resultPath.Insert(0, currentNode);
            //Debug.Log(currentNode);
            while (currentNode.previous != null && currentNode.isOpen == agentIndex)
            {
                resultPath.Insert(0, currentNode.previous);
                currentNode = currentNode.previous;
            }
            return resultPath;
            //TODO 
            //pathModifier;
            //if (isSmooth)
            //    return pathModifier.SmoothPath(resultPath);
            //else
            //return resultPath;
        }

        public FixedPointNode GetAvailableNode(FixedPointNode currentNode)
        {
            mOpenList.Clear();
            mOpenList.Add(currentNode);
            mCurrentIndex = 0;
            bool searched = false;
            FixedPointNode resultNode = null;
            while (!searched && mOpenList.Count > 0)
            {
                FixedPointNode node = RemoveFirstFromOpenList();
                if (!node.IsBlock && node.Enable)
                {
                    resultNode = node;
                    searched = true;
                }
                else
                {
                    for (int i = 0; i < node.neighbors.Count; i++)
                    {
                        if (node.neighbors[i].isOpen != agentIndex)
                        {
                            if (!node.neighbors[i].IsBlock && node.Enable)
                            {
                                resultNode = node;
                                searched = true;
                            }
                            else
                            {
                                node.neighbors[i].F = node.F + node.consumes[i];
                                AddToOpenList(node.neighbors[i]);
                            }
                        }
                    }
                }
            }
            return resultNode;
        }

        float mColorSpeed = 10f;
        float mColorPlus = -0.005f;
        public bool showGizmos = true;
        public bool showHandle = false;
        float mDebugSize = 0.5f;
        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            //return;
            Gizmos.color = Color.yellow;
            for (int i = 0; i < mCloseList.Count; i++)
            {
                if (mCloseList[i].F > 0 && showHandle)
                {
                    UnityEditor.Handles.Label(mCloseList[i].pos.ToVector3(), mCloseList[i].F.ToString());
                    if (mCloseList[i].previous != null)
                    {
                        Quaternion rotation = Quaternion.LookRotation((mCloseList[i].previous.pos - mCloseList[i].pos).ToVector3().normalized, Vector3.up);
                        UnityEditor.Handles.ArrowHandleCap(0, mCloseList[i].pos.ToVector3(), rotation, 0.5f, EventType.Repaint);
                    }
                }
                Gizmos.DrawCube(mCloseList[i].pos.ToVector3(), Vector3.one * mGrid.NodeSize.AsFloat() * mDebugSize);
            }
            Gizmos.color = Color.red;
            for (int i = mCurrentIndex; i < mOpenList.Count; i++)
            {
                if (mOpenList[i].F > 0 && showHandle)
                {
                    UnityEditor.Handles.Label(mOpenList[i].pos.ToVector3(), mOpenList[i].F.ToString());

                    if (mOpenList[i].previous != null)
                    {
                        Quaternion rotation = Quaternion.LookRotation((mOpenList[i].previous.pos - mOpenList[i].pos).ToVector3().normalized, Vector3.up);
                        UnityEditor.Handles.ArrowHandleCap(0, mOpenList[i].pos.ToVector3(), rotation, 0.5f, EventType.Repaint);
                    }
                }
                Gizmos.DrawCube(mOpenList[i].pos.ToVector3(), Vector3.one * mGrid.NodeSize.AsFloat() * mDebugSize);
            }
            for (int i = 0; i < resultPath.Count; i++)
            {
                float sin = Mathf.Sin(Time.time * mColorSpeed + i * mColorPlus);
                float cos = Mathf.Cos(Time.time * mColorSpeed + i * mColorPlus);
                Gizmos.color = new Color(cos, sin, cos, 1);
                if (resultPath[i].F > 0 && showHandle)
                    UnityEditor.Handles.Label(resultPath[i].pos.ToVector3(), resultPath[i].F.ToString());
                Gizmos.DrawCube(resultPath[i].pos.ToVector3(), Vector3.one * mGrid.NodeSize.AsFloat() * mDebugSize);
            }
            if (mNextNode != null)
            {
                Gizmos.DrawCube(mNextNode.pos.ToVector3(), Vector3.one * mGrid.NodeSize.AsFloat() * 1f);
            }
#endif
        }
    }
}
