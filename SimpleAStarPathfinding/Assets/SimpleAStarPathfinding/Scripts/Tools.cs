
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

        /// <summary>
        /// 地图数据转化为二进制数据
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        public static byte[] MapToByteData(Node[,] maps)
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(stream))
                {
                    int x = maps.GetLength(0);
                    int y = maps.GetLength(1);
                    w.Write(x);
                    w.Write(y);
                    for (int i = 0; i < x; i++)
                    {
                        for (int j = 0; j < y; j++)
                        {
                            w.Write(JsonUtility.ToJson(maps[i, j]));
                        }
#if UNITY_EDITOR
                        int cur = i;
                        UnityEditor.EditorUtility.DisplayProgressBar("Save.....", "Save Map Data: " + cur * y + "/" + (x * y), cur / x);
#endif
                    }
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.ClearProgressBar();
#endif
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 存储的二进制数据转化为地图数据
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public static Node[,] ByteDataToMap(byte[] byteData)
        {
            if (byteData == null || byteData.Length < 1) return null;

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(byteData))
            {
                using (System.IO.BinaryReader r = new System.IO.BinaryReader(stream))
                {
                    int x = r.ReadInt32();
                    int y = r.ReadInt32();

                    Node[,] maps = new Node[x, y];

                    for (int i = 0; i < x; i++)
                    {
                        for (int j = 0; j < y; j++)
                        {
                            maps[i, j] = JsonUtility.FromJson<Node>(r.ReadString());
                        }
#if UNITY_EDITOR
                        int cur = i;
                        UnityEditor.EditorUtility.DisplayProgressBar("Load.....", "Load Map Data: " + cur * y + "/" + (x * y), cur / x);
#endif
                    }
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.ClearProgressBar();
#endif
                    return maps;
                }
            }
        }
    }
}