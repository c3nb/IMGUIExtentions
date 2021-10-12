using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UTJ
{
    // Katsumasa.Kimura
    public partial class GUILayout
    {
        static public Rect Graph(GUIContent content, List<float> srcs, params UnityEngine.GUILayoutOption[] options)
        {
            return Graph(content, srcs, 0, options);
        }


        static public Rect Graph(GUIContent content, List<float> srcs, int index, params UnityEngine.GUILayoutOption[] options)
        {
            return Graph(content, srcs, index, UnityEngine.GUI.contentColor, options);
        }


        static public Rect Graph(GUIContent content, List<float> srcs, int index, Color color, params UnityEngine.GUILayoutOption[] options)
        {

            return Graph(content, srcs, index, color, -1, Color.black, options);
        }

        static public Rect Graph(GUIContent content, List<float> srcs, int index, Color color, int select, Color selectColor, params UnityEngine.GUILayoutOption[] options)
        {
            return Graph(content, srcs, index, color, select, selectColor, UnityEngine.GUI.skin.box, options);
        }


        static public Rect Graph(GUIContent content, List<float> srcs, int index, Color color, int select, Color selectColor, GUIStyle style, params UnityEngine.GUILayoutOption[] options)
        {
            float w = index >= 0 ? (srcs.Count - index) : srcs.Count;
            float h = 300;

#if false
            // Optionの内容を反映
            foreach (var option in options)
            {
                var opt = new UTJ.GUILayoutOption(option);
                switch (opt.type)
                {
                    case GUILayoutOption.Type.fixedWidth:
                        w = (float)opt.value;
                        break;

                    case GUILayoutOption.Type.fixedHeight:
                        h = (float)opt.value;
                        break;

                    case GUILayoutOption.Type.maxWidth:
                        w = Mathf.Min(w, (float)opt.value);
                        break;

                    case GUILayoutOption.Type.maxHeight:
                        h = Mathf.Min(h, (float)opt.value);
                        break;

                    case GUILayoutOption.Type.minWidth:
                        w = Mathf.Max(w, (float)opt.value);
                        break;

                    case GUILayoutOption.Type.minHeight:
                        h = Mathf.Min(h, (float)opt.value);
                        break;
                }
            }
#endif
            // 領域を確保
            var rect = GUILayoutUtility.GetRect(w, h,style,options);                    
            // グラフを描画
            UTJ.GUI.Graph(rect, content, srcs,index,color, select,selectColor,style);
            return rect;
        }
               
    }
}