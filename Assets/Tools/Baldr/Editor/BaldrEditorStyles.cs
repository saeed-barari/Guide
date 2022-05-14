using UnityEditor;
using UnityEngine;

namespace Baldr.Editor
{
    public static class BaldrEditorStyles
    {
        private static GUIStyle _headerStyle;

        public static GUIStyle HeaderButton
        {
            get
            {
                if (_headerStyle is null)
                {
                    _headerStyle = new GUIStyle(EditorStyles.helpBox)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 14,
                        fontStyle = FontStyle.Bold,
                        stretchWidth = true
                    };
                    _headerStyle.focused.textColor = Color.blue;
                    _headerStyle.onFocused.textColor = Color.red;
                    
                        // = Texture2D.whiteTexture;
                    _headerStyle.onHover.background = Texture2D.redTexture;
                }

                return _headerStyle;
            }
        }
        private static GUIStyle _innerBox;

        public static GUIStyle InnerBox
        {
            get
            {
                if (_innerBox is null)
                {
                    _innerBox = new GUIStyle(GUI.skin.box)
                    {
                        padding = new RectOffset(10, 10, 3, 3),
                        stretchWidth = true
                    };
                }
                return _innerBox;
            }
        }
    }
}