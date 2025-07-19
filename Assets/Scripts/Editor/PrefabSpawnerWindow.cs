using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabSpawnerWindow : EditorWindow
{
    private static bool _isSpawningEnabled = true;
    private int _selectedIndex = 0;
    private GUIStyle _labelStyle;
    private Dictionary<string, GameObject> _prefabDictionary;

    private string[] _dropDownOptions = new string[] 
    {
        "Prefab_Shadow",
        "Prefab_Heart",
        "Prefab_Flower",
        "Prefab_Star",
        "Prefab_Speed",
        "Prefab_MovingFloor",
        "Prefab_Coin",
        "Prefab_Floor",
        "Prefab_Spikes",
        "Prefab_DissapearingFloor"
    };

    [MenuItem("Tools/Prefab Spawner")]
    public static void ShowWindow()
    {
       var window = GetWindow<PrefabSpawnerWindow>();
       window.titleContent = new GUIContent("Prefab Spawner");
       window.Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        _labelStyle = new GUIStyle();
        _labelStyle.normal.textColor = Color.white;

        if (_prefabDictionary == null || _prefabDictionary.Count == 0)
            LoadPrefabs();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        _selectedIndex = EditorGUILayout.Popup("Select Option",_selectedIndex,_dropDownOptions);
        EditorGUILayout.Space();

        if (GUILayout.Button("Toggle Prefab Spawning"))
            TogglePrefabSpawning();

        GUILayout.Label("Prefab Spawning Status: " + (_isSpawningEnabled ? "<color=yellow>Enabled</color>" : "<color=red>Disabled</color>"), _labelStyle);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        // Make sure prefabs are loaded and the selected index is valid
        if (_prefabDictionary == null || !_prefabDictionary.ContainsKey(_dropDownOptions[_selectedIndex]))
            return;

        Event current = Event.current; // Get the current scene event (e.g., mouse, key)

        // Respond to right-click (mouse button 1)
        if (current.type == EventType.MouseDown && current.button == 1)
        {
            // Convert GUI mouse position to a world ray
            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            Vector3 mouseWorldPos = ray.origin;

            // Round the world position to whole numbers for grid alignment
            Vector3 mouseWorldPosRounded = new Vector3(
                Mathf.RoundToInt(mouseWorldPos.x),
                Mathf.RoundToInt(mouseWorldPos.y),
                0
            );

            // If Shift is held → delete object at that position
            if (current.shift)
            {
                // Check for any 2D collider at the clicked position
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosRounded);
                if (hit != null)
                {
                    // Destroy the object immediately (editor mode)
                    GameObject.DestroyImmediate(hit.gameObject);
                    Debug.Log("Deleted object at " + mouseWorldPosRounded);
                }
            }
            // If just right-click → spawn selected prefab
            else if (_isSpawningEnabled)
            {
                // Instantiate selected prefab at the clicked world position
                Instantiate(
                    _prefabDictionary[_dropDownOptions[_selectedIndex]],
                    mouseWorldPosRounded,
                    Quaternion.identity
                );

                Debug.Log("Spawned at " + mouseWorldPosRounded);
            }

            current.Use(); // Mark the event as handled
        }
    }


    private void TogglePrefabSpawning()
    {
       _isSpawningEnabled = !_isSpawningEnabled;
    }

    private void LoadPrefabs()
    {
        _prefabDictionary = new Dictionary<string, GameObject>();
        foreach (string n in _dropDownOptions)
        {
            _prefabDictionary.Add(n,Resources.Load<GameObject>("Tiles/" + n));
        }
    }
}
