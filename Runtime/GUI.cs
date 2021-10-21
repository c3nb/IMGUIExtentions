using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


namespace UTJ
{
    // Katsumasa.Kimura
    public partial class GUI
    {
        static public void Graph(Rect rect, GUIContent content, List<float> srcs)
        {
            Graph(rect, content, srcs, 0);
        }


        static public void Graph(Rect rect, GUIContent content, List<float> srcs, int index)
        {
            Graph(rect, content, srcs, index, UnityEngine.GUI.contentColor);
        }



        static public void Graph(Rect rect, GUIContent content, List<float> srcs, int index, Color color)
        {
            Graph(rect, content, srcs, index, color, -1, Color.red);
        }


        static public void Graph(Rect rect, GUIContent content, List<float> srcs, int index, Color color, int select, Color selectColor)
        {
            Graph(rect, content, srcs, index, color, select, selectColor, UnityEngine.GUI.skin.box);
        }


        /// <summary>
        /// グラフの描画を行う
        /// </summary>
        /// <param name="rect">グラフの描画を行う</param>
        /// <param name="content">グラフのタイトル</param>
        /// <param name="srcs">グラフに描画するデータ</param>
        /// <param name="index">描画を開始するインデックス</param>
        /// <param name="color">グラフの井戸</param>
        /// <param name="select">グラフを強調表示するインデックス</param>
        /// <param name="selectColor">協調表示する色</param>
        /// <param name="style">スタイル（未使用）</param>
        static public void Graph(Rect rect, GUIContent content, List<float> srcs, int index, Color color, int select, Color selectColor, GUIStyle style)
        {
            Graph(rect, content, srcs, index, color, select, Vector2.zero,selectColor, 1, UnityEngine.Color.gray, true, Color.white, 0f, UnityEngine.GUI.skin.box);
        }

        static public float Graph(
        Rect rect,
        GUIContent content,
        List<float> srcs,
        int index,
        Color color,
        int select,
        Vector2 position,
        Color selectColor,
        int barWidth,
        Color backColor,
        bool isEnabledAdditionalLine,
        Color additionalLineColor,
        float scale,
        GUIStyle style
        ) 
            {
            int len = ((int)rect.width) / barWidth;
            var ofst = Mathf.Min(index, srcs.Count - len);
            ofst = Mathf.Max(0, ofst);
            len = Mathf.Min(len, srcs.Count - ofst);


            // indexの位置からグラフに表示される範囲でリストを作成する
            var list = new List<float>();            
            for(var i = 0; i < len; i++)
            {
                list.Add(srcs[i + ofst]);
            }


            // 背景                        
            UnityEngine.GUI.Box(rect, content,style);

            if (list.Count != 0)
            {
                Texture2D texture = Texture2D.whiteTexture;


                var minValue = list.Min();
                var maxValue = list.Max();
                var avgValue = list.Average();

                /// 底上げされる可能性がある為、最小値・最大値・平均値を退避
                var realMin = minValue;
                var realMax = maxValue;
                var realAvg = avgValue;

                // 最小値が0より小さい場合、グラフ的には最小値が0となるように底上げする
                if (minValue < 0f)
                {
                    for (var i = 0; i < len; i++)
                    {
                        list[i] += Mathf.Abs(minValue);
                    }
                    minValue = list.Min();
                    maxValue = list.Max();
                    avgValue = list.Average();
                }
                // 最大値の高さが描画範囲の90%位に                                
                if (scale <= 0f)
                {
                    scale = rect.height / maxValue * 0.90f;
                }


                for (var i = 0; i < list.Count; i++)
                {
                    var w = 1.0f;
                    var h = list[i] * scale;
                    var x = rect.x + rect.width - len + i * w;
                    var y = rect.y + rect.height - h;
                    var r = new Rect(x, y, w, h);

                    if ((select == -2) && r.Contains(position))
                    {
                        select = i;
                    }
                    if (i == select)
                    {                        
                        UnityEngine.GUI.DrawTexture(r,Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, selectColor, 0, 0);
                    }
                    else
                    {
                        UnityEngine.GUI.DrawTexture(r, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, color, 0, 0);                        
                    }
                }

                if (isEnabledAdditionalLine)
                {
                    // 最大値の補助線
                    {
                        var x = rect.x;
                        var y = rect.y + rect.height - maxValue * scale;
                        var w = rect.width;
                        var h = 1.0f;
                        DrawAdditionalLine(new Rect(x, y, w, h), realMax, additionalLineColor);
                    }

                    // 平均値の補助線
                    {
                        var x = rect.x;
                        var y = rect.y + rect.height - avgValue * scale;
                        var w = rect.width;
                        var h = 1.0f;
                        DrawAdditionalLine(new Rect(x, y, w, h), realAvg, additionalLineColor);
                    }

                    // 最小値の補助線
                    {
                        var x = rect.x;
                        var y = rect.y + rect.height - minValue * scale;
                        var w = rect.width;
                        var h = 1.0f;
                        DrawAdditionalLine(new Rect(x, y, w, h), realMin, additionalLineColor);
                    }
                }

                // 選択された値を表示する
                if (select >= 0 && select < list.Count)
                {
                    var value = list[select];
                    if (realMin < 0f)
                    {
                        value -= Mathf.Abs(realMin);
                    }
                    var label = new GUIContent(Format("{0,3:F1}", value));
                    var contentSize = UnityEngine.GUI.skin.label.CalcSize(label);
                    var x = rect.x + rect.width - len + select * 1.0f - contentSize.x / 2;
                    var y = rect.y + rect.height - list[select] * scale - contentSize.y;
                    var w = contentSize.x;
                    var h = contentSize.y;

                    var r = new Rect(x, y, w, h);                    
                    UnityEngine.GUI.DrawTexture(r,Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, new Color32(0,0,0,128), 0, 0);
                    UnityEngine.GUI.Label(r, label);

                    var frame = ofst + select;
                    label = new GUIContent(Format("{0,3}", frame));
                    contentSize = UnityEngine.GUI.skin.label.CalcSize(label);
                    x = rect.x + rect.width - len + select * 1.0f - contentSize.x / 2;
                    y = rect.y + rect.height - contentSize.y;
                    w = contentSize.x;
                    h = contentSize.y;
                    r = new Rect(x, y, w, h);
                    UnityEngine.GUI.DrawTexture(r, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, new Color32(0, 0, 0, 128), 0, 0);
                    UnityEngine.GUI.Label(r, label);
                }                
            }
            return scale;
        }


        /// <summary>
        /// 補助線を引く
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="value"></param>
        /// <param name="color"></param>
        static void DrawAdditionalLine(Rect rect, float value, Color color)
        {
            UnityEngine.GUI.DrawTexture(rect, Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,Color.white,0,0);
            var label = new GUIContent(Format("{0,3:F1}", value));
            var contentSize = UnityEngine.GUI.skin.label.CalcSize(label);            
            var rect2 = new Rect(rect.x, rect.y - contentSize.y / 2, contentSize.x, contentSize.y);
            UnityEngine.GUI.DrawTexture(rect2, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, Color.black, 0, 0);
            UnityEngine.GUI.Label(rect2, label);
        }


        public static string Format(string fmt, params object[] args)
        {
            return System.String.Format(CultureInfo.InvariantCulture.NumberFormat, fmt, args);

        }
    }
}