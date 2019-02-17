//using UnityEngine;
//using System.Collections.Generic;
//using BlueNoah.Math.FixedPoint;
//using System.Collections;
//using UnityEngine.Events;

//namespace BlueNoah.PathFinding.FixedPoint
//{
//    public class DoubleSidePathAgent
//    {

//        static long agentIndex = 0;

//        Grid mGrid;

//        Node mTargetNode;

//        public bool isSmooth = true;

//        //public PathFinding.PathModifier pathModifier;

//        //これで計算する、リストのRemoveの方法を使わない、早く、CGがない。
//        int mCurrentFrontOpenIndex = 0;

//        int mCurrentBackOpenIndex = 0;

//        //From StartNode to EndNode.
//        List<Node> mOpenListFront = new List<Node>(1000);
//#if UNITY_EDITOR
//        List<Node> mCloseListFront = new List<Node>(1000);
//#endif
//        //From EndNode to StartNode.
//        List<Node> mOpenListBack = new List<Node>(1000);
//#if UNITY_EDITOR
//        List<Node> mCloseListBack = new List<Node>(1000);
//#endif

//        public DoubleSidePathAgent(Grid grid)
//        {
//            this.mGrid = grid;
//            agentIndex = long.MinValue;
//        }

//        void AddToFrontOpenList(Node node)
//        {
//            node.isOpenFront = agentIndex;
//            AddOpenList(mCurrentFrontOpenIndex, node, mOpenListFront);
//        }

//        void AddToBackOpenList(Node node)
//        {
//            node.isOpenBack = agentIndex;
//            AddOpenList(mCurrentBackOpenIndex, node, mOpenListBack);
//        }

//        //ノードをF（H）で順番で openlist へ置いて
//        //付きキュー
//        void AddOpenList(int currentIndex, Node node, List<Node> targetList)
//        {
//            bool added = false;
//            for (int i = currentIndex; i < targetList.Count; i++)
//            {
//                if (targetList[i].F >= node.F)
//                {
//                    targetList.Insert(i, node);
//                    added = true;
//                    break;
//                }
//            }
//            if (!added)
//            {
//                targetList.Insert(targetList.Count, node);
//            }
//        }

//        Node RemoveFirstFromFrontOpenList()
//        {
//            return RemoveFirstFromOpenList(mOpenListFront, ref mCurrentFrontOpenIndex);
//        }

//        Node RemoveFirstFromBackOpenList()
//        {
//            return RemoveFirstFromOpenList(mOpenListBack, ref mCurrentBackOpenIndex);
//        }

//        Node RemoveFirstFromOpenList(List<Node> openList, ref int index)
//        {
//            Node node = openList[index];
//            index++;
//            return node;
//        }

//        void AddToFrontCloseList(Node node)
//        {
//            node.isCloseFront = agentIndex;
//#if UNITY_EDITOR
//            AddToCloseList(mCloseListFront,node);
//#endif
//        }

//        void AddToBackCloseList(Node node)
//        {
//            node.isCloseBack = agentIndex;
//#if UNITY_EDITOR
//            AddToCloseList(mCloseListBack,node);
//#endif
//        }

//        void AddToCloseList(List<Node> closeList,Node node)
//        {
//            closeList.Add(node);
//        }


//        Node RemoveFirstFromOpenList()
//        {
//            Node node = mOpenListFront[mCurrentFrontOpenIndex];
//            mCurrentFrontOpenIndex++;
//            return node;
//        }

//        void AddToCloseList(Node node)
//        {
//#if UNITY_EDITOR
//            mCloseListFront.Add(node);
//#endif
//            node.isClose = agentIndex;
//        }

//        public List<Node> StartFind(FixedPointVector3 startPos, FixedPointVector3 targetPos)
//        {

//            Node startNode = mGrid.GetNode(startPos);

//            Node targetNode = mGrid.GetNode(targetPos);

//            return StartFind(startNode, targetNode);
//        }

