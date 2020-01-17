using UnityEngine;
using DG.Tweening;

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

        int mLayer;

        public MeshCollider meshCollider;

        RectInt mRectInt;

        public RectInt VeiwRect { get { return mRectInt; } }

        //notice: padding is persentage.
        public void InitGridView(int xCount, int yCount, float gridSize, float padding, Material material, int layer,RectInt rectInt)
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
            mRectInt = rectInt;
            mLayer = layer;
            InitGridView();
        }

        void InitGridView()
        {
            GameObject go = MeshUtility.DrawGridGameObject(mMaterial, Color.white, mNodeSize, mXCount, mYCount, mPadding * mNodeSize, mRectInt);
            mMesh = go.GetComponent<MeshFilter>().mesh;
            mColors = mMesh.colors;
            go.transform.SetParent(mTrans);
            go.transform.localPosition = new Vector3(-mNodeSize * mXCount / 2f, 0, -mNodeSize * mYCount / 2f);
            go.name = "Grid";
            gridGameObject = go;
            meshCollider = go.GetOrAddComponent<MeshCollider>();
            go.layer = mLayer;
            HideGrid();
        }

        bool GetNodeStartIndex( int x, int z, out int number)
        {
            number = (x - mRectInt.x) * mRectInt.height + (z - mRectInt.y);
            if (number * 4 < 0 || number * 4 >= mColors.Length)
            {
                return false;
            }
            return true;
        }

        public void SetNodeColor(int x,int z, Color color)
        {
            int number = 0;
            if(GetNodeStartIndex(x,z,out number))
            {
                mColors[number * 4] = color;
                mColors[number * 4 + 1] = color;
                mColors[number * 4 + 2] = color;
                mColors[number * 4 + 3] = color;
            }
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

        public void ApplyColors()
        {
            mMesh.colors = mColors;
        }
    }
}