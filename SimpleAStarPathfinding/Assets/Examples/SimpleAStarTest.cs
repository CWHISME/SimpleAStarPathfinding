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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (_startPos == Vector3.zero)
                    {
                        _startPos = hit.point;
                        return;
                    }

                    _endPos = hit.point;
                    SimpleAStarManager.GetInstance.CalcPath(_startPos, _endPos, (path) => _path = path);
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