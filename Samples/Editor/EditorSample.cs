using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorSample : UnityEditor.EditorWindow
{

    static List<float> list;
    public static int mSlider;
    public int mSelect;
    
    
    [MenuItem("Window/UTJ/IMGUI Extentions/Editor Sample")]    
    public static void  Open()
    {
        var window = (EditorSample)EditorWindow.GetWindow(typeof(EditorSample));
        window.wantsMouseMove = true;
    }


    private void OnEnable()
    {
        list = new List<float>();

        for (var i = 0; i < 1000; i++)
        {
            var v = Mathf.Sin((i % 360.0f) / 180.0f * Mathf.PI);
            //var v = 1000 + i;
            list.Add(v);
        }
        mSelect = -1;

    }

    public void OnGUI()
    {
        UnityEditor.EditorGUILayout.BeginHorizontal();
        {
            UnityEditor.EditorGUILayout.BeginVertical();
            {
                UnityEditor.EditorGUILayout.LabelField("UTJ.EditorGUILayout.Graph Sample");
                UnityEditor.EditorGUILayout.Space();
                UnityEditor.EditorGUILayout.LabelField("Mouse Position (" + Event.current.mousePosition.x + "," + Event.current.mousePosition.y + ")");                
            }
            UnityEditor.EditorGUILayout.EndVertical();

            UnityEditor.EditorGUILayout.BeginVertical();
            {
                var rect = UTJ.EditorGUILayout.Graph(new GUIContent("TEST"), list, mSlider, Color.blue, mSelect, Color.red, GUILayout.Height(300));
                if (rect.Contains(Event.current.mousePosition))
                {
                    mSelect = (int)(Event.current.mousePosition.x - rect.x);
                }


                var sub = (int)(list.Count - rect.width);
                sub = Mathf.Max(sub, 0);
                mSlider = UnityEditor.EditorGUILayout.IntSlider(mSlider, 0, sub);

                UnityEditor.EditorGUILayout.LabelField("GrapRect("+ rect.x + "," + rect.y + "," + rect.width + "," + rect.height + ")");

            }
            UnityEditor.EditorGUILayout.EndVertical();
        }
        UnityEditor.EditorGUILayout.EndHorizontal();
    }

    void OnInspectorUpdate()
    {
        if (EditorWindow.mouseOverWindow)
        {
            EditorWindow.mouseOverWindow.Focus();
        }

        this.Repaint();
    }
}
