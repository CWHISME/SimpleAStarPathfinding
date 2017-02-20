using System.Diagnostics;
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
    public class SimpleAStarTest : MonoBehaviour
    {

        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3[] _path;

        private string _info;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (_startPos == Vector3.zero)
                    {
                        _startPos = hit.point;
                        return;
                    }

                    _endPos = hit.point;
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    SimpleAStarManager.GetInstance.CalcPath(_startPos, _endPos, (path) =>
                    {
                        watch.Stop();
                        _info = "上一次消耗：" + watch.ElapsedMilliseconds + " 毫秒";
                        _path = path;
                    });
                }
            }
        }

        public void OnGUI()
        {
            if (GUILayout.Button("清除寻路数据"))
            {
                _startPos = Vector3.zero;
                _endPos = Vector3.zero;
                _path = null;
            }

            GUILayout.Label(_info);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 1f);
            if (_startPos != Vector3.zero)
                Gizmos.DrawSphere(_startPos, 0.5f);
            Gizmos.color = new Color(0, 0, 0, 1f);
            if (_endPos != Vector3.zero)
                Gizmos.DrawSphere(_endPos, 0.5f);
            Gizmos.color = Color.green;
            if (_path != null)
                for (int i = 0; i < _path.Length - 1; i++)
                {
                    Gizmos.DrawLine(_path[i], _path[i + 1]);
                }
        }

    }
}