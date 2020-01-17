///
/// @file   MinBinaryHeap.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{
    //Min Heap For PathAgent.
    //パスファインダは最小値が必要ので、一番目最速の方法はMinBinaryHeap
    public class MinBinaryHeap 
    {
        //ノードを保存されたこのリスト
        public Node[]  nodes;
        //ヒープの最大限,このサイズは速度を影響している。
        int Capacity = 1000;
        //Current number of elements in the heap.
        int _Count;

        public int Count
        {
            get
            {
                return _Count;
            }
        }
        public MinBinaryHeap(int capacity)
        {
            Capacity = capacity;
            nodes = new Node[Capacity]; 
        }

        int Parent(int i)
        {
            return (i - 1) / 2;
        }
        //To get the index of left child of node at index i.
        int Left(int i)
        {
            return (2 * i + 1);
        }
        //To get the index of right child of node at index i.
        int Right(int i)
        {
            return (2 * i + 2);
        }
        //To extract the root element which is the minimum .
        //Method to remove minimum element (or root) from min heap.
        public Node Extract()
        {
            if (_Count <= 0)
            {
                return null;
            }
            if (_Count == 1)
            {
                _Count--;
                return nodes[0];
            }
            //Store the minimum value and remove it from heap.
            Node root = nodes[0];
            nodes[0] = nodes[_Count - 1];
            _Count--;
            MinHeapify(0);
            return root;
        }
        // A recursive method to heapify a subtree with the root at given index
        // This method assumes that the subtrees are already heapufied
        //再帰からやり直すバージョン
        void MinHeapify(int i)
        {
            
            int leftIndex;
            int rightIndex;
            int smallestIndex = i;
            int preIndex = smallestIndex;
            while (true)
            {
                preIndex = smallestIndex;

                leftIndex = Left(smallestIndex);

                rightIndex = Right(smallestIndex);

                if (leftIndex < _Count && nodes[leftIndex].F < nodes[smallestIndex].F)
                {
                    smallestIndex = leftIndex;
                }

                if (rightIndex < _Count && nodes[rightIndex].F < nodes[smallestIndex].F)
                {
                    smallestIndex = rightIndex;
                }
                if (preIndex != smallestIndex)
                {
                    Swap(preIndex, smallestIndex);
                }
                else
                {
                    break;
                }
            }
        }
        // A recursive method to heapify a subtree with the root at given index
        // This method assumes that the subtrees are already heapufied
        /*
         * 前のバージョン、一旦残る。
        void MinHeapifyRecursion(int i)
        {
            int l = Left(i);
            int r = Right(i);
            int smallest = i;
            if (l < _Count && nodes[l].F < nodes[i].F)
            {
                smallest = l;
            }

            if (r < _Count && nodes[r].F < nodes[smallest].F)
            {
                smallest = r;
            }

            if (smallest != i)
            {
                Swap(i,smallest);
                MinHeapify(smallest);
            }
        }*/

        //Descrease key value of key(key at root) from min heap
        //Decreases value of key at index 'i' to Val. It is assumed that Val the smaller than nodes[i]
        public void DescreaseKey(int i,int Val)
        {
            nodes[i].F = Val;
            while (i != 0 && nodes[Parent(i)].F > nodes[i].F)
            {
                Swap(i,Parent(i));
                i = Parent(i);
            }
        }

        public void DescreaseKey(Node node ,int Val)
        {
            DescreaseKey(node.IndexInBinaryHeap,Val);
        }

        public Node GetMin()
        {
            return nodes[0];
        }

        //Delete a key stored at index i.
        public void DeleteKey(int i)
        {
            DescreaseKey(i, int.MinValue);
            Extract();
        }

        public void DeleteKey(Node node)
        {
            DeleteKey(node.IndexInBinaryHeap);
        }

        //Prototype of a utility function to swap two integers.
        public void Swap(int x,int y)
        {
            Node temp = nodes[x];
            nodes[x] = nodes[y];
            nodes[y] = temp;
            nodes[x].IndexInBinaryHeap = x;
            nodes[y].IndexInBinaryHeap = y;
        }

        //Insert a new key k.
       public void InsertKey(Node node) {
            if (_Count == Capacity)
            {
                Debug.LogError(string.Format("Capacity is exceeding the current capcity {0} size , could not insert", Capacity));
            }
            //First insert the new key at the end .
            _Count++;
            int i = _Count - 1;
            nodes[i] = node;

            //Fix the min heap property if it is vviolated
            while (i != 0 && nodes[Parent(i)].F > nodes[i].F)
            {
                Swap(i,Parent(i));
                i = Parent(i);
            }
        }

        public void Refresh()
        {
            int i = _Count - 1;
            while (i != 0 && nodes[Parent(i)].F > nodes[i].F)
            {
                Swap(i,Parent(i));
            }
        }

        public void Clear()
        {
            for (int i=0;i<_Count;i++)
            {
                nodes[i].H = 0;
                nodes[i].G = 0;
                nodes[i].F = 0;
                nodes[i].Previous = null;
            }
            nodes = new Node[Capacity];
            _Count = 0;
        }

        public static void Test()
        {
            List<Node> nodes = new List<Node>();
            //int min = int.MaxValue;
            MinBinaryHeap minBinaryHeap = new MinBinaryHeap(10);
            /*
            for (int i=0;i<10;i++)
            {
                int v = Random.RandomRange(0,10000);
                if (min > v)
                {
                    min = v;
                }
                Node node = new Node(v);
               // nodes.Add(new Node(v));
                minBinaryHeap.InsertKey(node);
            }
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(min);
            */

            minBinaryHeap.InsertKey(new Node(3));
            minBinaryHeap.InsertKey(new Node(2));
            minBinaryHeap.InsertKey(new Node(-11));
            minBinaryHeap.InsertKey(new Node(15));
            minBinaryHeap.InsertKey(new Node(-5));
            minBinaryHeap.InsertKey(new Node(4));
            minBinaryHeap.InsertKey(new Node(-19));
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.GetMin().F);
            minBinaryHeap.DescreaseKey(2,0);
            Debug.Log(minBinaryHeap.GetMin().F);

    
        }
    }

}