//        public static int nodeCountSearched;
//        //同期
//        public List<Node> StartFind(Node currentNode, Node target)
//        {
//            if (currentNode == null || target == null)
//            {
//#if UNITY_EDITOR
//                Debug.LogError("currentNode:" + currentNode + ";targetNode:" + target);
//#endif
//                return new List<Node>();
//            }
//            agentIndex++;
//            mCurrentFrontOpenIndex = 0;
//            mTargetNode = target;
//            bool searched = false;
//            mOpenListFront.Clear();
//#if UNITY_EDITOR
//            mCloseListFront.Clear();
//#endif
//            //grid.Reset();
//            mOpenListFront.Add(currentNode);
//            while ((mOpenListFront.Count > mCurrentFrontOpenIndex && !searched))
//            {
//                //リスト中にF値一番小さいのノード
//                Node node = RemoveFirstFromOpenList();// openList [0];
//                if (node != mTargetNode)
//                {
//                    AddToCloseList(node);
//                    float t = Time.realtimeSinceStartup;
//                    for (int i = 0; i < node.neighbors.Count; i++)
//                    {
//                        if (node.neighbors[i].isOpen != agentIndex && node.neighbors[i].isClose != agentIndex && !node.neighbors[i].IsBlock && node.Enable)
//                        {
//                            //Calculate G
//                            node.neighbors[i].G = node.G + node.consumes[i];
//                            //Calculate H
//                            node.neighbors[i].H = Mathf.Abs(mTargetNode.x - node.neighbors[i].x) + Mathf.Abs(mTargetNode.z - node.neighbors[i].z);
//                            //Calculate F
//                            node.neighbors[i].F = node.neighbors[i].G + node.neighbors[i].H;
//                            //insert openlist order by F;
//                            AddToFrontOpenList(node.neighbors[i]);
//                            node.neighbors[i].previous = node;
//                            nodeCountSearched++;
//                        }
//                    }
//                }
//                else
//                {
//                    searched = true;
//                }
//            }
//            List<Node> resultPath = new List<Node>();
//            if (searched)
//            {
//                resultPath = GetMovePath(mTargetNode);
//            }
//            else
//            {
//                Debug.Log("target can't be reached!");
//            }
//            List<Node> realPath = new List<Node>(resultPath);
//            if (agentIndex == long.MaxValue)
//            {
//                ResetAgentIndex();
//            }
//            return realPath;
//        }

//        void ResetAgentIndex()
//        {
//            agentIndex = long.MinValue;
//            for (int i = 0; i < mGrid.NodeList.Count; i++)
//            {
//                mGrid.NodeList[i].isClose = agentIndex;
//                mGrid.NodeList[i].isOpen = agentIndex;
//            }
//        }

//        //Test only,single running only.
//        public IEnumerator _StartFind(FixedPointVector3 startPos, FixedPointVector3 targetPos, UnityAction<List<Node>> onComplete)
//        {
//            Node currentNode = mGrid.GetNode(startPos);
//            Node targetNode = mGrid.GetNode(targetPos);
//            agentIndex++;
//            mCurrentFrontOpenIndex = 0;
//            mTargetNode = targetNode;
//            bool searched = false;
//            mOpenListFront.Clear();
//#if UNITY_EDITOR
//            mCloseListFront.Clear();
//#endif
//            //grid.Reset();
//            mOpenListFront.Add(currentNode);
//            while ((mOpenListFront.Count > mCurrentFrontOpenIndex && !searched))
//            {
//                //リスト中にF値一番小さいのノード
//                Node node = RemoveFirstFromOpenList();
//                if (node != mTargetNode)
//                {
//                    AddToCloseList(node);
//                    float t = Time.realtimeSinceStartup;
//                    for (int i = 0; i < node.neighbors.Count; i++)
//                    {
//                        if (node.neighbors[i].isOpen != agentIndex && node.neighbors[i].isClose != agentIndex && !node.neighbors[i].IsBlock && node.Enable)
//                        {
//                            //Calculate G
//                            node.neighbors[i].G = node.G + node.consumes[i];
//                            //Calculate H
//                            node.neighbors[i].H = Mathf.Abs(mTargetNode.x - node.neighbors[i].x) * mGrid.NodeSize + Mathf.Abs(mTargetNode.z - node.neighbors[i].z) * mGrid.NodeSize;
//                            //Calculate F
//                            node.neighbors[i].F = node.neighbors[i].G + node.neighbors[i].H;
//                            //Debug.Log(node.neighbors[i].G + "||" + node.neighbors[i].H.AsFloat() + "||" +  node.neighbors[i].F.AsFloat());
//                            //insert openlist order by F;
//                            AddToFrontOpenList(node.neighbors[i]);

