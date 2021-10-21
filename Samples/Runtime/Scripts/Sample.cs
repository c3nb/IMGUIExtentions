using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sample : MonoBehaviour
{
    static List<float> list;
    static List<float> list2;
    static List<float> list3;
    public static int mSlider;
    public static int mSlider2;
    public int mSelect;


    // Start is called before the first frame update
    void Start()
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

    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        var pos = Event.current.mousePosition;
#else
        var pos = Input.mousePosition;
#endif

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("UTJ.IMGUIExtentions.GUILayout Sample");
                GUILayout.Space(2);                
                GUILayout.Label(string.Format("Mouse Position ({0:D4},{1:D4})", (int)pos.x, (int)pos.y));
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            {
                
                var rect = UTJ.GUILayout.Graph(new GUIContent("TEST"), list, mSlider, Color.blue, mSelect, Color.red, GUILayout.MaxWidth(400), GUILayout.Height(300));
                if (rect.Contains(pos))
                {
                    var ofst = rect.width > list.Count ? rect.width - list.Count : 0;
                    mSelect = (int)(pos.x - rect.x - ofst);
                }
                var sub = (int)(list.Count - rect.width);
                sub = Mathf.Max(sub, 0);

                mSlider = (int)GUILayout.HorizontalSlider(mSlider, 0, sub);

                GUILayout.Space(2);


                var w = 1;
                 rect = GUILayoutUtility.GetRect(400, 300);                
                var scale = UTJ.GUI.Graph(rect, new GUIContent(), list2, mSlider2, Color.green, -2, pos, Color.red, w, UnityEngine.Color.gray, true, Color.white, 0f, UnityEngine.GUI.skin.box); ;
                UTJ.GUI.Graph(rect, new GUIContent(), list3, mSlider2, Color.yellow, -2, pos, Color.red, w, new Color32(0, 0, 0, 0), true, Color.white, scale, UnityEngine.GUI.skin.box);

                sub = (int)(list.Count - rect.width / w);
                sub = Mathf.Max(sub, 0);
                mSlider2 = (int)GUILayout.HorizontalSlider(mSlider2, 0, sub);


            }
            GUILayout.EndVertical();
            
        }
        GUILayout.EndHorizontal();
    }
}
