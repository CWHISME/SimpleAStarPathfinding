using System;
using System.Collections;
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
    [System.Serializable]
    public class Node : NodeBase, IComparable<Node>
    {

        public uint NodeLabel;

        private Node _parentNode;

        private int _G;
        private int _H;
        private int _F;
        public int G { get { return _G; } set { _G = value; } }
        public int H { get { return _H; } set { _H = value; } }
        public int F { get { return _F; } }

        //用于方便从地图数据中取出的索引
        [SerializeField]
        private int _indexX;
        [SerializeField]
        private int _indexY;
        public int IndexX { get { return _indexX; } }
        public int IndexY { get { return _indexY; } }

        [SerializeField]
        private bool _obstacle = false;
        public bool IsObstacle { get { return _obstacle; } set { _obstacle = value; } }

        public Node() { }

        public Node(float x, float y, float z) : base(x, y, z) { }

        public Node ParentNode { set { _parentNode = value; } }

        public Vector3 Position { get { return new Vector3(X, Y, Z); } }

        public void SetIndex(int x, int y)
        {
            _indexX = x;
            _indexY = y;
        }

        public void RecalcF()
        {
            _F = _G + _H;
        }

        public void BuildPath(List<Vector3> path)
        {
            path.Insert(0, Position);
            if (_parentNode != null)
                _parentNode.BuildPath(path);
        }

        int IComparable<Node>.CompareTo(Node other)
        {
            return F - other.F;
        }
    }
}