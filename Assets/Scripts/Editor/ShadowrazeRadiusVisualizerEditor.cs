using System;
using Unity.BossRoom.Gameplay.Actions;
using UnityEditor;
using UnityEngine;

namespace Unity.BossRoom.Editor
{
    [CustomEditor(typeof(ShadowrazeAction))]
    public class ShadowrazeRadiusVisualizerEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
        
        private void OnSceneGUI(SceneView sceneView)
        {
            var action = (ShadowrazeAction)target;

            Handles.color = Color.red;
            Handles.DrawWireArc(action.Position, Vector3.up, Vector3.forward, 360, action.Radius);
        }
    }
}
