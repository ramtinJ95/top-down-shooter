using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator map = target as MapGenerator; // target is this [CustomEditor (typeof(MapGenerator))]

        // This runs as long as we have selected the map object and have it in our inspector view in the editor.
        if (GUI.changed)
        {
            map.GenerateMap();
        }
    }
}