//                            node.neighbors[i].previous = node;

//                            nodeCountSearched++;

//                            if (node == mTargetNode)
//                            {
//                                searched = true;
//                                break;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    searched = true;
//                }
//                yield return null;
//            }
//            List<Node> resultPath = new List<Node>();
//            if (searched)
//            {
//                resultPath = GetMovePath(mTargetNode);
//            }
//            else
//            {
//                Debug.Log("target can't be reached!");
//            }
//            List<Node> realPath = new List<Node>(resultPath);
//            onComplete(realPath);
//            if (agentIndex == long.MaxValue)
//            {
//                ResetAgentIndex();
//            }
//            //return realPath;
//        }



//        bool IsPathBlock()
//        {
//            foreach (Node node in resultPath)
//            {
//                if (node.IsBlock)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        List<Node> resultPath = new List<Node>(1000);
//        //経路を探索する。
//        public List<Node> GetMovePath(Node node)
//        {
//            resultPath.Clear();
//            Node currentNode = node;
//            resultPath.Insert(0, currentNode);
//            Debug.Log(currentNode);
//            while (currentNode.previous != null && currentNode.isOpen == agentIndex)
//            {
//                resultPath.Insert(0, currentNode.previous);
//                currentNode = currentNode.previous;
//            }
//            return resultPath;
//            //TODO 
//            //pathModifier;
//            //if (isSmooth)
//            //    return pathModifier.SmoothPath(resultPath);
//            //else
//            //return resultPath;
//        }

//        public Node GetAvailableNode(Node currentNode)
//        {
//            mOpenListFront.Clear();
//            mOpenListFront.Add(currentNode);
//            bool searched = false;
//            Node resultNode = null;
//            while (!searched && mOpenListFront.Count > 0)
//            {
//                Node node = RemoveFirstFromOpenList();// openList [0];
//                if (!node.IsBlock && node.Enable)
//                {
//                    resultNode = node;
//                    searched = true;
//                }
//                else
//                {
//                    for (int i = 0; i < node.neighbors.Count; i++)
//                    {
//                        if (node.neighbors[i].isOpen != agentIndex)
//                        {
//                            if (!node.neighbors[i].IsBlock && node.Enable)
//                            {
//                                resultNode = node;
//                                searched = true;
//                            }
//                            else
//                            {
//                                node.neighbors[i].F = node.F + node.consumes[i];
//                                AddToFrontOpenList(node.neighbors[i]);
//                            }
//                        }
//                    }
//                }
//            }
//            return resultNode;
//        }


//#if UNITY_EDITOR
//        float mColorSpeed = 10f;
//        float mColorPlus = -0.005f;
//        public bool showGizmos = true;
//        public void OnDrawGizmos()
//        {
//            //Debug.Log("OnDrawGizmos");
//            //if (!showGizmos)
//            //return;
//            Gizmos.color = Color.yellow;
//            for (int i = 0; i < mCloseListFront.Count; i++)
//            {
//                Gizmos.DrawCube(mCloseListFront[i].pos.ToVector3(), Vector3.one * mGrid.NodeSize.AsFloat() * 0.8f);
//            }
//            Gizmos.color = Color.red;
//            for (int i = mCurrentFrontOpenIndex; i < mOpenListFront.Count; i++)
//            {
//                Gizmos.DrawCube(mOpenListFront[i].pos.ToVector3(), Vector3.one * mGrid.NodeSize.AsFloat() * 0.8f);
//            }
//            for (int i = 0; i < resultPath.Count; i++)
//            {
//                float sin = Mathf.Sin(Time.time * mColorSpeed + i * mColorPlus);
//                float cos = Mathf.Cos(Time.time * mColorSpeed + i * mColorPlus);
//                Gizmos.color = new Color(cos, sin, cos, 1);
//                Gizmos.DrawCube(resultPath[i].pos.ToVector3(), Vector3.one * mGrid.NodeSize.AsFloat() * 0.8f);
//            }
//        }
//#endif
//    }
//}
