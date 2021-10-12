using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sample : MonoBehaviour
{
    List<float> list;
    public static int mSlider;
    public int mSelect;


    // Start is called before the first frame update
    void Start()
    {
        list = new List<float>();
        mSelect = -1;
        for (var i = 0; i < 1000; i++)
        {
            var v = Mathf.Sin((i % 360.0f) / 180.0f * Mathf.PI);
            list.Add(v);
        }
    
    }

    private void OnGUI()
    {

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("UTJ.IMGUIExtentions.GUILayout Sample");
                GUILayout.Space(2);                
                GUILayout.Label(string.Format("Mouse Position ({0:D4},{1:D4})", (int)Input.mousePosition.x, (int)Input.mousePosition.y));
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            {
                var rect = UTJ.GUILayout.Graph(new GUIContent("TEST"), list, mSlider, Color.blue, mSelect, Color.red, GUILayout.MaxWidth(400), GUILayout.Height(300));
                if (rect.Contains(Event.current.mousePosition))
                {
                    mSelect = (int)(Input.mousePosition.x - rect.x);
                }
                var rightValue = (int)(list.Count - rect.width);
                rightValue = Mathf.Max(rightValue, 0);
                mSlider = (int)GUILayout.HorizontalSlider(mSlider, 0, rightValue);

                GUILayout.Label("GrapRect(" + rect.x + "," + rect.y + "," + rect.width + "," + rect.height + ")");
            }
            GUILayout.EndVertical();
            
        }
        GUILayout.EndHorizontal();
    }
}
