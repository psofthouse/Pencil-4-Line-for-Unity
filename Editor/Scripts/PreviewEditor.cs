using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Pencil_4;

namespace Pcl4Editor
{
    public abstract class PreviewEditor : Editor
    {
        static private BrushDetailPreview.PreviewType _previewType;

        internal abstract BrushDetailNode previewNode { get; }
        internal virtual BrushSettingsNode optionalSettingsNode { get { return null; } }

        protected virtual void OnEnable()
        {
            _previewForBrush = new Texture2D(128, 128, TextureFormat.ARGB32, false, false);
            _previewForStroke = new Texture2D(4 * 96, 96, TextureFormat.ARGB32, false, false);
            _cacheInfo = new BrushDetailPreview.CacheInfomation();
        }

        protected virtual void OnDisable()
        {
            DestroyImmediate(_previewForBrush);
            _previewForBrush = null;
            DestroyImmediate(_previewForStroke);
            _previewForStroke = null;
        }
        
        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override GUIContent GetPreviewTitle()
        {
            return null;
        }

        public override void OnPreviewSettings()
        {
            _previewType = (BrushDetailPreview.PreviewType)EditorGUILayout.EnumPopup("", _previewType, GUILayout.Width(80) );
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            var tex = _previewType == BrushDetailPreview.PreviewType.Brush ? _previewForBrush : _previewForStroke;

            var center = r.center;
            r.width = Mathf.Min(r.width, tex.width);
            r.height = Mathf.Min(r.height, tex.height);
            r.center = center;

            var settingsNode = optionalSettingsNode;

            if (BrushDetailPreview.ValidateCacheInfomationAndUpdateBrushPreview(
                _cacheInfo, tex, previewNode, _previewType,
                settingsNode ? settingsNode.Size : 0.1f * tex.height,
                Color.black,
                Color.white
            ))
            {
                GUI.DrawTexture(r, tex, ScaleMode.ScaleToFit);
            }
        }

        private Texture2D _previewForBrush;
        private Texture2D _previewForStroke;
        private BrushDetailPreview.CacheInfomation _cacheInfo;
    }
}