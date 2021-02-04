using System;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace UnityEditor.Rendering
{
    public class RealtimeProfilerWindow : EditorWindow
    {
        RealtimeProfilerModel m_Model;
        RealtimeProfilerViewModel m_ViewModel = new RealtimeProfilerViewModel();

        [MenuItem("Window/Analysis/Realtime Profiler")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<RealtimeProfilerWindow>();
            wnd.titleContent = new GUIContent("Realtime Profiler");
        }

        public void OnEnable()
        {
            var availableStatHandles = new List<ProfilerRecorderHandle>();
            ProfilerRecorderHandle.GetAvailable(availableStatHandles);

            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.unity.render-pipelines.core/Resources/UXML/realtimeprofiler-content.uxml");
            if (windowTemplate != null)
                windowTemplate.CloneTree(this.rootVisualElement);
            this.minSize = new Vector2(400, 250);

            EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        public void OnDisable()
        {
            EditorApplication.playModeStateChanged -= PlayModeStateChanged;
        }

        private void PlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                m_Model = FindObjectOfType<RealtimeProfilerModel>();
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                m_Model = null;
                m_ViewModel.ResetUI(this.rootVisualElement);
            }
        }

        public void OnInspectorUpdate()
        {
            if (m_Model != null)
            {
                m_ViewModel.UpdateUI(m_Model, this.rootVisualElement);
            }
        }
    }
}
#endif
