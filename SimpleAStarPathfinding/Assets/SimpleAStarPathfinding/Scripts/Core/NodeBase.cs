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
    public class NodeBase
    {
        [SerializeField]
        protected float _x;
        [SerializeField]
        protected float _y;
        [SerializeField]
        protected float _z;

        public float X { get { return _x; } }
        public float Y { get { return _y; } set { _y = value; } }
        public float Z { get { return _z; } }

        public NodeBase() { }

        public NodeBase(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    }
}