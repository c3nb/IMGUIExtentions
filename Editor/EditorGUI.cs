using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


namespace UTJ
{
    // Katsumasa.Kimura
    public partial class EditorGUI
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
            Graph(rect, content, srcs, index, color, -1,Color.red);
        }

        

        static public void Graph(Rect rect, GUIContent content, List<float> srcs, int index, Color color, int select, Color selectColor)
        {
            Graph(rect, content, srcs, index, color, select,selectColor, UnityEngine.GUI.skin.box);
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
        static public void Graph(Rect rect,GUIContent content, List<float> srcs,int index, Color color,int select,Color selectColor, GUIStyle style)
        {
            Graph(rect, content, srcs, index, color, select,Vector2.zero, selectColor, 1, UnityEngine.Color.gray, true,Color.white,0f,UnityEngine.GUI.skin.box);
        }


        /// <summary>
        /// グラフの描画を行う
        /// </summary>
        /// <param name="rect">描画範囲</param>
        /// <param name="content">グラフのタイトル</param>
        /// <param name="srcs">プロットするデータのリスト</param>
        /// <param name="index">プロットする先頭のインデックス</param>
        /// <param name="color">バーの色</param>
        /// <param name="select">0以上:選択されたバー -1:選択バーを表示しない -2:positionを使用</param>
        /// <param name="position">選択位置</param>
        /// <param name="selectColor">選択されたバーの色</param>
        /// <param name="barWidth">バーの幅</param>
        /// <param name="backColor">背景の色</param>
        /// <param name="isEnabledAdditionalLine">補助線を引く引くか否か</param>
        /// <param name="additionalLineColor"></param>
        /// <param name="scale">0以下:表示領域の高さに合わせて自動スケール その他:グラフをスケールする倍率/param>
        /// <param name="style"></param>
        /// <returns>内部で使用されたスケール倍率</returns>
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
            for (var i = 0; i < len; i++)
            {
                list.Add(srcs[i + ofst]);
            }

            // 背景            
            UnityEditor.EditorGUI.DrawRect(rect, backColor);
            
            
            if (list.Count != 0)
            {
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

             

                // グラフを描画
                for (var i = 0; i < list.Count; i++)
                {
                    var w = barWidth;
                    var h = list[i] * scale;
                    var x = rect.x + rect.width - len * w + i * w;
                    var y = rect.y + rect.height - h;
                    var r = new Rect(x, y, w, h);

                    if((select == -2) && r.Contains(position))
                    {
                        select = i;
                    }
                    if(i == select)
                    {
                        UnityEditor.EditorGUI.DrawRect(r, selectColor);
                    }
                    else
                    {
                        UnityEditor.EditorGUI.DrawRect(r, color);
                    }                    
                }

                if(isEnabledAdditionalLine){
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
                        DrawAdditionalLine(new Rect(x,y,w,h),realMin, additionalLineColor);
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
                    var contentSize = UnityEditor.EditorStyles.label.CalcSize(label);                    
                    var x = rect.x + rect.width - len + select * 1.0f - contentSize.x / 2;                    
                    var y = rect.y + rect.height - list[select] * scale - contentSize.y;
                    var w = contentSize.x;
                    var h = contentSize.y;

                    var r = new Rect(x, y, w, h);
                    UnityEditor.EditorGUI.DrawRect(r, new Color32(0,0,0,128));
                    UnityEditor.EditorGUI.LabelField(r, label);

                    var frame = ofst + select;
                    label = new GUIContent(Format("{0,3}",frame));
                    contentSize = UnityEditor.EditorStyles.label.CalcSize(label);
                    x = rect.x + rect.width - len + select * 1.0f - contentSize.x / 2;
                    y = rect.y + rect.height - contentSize.y;
                    w = contentSize.x;
                    h = contentSize.y;
                    r = new Rect(x, y, w, h);                    
                    UnityEditor.EditorGUI.DrawRect(r, new Color32(0, 0, 0, 128));
                    UnityEditor.EditorGUI.LabelField(r,label);
                }
            }

            // タイトル
            // 隠れないように最後に描画する
            {
                var contentSize = UnityEditor.EditorStyles.label.CalcSize(content);
                UnityEditor.EditorGUI.LabelField(new Rect(rect.x, rect.y, contentSize.x, contentSize.y), content);
            }

            return scale;
        }


        /// <summary>
        /// 補助線を引く
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="value"></param>
        /// <param name="color"></param>
        static void DrawAdditionalLine(Rect rect,float value,Color color)
        {
            UnityEditor.EditorGUI.DrawRect(rect,color);
            var label = new GUIContent(Format("{0,3:F1}", value));
            var contentSize = UnityEditor.EditorStyles.label.CalcSize(label);
            var rect2 = new Rect(rect.x, rect.y - contentSize.y / 2, contentSize.x, contentSize.y);
            UnityEditor.EditorGUI.DrawRect(rect2, Color.black);
            UnityEditor.EditorGUI.LabelField(rect2, label);
        }

        


        static string Format(string fmt, params object[] args)
        {
            return System.String.Format(CultureInfo.InvariantCulture.NumberFormat, fmt, args);

        }
    }
}