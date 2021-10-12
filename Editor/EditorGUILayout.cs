using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UTJ
{
    // Katsumasa.KImura
    public partial class EditorGUILayout
    {
        static public Rect Graph(GUIContent content, List<float> srcs, params UnityEngine.GUILayoutOption[] options)
        {
            return Graph(content, srcs, 0, options);
        }


        static public Rect Graph(GUIContent content, List<float> srcs, int index, params UnityEngine.GUILayoutOption[] options)
        {
            return Graph(content, srcs, index, UnityEngine.GUI.contentColor, options);
        }


        static public Rect Graph(GUIContent content, List<float> srcs, int index, Color color,params UnityEngine.GUILayoutOption[] options)
        {
            return Graph(content, srcs, index,color,-1,Color.black, options);
        }


        static public Rect Graph(GUIContent content, List<float> srcs, int index, Color color, int select, Color selectColor,params UnityEngine.GUILayoutOption[] options)
        {
            return Graph(content, srcs, index, color, select,selectColor,UnityEngine.GUI.skin.box, options);
        }


        /// <summary>
        /// グラフの描画を行う
        /// </summary>
        /// <param name="content">グラフのタイトル</param>
        /// <param name="srcs">グラフに描画するデータ</param>
        /// <param name="index">描画を開始するインデックス</param>
        /// <param name="color">グラフの井戸</param>
        /// <param name="select">グラフを強調表示するインデックス</param>
        /// <param name="selectColor">協調表示する色</param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public Rect Graph(GUIContent content, List<float> srcs, int index, Color color, int select,Color selectColor,GUIStyle style, params UnityEngine.GUILayoutOption[] options)
        {
            // 領域を確保(オプションもこの中で処理)
            var rect = UnityEditor.EditorGUILayout.GetControlRect(options);
            // グラフを描画
            UTJ.EditorGUI.Graph(rect, content, srcs, index, color,select,selectColor,style);
            return rect;
        }
    }
}