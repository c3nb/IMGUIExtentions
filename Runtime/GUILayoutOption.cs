using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UTJ
{
    // Katsumasa.Kimura
    internal class GUILayoutOption
    {
        internal enum Type
        {
            fixedWidth, fixedHeight, minWidth, maxWidth, minHeight, maxHeight, stretchWidth, stretchHeight,

            alignStart, alignMiddle, alignEnd, alignJustify, equalSize, spacing
        }

        internal Type type;

        internal object value;

        internal GUILayoutOption(Type type, object value)
        {
            this.type = type;
            this.value = value;
        }


        internal GUILayoutOption(UnityEngine.GUILayoutOption option)
        {
            var st = option.GetType();

            var typeField = st.GetField("type", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            type = (Type)typeField.GetValue(option);

            var valueField = st.GetField("value", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            value = valueField.GetValue(option);
        }
    }
}