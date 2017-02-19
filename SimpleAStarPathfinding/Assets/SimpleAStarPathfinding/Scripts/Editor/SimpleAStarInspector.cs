using UnityEditor;
using UnityEngine;

namespace SimpleAStar
{
    /// <summary>
    /// *******************************************
    ///   Powered By  Wangjiaying
    ///   Date: 
    ///   Func :
    /// *******************************************
    /// </summary>
    [CustomEditor(typeof(SimpleAStar))]
    public class SimpleAStarInspector : Editor
    {

        private GUIStyle _labelStyle;

        private void OnEnable()
        {
            SimpleAStar aStar = target as SimpleAStar;

            if (aStar.MapData == null)
                aStar.LoadMapData();

            _labelStyle = new GUIStyle();
            _labelStyle.normal.textColor = Color.white;
            _labelStyle.richText = true;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            base.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            SimpleAStar aStar = target as SimpleAStar;

            bool haveData = aStar.MapData != null;
            EditorGUILayout.LabelField("数据：" + (haveData ? "<color=green>存在！</color>" : "<color=red>不存在！</color>"), _labelStyle);
            if (haveData)
            {
                int x = aStar.MapData.GetLength(0);
                int y = aStar.MapData.GetLength(1);
                EditorGUILayout.LabelField("[" + x + " x " + y + "] 节点总数：" + (x * y) + "个");
            }

            if (GUILayout.Button("Scan", GUILayout.Height(30), GUILayout.Width(50)))
            {
                aStar.Scan();
                aStar.SaveMapData();
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }

            if (GUILayout.Button("Clear", GUILayout.Height(30), GUILayout.Width(50)))
            {
                aStar.ClearData();
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }

        }
    }
}