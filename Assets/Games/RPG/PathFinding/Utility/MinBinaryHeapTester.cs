using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{

    public class MinBinaryHeapTester
    {

        MinBinaryHeap minBinaryHeap;

        public   void TestInsertKeyAndExtract()
        {
            List<Node> nodes = new List<Node>();
            MinBinaryHeap minBinaryHeap = new MinBinaryHeap(1000);

            nodes.Add(new Node(1));
            nodes.Add(new Node(2));
            nodes.Add(new Node(3));
            nodes.Add(new Node(4));
            nodes.Add(new Node(5));
            nodes.Add(new Node(6));
            nodes.Add(new Node(-1));
            nodes.Add(new Node(-2));
            nodes.Add(new Node(-3));

            for (int i = 0; i < nodes.Count; i++)
            {
                minBinaryHeap.InsertKey(nodes[i]);
            }

            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
        }

        public void TestDeleteKey()
        {
            List<Node> nodes = new List<Node>();
            MinBinaryHeap minBinaryHeap = new MinBinaryHeap(1000);
            nodes.Add(new Node(1));
            nodes.Add(new Node(2));
            nodes.Add(new Node(3));
            nodes.Add(new Node(-1));
            nodes.Add(new Node(-2));
            nodes.Add(new Node(-3));

            for (int i=0;i<nodes.Count;i++) {
                minBinaryHeap.InsertKey(nodes[i]);
            }

            minBinaryHeap.DeleteKey(nodes[5]);
            Debug.Log(minBinaryHeap.GetMin().F);
            minBinaryHeap.DeleteKey(nodes[4]);
            Debug.Log(minBinaryHeap.GetMin().F);
            minBinaryHeap.DeleteKey(nodes[3]);
            Debug.Log(minBinaryHeap.GetMin().F);
            minBinaryHeap.DeleteKey(nodes[2]);
            Debug.Log(minBinaryHeap.GetMin().F);
            minBinaryHeap.DeleteKey(nodes[1]);
            Debug.Log(minBinaryHeap.GetMin().F);
        }

        public void TestDecreaseKey()
        {
            List<Node> nodes = new List<Node>();
            MinBinaryHeap minBinaryHeap = new MinBinaryHeap(1000);
            nodes.Add(new Node(1));
            nodes.Add(new Node(2));
            nodes.Add(new Node(3));
            nodes.Add(new Node(-3));
            nodes.Add(new Node(-2));
            nodes.Add(new Node(-1));
            for (int i= 0;i<nodes.Count;i++)
            {
                minBinaryHeap.InsertKey(nodes[i]);
            }

            minBinaryHeap.DescreaseKey(nodes[2],-4);
            Debug.Log(minBinaryHeap.GetMin().F);
            minBinaryHeap.DescreaseKey(nodes[4],-9);
            Debug.Log(minBinaryHeap.GetMin().F);

        }

        public void Test()
        {
            /*
            MinBinaryHeap1 minBinaryHeap1 = new MinBinaryHeap1(1000);
            minBinaryHeap1.Insert(new Node(2));
            Debug.Log(minBinaryHeap1.GetMin().F);
            minBinaryHeap1.Insert(new Node(3));
            Debug.Log(minBinaryHeap1.GetMin().F);
            minBinaryHeap1.Insert(new Node(11));
            Debug.Log(minBinaryHeap1.GetMin().F);

            minBinaryHeap1.Insert(new Node(1));
            Debug.Log(minBinaryHeap1.GetMin().F);
            minBinaryHeap1.Insert(new Node(-2));
            Debug.Log(minBinaryHeap1.GetMin().F);
            minBinaryHeap1.Insert(new Node(-5));
            Debug.Log(minBinaryHeap1.GetMin().F);
            return;
            */

            /*
            MinHeap<int> heap = new MinHeap<int>();

            heap.insert(3);
            heap.insert(1);
            heap.insert(5);
            heap.insert(3);
            heap.insert(7);
            heap.insert(-1);
            heap.insert(-10);

            Debug.Log(heap.extractMin());
            Debug.Log(heap.extractMin());
            Debug.Log(heap.extractMin());
            Debug.Log(heap.extractMin());

            return;
            */
            /*
           #region Test InsertKey
           List<Node> nodes = new List<Node>();
           MinBinaryHeap minBinaryHeap = new MinBinaryHeap(1000);

           nodes.Add(new Node(1));
           nodes.Add(new Node(2));
           nodes.Add(new Node(3));
           nodes.Add(new Node(4));
           nodes.Add(new Node(5));
           nodes.Add(new Node(6));
           nodes.Add(new Node(-1));
           nodes.Add(new Node(-2));
           nodes.Add(new Node(-3));

           for (int i=0;i<nodes.Count;i++)
           {
               minBinaryHeap.InsertKey(nodes[i]);
           }

           Debug.Log(minBinaryHeap.Extract().F);
           Debug.Log(minBinaryHeap.Extract().F);
           Debug.Log(minBinaryHeap.Extract().F);
           Debug.Log(minBinaryHeap.Extract().F);
           Debug.Log(minBinaryHeap.Extract().F);
           Debug.Log(minBinaryHeap.Extract().F);
           Debug.Log(minBinaryHeap.Extract().F);
           Debug.Log(minBinaryHeap.Extract().F);

           //minBinaryHeap.InsertKey(new Node(1));
           //minBinaryHeap.InsertKey(new Node(2));
           //minBinaryHeap.InsertKey(new Node(3));
           //minBinaryHeap.InsertKey(new Node(4));
           //minBinaryHeap.InsertKey(new Node(5));
           //minBinaryHeap.InsertKey(new Node(6));
           //minBinaryHeap.InsertKey(new Node(-1));
           //minBinaryHeap.InsertKey(new Node(-2));
           #endregion
            */

            #region Test DeleteKey
            minBinaryHeap.DeleteKey(1);
           #endregion


           #region Test 

           #endregion


           #region Test 3

           #endregion

           //List<Node> nodes = new List<Node>();
           //int min = int.MaxValue;
           //MinBinaryHeap minBinaryHeap = new MinBinaryHeap(10);
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

            minBinaryHeap.DescreaseKey(5,-20);


            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);
            Debug.Log(minBinaryHeap.Extract().F);


            //Debug.Log(minBinaryHeap.GetMin().F);
            minBinaryHeap.DescreaseKey(2, 0);
            //Debug.Log(minBinaryHeap.GetMin().F);
        }
    }
}
