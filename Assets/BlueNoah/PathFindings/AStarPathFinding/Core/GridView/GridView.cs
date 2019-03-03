using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using BlueNoah.Utility;

namespace BlueNoah.PathFinding
{
    public class GridView : MonoBehaviour
    {

        Material mMaterial;

        int mXCount = 20;

        int mYCount = 20;

        float mNodeSize = 1f;

        float mPadding = 0.04f;

        Vector3 mPrePos;

        public GameObject gridGameObject;

        Color[] mColors;

        Mesh mMesh;

        Transform mTrans;

        int layer;

        public Color normalColor = new Color(0, 1, 0, 0.5f);

        public Color blockColor = new Color(1, 0, 0, 0.5f);

        public Color hoverColor = new Color(0, 0, 1, 0.5f);

        public Color disableColor = new Color(1, 1, 1, 0.5f);

        public BoxCollider boxCollider;

        //notice: padding is persentage.
        public void InitGridView(int xCount, int yCount, float gridSize, float padding, Material material, int layer)
        {
            if (gridGameObject != null)
            {
                Destroy(gridGameObject);
            }
            mMaterial = material;
            mXCount = xCount;
            mYCount = yCount;
            mNodeSize = gridSize;
            mPadding = padding;
            mTrans = transform;
            this.layer = layer;
            InitGridView();
        }

        void InitGridView()
        {
            GameObject go = MeshUtility.DrawGridGameObject(mMaterial, Color.white, mNodeSize, mXCount, mYCount, mPadding * mNodeSize);
            mMesh = go.GetComponent<MeshFilter>().mesh;
            mColors = mMesh.colors;
            go.transform.SetParent(mTrans);
            go.transform.localPosition = new Vector3(-mNodeSize * mXCount / 2f, 0, -mNodeSize * mYCount / 2f);
            //go.transform.localPosition = Vector3.zero;
            go.name = "Grid";
            gridGameObject = go;
            boxCollider = go.GetOrAddComponent<BoxCollider>();
            boxCollider.size = new Vector3(mXCount * mNodeSize * 1.2f, 0.05f, mYCount * mNodeSize * 2f);
            go.layer = layer;
            Debug.Log("mXCount:" + mXCount + ";mYCount:" + mYCount);
            HideGrid();
        }

        public void DOShowGrid()
        {
            gridGameObject.GetComponent<MeshRenderer>().material.DOFade(1, 0.5f);
        }

        public void ShowGrid()
        {
            Color color = gridGameObject.GetComponent<MeshRenderer>().material.color;
            gridGameObject.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 1);
        }

        public void HideGrid()
        {
            Color color = gridGameObject.GetComponent<MeshRenderer>().material.color;
            gridGameObject.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0);
        }

        public void DOHideGrid()
        {
            gridGameObject.GetComponent<MeshRenderer>().material.DOFade(0, 0.5f);
        }

        public void SetNodeByWorldPosition(ref Color[] colors, Vector3 pos, Color color)
        {
            pos = mTrans.InverseTransformPoint(pos);
            SetNodeByLocalPosition(ref colors, pos, color);
        }

        public void SetNodeByLocalPosition(ref Color[] colors, Vector3 pos, Color color)
        {
            int x = Mathf.FloorToInt((pos.x - gridGameObject.transform.localPosition.x ) / mNodeSize);
            int z = Mathf.FloorToInt((pos.z - gridGameObject.transform.localPosition.z ) / mNodeSize);
            int number = x * mYCount + z;
            if (number * 4 < 0 || number * 4 > colors.Length)
            {
                return;
            }
            colors[number * 4] = color;
            colors[number * 4 + 1] = color;
            colors[number * 4 + 2] = color;
            colors[number * 4 + 3] = color;
        }

        void SetNodeListColorByWorldPosition(List<Vector3> positions, Color color)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                SetNodeByWorldPosition(ref mColors, positions[i], color);
            }
            ApplyColors();
        }

        public void ResetNodeColorByWorldPosition(Vector3 position)
        {
            SetNodeByWorldPosition(ref mColors, position, normalColor);
        }

        public void ResetNodeColorByLocalPosition(Vector3 position)
        {
            SetNodeByLocalPosition(ref mColors, position, normalColor);
        }

        public void HoverNodeColorByWorldPosition(Vector3 position)
        {
            SetNodeByWorldPosition(ref mColors, position, hoverColor);
        }

        public void HoverNodeColorByLocalPosition(Vector3 position)
        {
            SetNodeByLocalPosition(ref mColors, position, hoverColor);
        }

        public void BlockNodeColorByWorldPosition(Vector3 position)
        {
            SetNodeByWorldPosition(ref mColors, position, blockColor);
        }

        public void BlockNodeColorByLocalPosition(Vector3 position)
        {
            SetNodeByLocalPosition(ref mColors, position, blockColor);
        }

        public void DisableNodeColorByWorldPosition(Vector3 position)
        {
            SetNodeByWorldPosition(ref mColors, position, disableColor);
        }

        public void DisableNodeColorByLocalPosition(Vector3 position)
        {
            SetNodeByLocalPosition(ref mColors, position, disableColor);
        }

        public void EnableNodeColorByWorldPosition(Vector3 position)
        {
            SetNodeByWorldPosition(ref mColors, position, normalColor);
        }
        //TODO
        public void EnableNodeColorByLocalPosition(Vector3 position)
        {
            SetNodeByLocalPosition(ref mColors, position, normalColor);
        }

        public void ResetNodeColors(List<Vector3> positions)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                SetNodeByWorldPosition(ref mColors, positions[i], normalColor);
            }
            ApplyColors();
        }

        public void HoverNodes(List<Vector3> positions)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                SetNodeByWorldPosition(ref mColors, positions[i], hoverColor);
            }
            ApplyColors();
        }

        public void BlockNodes(List<Vector3> positions)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                SetNodeByWorldPosition(ref mColors, positions[i], blockColor);
            }
            ApplyColors();
        }

        public void ResetNodeColors()
        {
            for (int i = 0; i < mColors.Length; i++)
            {
                mColors[i] = normalColor;
            }
            ApplyColors();
        }

        public void ApplyColors()
        {
            mMesh.colors = mColors;
        }

        public Bounds GetBounds(){
            return boxCollider.bounds;
        }
    }
}