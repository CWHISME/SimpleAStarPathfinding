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

        [SerializeField]
        [HideInInspector]
        private Node[,] _nodeList;

        private void Awake()
        {
            SimpleAStarManager.GetInstance.Register(this);

            //测试代码
            //因为还没做保存功能，所以开始时刷新一下
            Scan();
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
            }
        }

        /// <summary>
        /// 获取A*地图数据
        /// </summary>
        public Node[,] MapData
        {
            get { return _nodeList; }
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