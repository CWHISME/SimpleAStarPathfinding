using System.Collections.Generic;
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
    public class SimpleAStarManager
    {

        private static SimpleAStarManager _instance;
        public static SimpleAStarManager GetInstance
        {
            get
            {
                if (_instance == null) _instance = new SimpleAStarManager();
                return _instance;
            }
        }

        private readonly int _GAdder = 10;
        private readonly int _HAdder = 20;

        //暂时，假设一个场景只会有一个寻路数据
        private SimpleAStar _aStar;
        private float _gridSize;

        public void Register(SimpleAStar aStar)
        {
            _aStar = aStar;

            _gridSize = _aStar.GridSize;
        }

        private Node _startNode;
        private Node _endNode;
        private List<Node> _openList = new List<Node>(100);
        private List<Node> _closeList = new List<Node>(100);
        private List<Vector3> _pathList = new List<Vector3>();

#if UNITY_EDITOR
        public List<Node> OpenList { get { return _openList; } }
        public List<Node> CloseList { get { return _closeList; } }
#endif

        /// <summary>
        /// 计算一条从指定点到指定点的路径
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void CalcPath(Vector3 start, Vector3 end, System.Action<Vector3[]> callBack)
        {
            //Debug.Log("Reuest");
            //计算起点位于数据图中的坐标
            _startNode = GetNode(start, _aStar);
            if (_startNode.IsObstacle) return;
            //计算终点位于数据图中的坐标
            _endNode = GetNode(end, _aStar);
            if (_endNode.IsObstacle) return;

            //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CalcPathThread));
            //thread.Start(callBack);
            CalcPathThread(callBack);
        }

        private void CalcPathThread(object callback)
        {
            //Debug.Log("Thread");
            //lock (this)
            {

                _pathList.Clear();
                _openList.Clear();
                _closeList.Clear();

                //将其父级及消耗置为空，避免多次使用可能出现的问题
                _startNode.ParentNode = null;
                _startNode.G = 0;
                _startNode.H = 0;

                _openList.Add(_startNode);
                do
                {
                    Node currentNode = _openList[0];
                    _openList.RemoveAt(0);
                    _closeList.Add(currentNode);

                    //计算邻边
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            //目前的话，我们仅考虑四个方向
                            //为了简单嘛
                            if (i == j) continue;
                            Node node = GetNode(currentNode.IndexX + i, currentNode.IndexY + j);
                            if (node != null)
                            {
                                //若节点就是结束点，那么...
                                if (node == _endNode)
                                {
                                    node.ParentNode = currentNode;
                                    node.BuildPath(_pathList);

                                    if (callback != null)
                                    {
                                        Vector3[] path = _pathList.ToArray();
                                        //最后将路径反向
                                        //System.Array.Reverse(path);
                                        (callback as System.Action<Vector3[]>).Invoke(path);
                                    }
                                    return;
                                }

                                //已经位于关闭表，跳过
                                if (_closeList.Contains(node)) continue;
                                //若节点是障碍物，直接放入关闭列表
                                if (node.IsObstacle)
                                {
                                    _closeList.Add(node);
                                    continue;
                                }
                                //计算开始到当前节点消耗
                                int G = CalcG(currentNode);
                                //当前点至终点消耗
                                int H = CalcH(node, _endNode);

                                //开启表中存在节点
                                if (_openList.Contains(node))
                                {
                                    //判断新的路径消耗
                                    if (G < node.G)
                                    {
                                        node.G = G;
                                        node.H = H;
                                        node.ParentNode = currentNode;
                                        node.RecalcF();
                                    }
                                }
                                else
                                {
                                    //开启列表不存在节点
                                    node.G = G;
                                    node.H = H;
                                    node.ParentNode = currentNode;
                                    node.RecalcF();
                                    //添加至开启列表
                                    _openList.Add(node);
                                }
                            }
                        }
                    }

                    //重新排序
                    _openList.Sort();
                    //Debug.Log(_openList.Count);
                } while (_openList.Count > 0);

                //Debug.Log("Thread End");
            }
        }

        /// <summary>
        /// 通过索引获取Node，将会判断是否合法
        /// </summary>
        /// <param name="indexX"></param>
        /// <param name="indexY"></param>
        /// <returns></returns>
        private Node GetNode(int indexX, int indexY)
        {
            if (indexX >= _aStar.MapData.GetLength(0) || indexY >= _aStar.MapData.GetLength(1) || indexX < 0 || indexY < 0) return null;
            //Debug.Log(indexX + "  " + indexY);
            return _aStar.MapData[indexX, indexY];
        }

        private int CalcG(Node node)
        {
            return node.G + (int)(_gridSize * _GAdder);
        }

        private int CalcH(Node node, Node endNode)
        {
            switch (_aStar.Heuristic)
            {
                case EnumHeuristic.Manhattan:
                    //使用曼哈顿距离进行Hint
                    return (int)(Mathf.Abs(endNode.X - node.X) + Mathf.Abs(endNode.Z - node.Z)) * _HAdder;
                //欧几里德距离
                case EnumHeuristic.Euclid:
                default:
                    return (int)Vector3.Distance(endNode.Position, node.Position) * _HAdder;
            }
        }

        private static Node GetNode(Vector3 pos, SimpleAStar aStar)
        {
            Vector3 originPos = aStar.MapOriginPosition;
            int x = (int)((pos.x - originPos.x) / aStar.GridSize);
            int y = (int)((pos.z - originPos.z) / aStar.GridSize);

            //返回计算出的点
            return aStar.MapData[x, y];
        }

    }
}