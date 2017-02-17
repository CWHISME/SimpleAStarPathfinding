
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
    public class Tools
    {
        public static Vector3 NodeToPosition(Node node)
        {
            return new Vector3(node.X, node.Y, node.Z);
        }

        /// <summary>
        /// 通过一个坐标点生成一个Node
        /// 会通过Raycast检查节点是否可一通行
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Node CreateNode(Vector3 pos)
        {
            Node node = new Node(pos.x, pos.y, pos.z);
            RaycastHit hit;
            if (Physics.Raycast(pos + Vector3.up * 100, Vector3.down, out hit, 120))
            {
                node.Y = hit.point.y;
                //通过Tag来判断是否属于障碍物
                if (hit.transform.CompareTag("Obstacle"))
                    node.IsObstacle = true;
            }

            return node;
        }
    }
}