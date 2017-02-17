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
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            base.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            SimpleAStar aStar = target as SimpleAStar;
            if (GUILayout.Button("Scan", GUILayout.Height(30), GUILayout.Width(50)))
            {
                aStar.Scan();
            }
        }
    }
}