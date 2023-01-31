using UnityEngine;
using UnityEditor;

namespace SeroJob.BroadcastingSystem.Editor
{
    
    [CustomEditor(typeof(BroadcastChannel), true)]
    public class BroadcastChannelEditor : UnityEditor.Editor
    {
        private BroadcastChannelDebugContent content;

        private bool _showContent;

        void OnEnable()
        {
            _showContent = false;
            content = null;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Find Content"))
            {
                _showContent = true;
            }

            if (_showContent)
            {
                if (content == null) FindContent();
                if (content == null)
                {
                    return;
                }

                ShowContent();
            }
        }

        private void FindContent()
        {
            BroadcastChannel channel = (BroadcastChannel)target;
            content = channel.GetBroadcastContent();
        }

        private void ShowContent()
        {
            EditorGUILayout.BeginVertical();

            foreach (var content in content.Content)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.TextArea(content.MethodTarget.ToString());
                EditorGUILayout.TextArea(content.MethodName);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }
    }
}