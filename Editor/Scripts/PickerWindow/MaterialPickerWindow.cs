using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

#if UNITY_6000_2_OR_NEWER
using TreeViewState = UnityEditor.IMGUI.Controls.TreeViewState<int>;
#endif

namespace Pcl4Editor
{
    public class MaterialPickerWindow : PickerWindow<Material>
    {

        static MaterialPickerWindow pickerWindow;

        public static void Open(IEnumerable<Material> materialsToExclude, Action<List<Material>> onAddButtonPushed)
        {
            if (pickerWindow == null)
            {
                pickerWindow = CreateInstance<MaterialPickerWindow>();
                pickerWindow.titleContent = new GUIContent("Select Material");
            }
            pickerWindow.OnAddButtonPushed = onAddButtonPushed;
            pickerWindow.treeView = new PickerTreeView<Material>(
                new TreeViewState(),
                () => EnumerateMaterials().Where(x => !materialsToExclude.Contains(x)),
                EditorGUIUtility.IconContent("Material Icon").image as Texture2D);
            pickerWindow.treeView.ItemDoubleClicked = (x) =>
            {
                onAddButtonPushed(new List<Material> { x });
                pickerWindow.Close();
            };
            pickerWindow.treeView.Reload();
            pickerWindow.ShowAuxWindow();
        }


        static IEnumerable<Material> EnumerateMaterials()
        {
            return Resources.FindObjectsOfTypeAll<Material>()
                .Where(x => x.hideFlags == HideFlags.None || x.hideFlags == HideFlags.NotEditable)
                .Where(x => !x.name.StartsWith("Hidden/"));
            // Hidden/以下のマテリアルが混入するので、差し当たりの対応
        }
    }
}
