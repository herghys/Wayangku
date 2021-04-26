using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataCreatorWindow : EditorWindow
{
    [SerializeField]    DataXML data = new DataXML();
    [SerializeField] string path = string.Empty;

    private Vector2 scrollPosition = Vector2.zero;

    private SerializedObject serializedObject = null;
    private SerializedProperty questionsProp = null;

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        data.Questions = new Question[0];
        questionsProp = serializedObject.FindProperty("data").FindPropertyRelative("Questions");
    }
    [MenuItem("Window/Game/Data Creator")]
    public static void OpenWindow()
    {
        var window = GetWindow<DataCreatorWindow>("XML Data Creator");

        window.minSize = new Vector2(510.0f, 344.0f);
        window.Show();
    }
    private void OnGUI()
    {
        #region Header
        Rect headerRect = new Rect(15, 15, position.width - 30, 65);
        GUI.Box(headerRect, GUIContent.none);

        GUIStyle headerStyle = new GUIStyle(EditorStyles.largeLabel)
        {
            fontSize = 26,
            alignment = TextAnchor.UpperLeft
        };
        headerRect.x += 5;
        headerRect.width -= 10;
        headerRect.y += 5;
        headerRect.height -= 10;
        GUI.Label(headerRect, "Data to XML Creator", headerStyle);

        //Summary Text
        Rect summaryRect = new Rect(headerRect.x + 5, (headerRect.y + headerRect.height) - 20, headerRect.width - 50, 15);
        GUI.Label(summaryRect, "Create the data that needs to be included into the XML file");
        #endregion

        #region Body Section
        Rect bodyRect = new Rect(15, (headerRect.y + headerRect.height) + 20, this.position.width - 30, this.position.height - (headerRect.y + headerRect.height) - 80);
        GUI.Box(bodyRect, GUIContent.none);

        var arraySize = data.Questions.Length;

        Rect viewRect = new Rect(bodyRect.x + 10, bodyRect.y + 10, bodyRect.width - 20, EditorGUI.GetPropertyHeight(questionsProp));
        Rect scrollPosRect = new Rect(viewRect)
        {
            height = bodyRect.height - 20
        };
        scrollPosition = GUI.BeginScrollView(scrollPosRect, scrollPosition, viewRect, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

        var drawSlider = viewRect.height > scrollPosRect.height;

        Rect propertyRect = new Rect(bodyRect.x + 10, bodyRect.y + 10, bodyRect.width - (drawSlider ? 40 : 20), 17);
        EditorGUI.PropertyField(propertyRect, questionsProp, true);

        serializedObject.ApplyModifiedProperties();

        GUI.EndScrollView();
        #endregion

        #region Navigation
        Rect buttonRect = new Rect(bodyRect.x + bodyRect.width - 85, bodyRect.y + bodyRect.height + 5, 85, 40);
        bool pressed = GUI.Button(buttonRect, string.IsNullOrEmpty(path) ? "Create" : "Save", EditorStyles.miniButtonRight);
        if (pressed)
        {
            if (string.IsNullOrEmpty(path) == true)
            {
                path = EditorUtility.SaveFilePanel("Save", Application.dataPath + "/Questions/", GameData.xmlFile ,"xml");
            }
            if (string.IsNullOrEmpty(path) == false)
            {
                DataXML.Write(data, path);
            }
            AssetDatabase.Refresh();
        }

        buttonRect.x -= buttonRect.width;
        pressed = GUI.Button(buttonRect, "Fetch", EditorStyles.miniButtonLeft);
        if (pressed)
        {
            path = EditorUtility.OpenFilePanel("Select", Application.dataPath + "/Questions/", "xml");
            if (!string.IsNullOrEmpty(path))
            {
                var d = DataXML.Fetch(out bool result, path);
                if (result)
                {
                    data = d;
                }
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }
        buttonRect.x = bodyRect.x;
        pressed = GUI.Button(buttonRect, "New", EditorStyles.miniButton);
        if (pressed)
        {
            path = string.Empty;
            data = new DataXML
            {
                Questions = new Question[0]
            };
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
        #endregion

    }
}