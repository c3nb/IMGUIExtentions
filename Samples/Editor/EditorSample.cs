using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorSample : UnityEditor.EditorWindow
{

    static List<float> list;
    static List<float> list2;
    static List<float> list3;
    public static int mSlider;
    public static int mSlider2;
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
        list2 = new List<float>();
        list3 = new List<float>();
        for (var i = 0; i < 1000; i++)
        {
            
            var v = -1000 + i;
            list.Add(v);

            var v2 = Mathf.Sin((i % 360.0f) / 180.0f * Mathf.PI);
            list2.Add(v2);
            list3.Add(v2 * 0.5f);
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
                    var ofst = rect.width > list.Count ? rect.width - list.Count : 0;                                        
                    mSelect = (int)(Event.current.mousePosition.x - rect.x - ofst);                    
                }
                var sub = (int)(list.Count - rect.width);
                sub = Mathf.Max(sub, 0);
                mSlider = UnityEditor.EditorGUILayout.IntSlider(mSlider, 0, sub);


                UnityEditor.EditorGUILayout.Space();

                Vector2 pos = Event.current.mousePosition;

                var w = 1;
                rect = UnityEditor.EditorGUILayout.GetControlRect(GUILayout.Height(300));
                var scale = UTJ.EditorGUI.Graph(rect, new GUIContent(), list2, mSlider2, Color.green, -2, pos,Color.red, w, UnityEngine.Color.gray, true, Color.white,0f, UnityEngine.GUI.skin.box); ;
                UTJ.EditorGUI.Graph(rect, new GUIContent(), list3, mSlider2, Color.yellow,-2, pos,Color.red, w, new Color32(0,0,0,0), true, Color.white,scale,UnityEngine.GUI.skin.box);

                sub = (int)(list.Count - rect.width / w);
                sub = Mathf.Max(sub, 0);
                mSlider2 = UnityEditor.EditorGUILayout.IntSlider(mSlider2, 0, sub);


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
