using UnityEngine;

namespace SimpleAStar
{
    /// <summary>
    /// *******************************************
    ///   Powered By  Wangjiaying
    ///   Date: 2017.2.17
    ///   Func :
    /// *******************************************
    /// </summary>
    public class SimpleAStar : MonoBehaviour
    {

        //代表该A*的区域长和宽
        [SerializeField]
        private int _gridX = 50;
        [SerializeField]
        private int _gridY = 50;
        [SerializeField]
        //格子的大小
        private float _gridSize = 1f;
        public float GridSize { get { return _gridSize; } }
        [Tooltip("是否使用预先烘焙的数据？若选择false，则第一次运行时，将会自定重新计算一次")]
        [SerializeField]
        private bool _useBakeMapData = true;

        [HideInInspector]
        [SerializeField]
        private byte[] _rawMapData;

        private Node[,] _nodeList;

        private void Awake()
        {
            SimpleAStarManager.GetInstance.Register(this);

            //若不是预先计算的地图，那么第一次运行时，开始时刷新一下
            if (!_useBakeMapData)
                Scan();
            //否则，读取保存地图数据
            else if (!LoadMapData())
            {
                Debug.Log("数据加载失败！将会重新扫描地图数据！若目的是想实时计算，那么请取消勾选“UseBakeMapData”选项。");

                //若读取数据失败，那么也会直接实时计算一次
                Scan();
            }
        }

        /// <summary>
        /// 扫描(可以理解为烘焙一次寻路数据)
        /// </summary>
        public void Scan()
        {
            //初始化数组
            _nodeList = new Node[_gridX, _gridY];
            //起点，为当前物体的位置
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            for (int i = 0; i < _gridX; i++)
            {
                for (int j = 0; j < _gridY; j++)
                {
                    Node node = Tools.CreateNode(new Vector3(x + i * _gridSize, y, z + j * _gridSize));
                    node.SetIndex(i, j);
                    _nodeList[i, j] = node;

                }

#if UNITY_EDITOR
                int cur = i;
                UnityEditor.EditorUtility.DisplayProgressBar("Scan.....", "Scan A* Map Data: " + cur + "/" + (_gridX * _gridY), cur / _gridX);
#endif
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.ClearProgressBar();
#endif
        }

        public void SaveMapData()
        {
            _rawMapData = Tools.MapToByteData(_nodeList);
        }

        public bool LoadMapData()
        {
            _nodeList = Tools.ByteDataToMap(_rawMapData);
            return _nodeList != null;
        }

        /// <summary>
        /// 获取A*地图数据
        /// </summary>
        public Node[,] MapData
        {
            get { return _nodeList; }
        }

        public void ClearData()
        {
            _nodeList = null;
            _rawMapData = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_nodeList == null) return;
            Color normal = new Color(0, 0, 1, 0.3f);
            Color obstacle = new Color(1, 0, 0, 0.3f);
            for (int i = 0; i < _nodeList.GetLength(0); i++)
            {
                for (int j = 0; j < _nodeList.GetLength(1); j++)
                {
                    Node node = _nodeList[i, j];
                    Gizmos.color = node.IsObstacle ? obstacle : normal;
                    Gizmos.DrawWireCube(node.Position, Vector3.one * _gridSize);
                }
            }


            Gizmos.color = new Color(0, 0, 1, 0.3f);
            foreach (var node in SimpleAStarManager.GetInstance.OpenList)
            {
                Gizmos.DrawCube(node.Position, Vector3.one * _gridSize);
            }

            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            foreach (var node in SimpleAStarManager.GetInstance.CloseList)
            {
                Gizmos.DrawCube(node.Position, Vector3.one * _gridSize);
            }
        }
#endif

    }
}