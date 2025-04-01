using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoTweenPopup))]
[CanEditMultipleObjects]
public class DoTweenPopupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty showType = serializedObject.FindProperty("popupShowType");
        EditorGUILayout.PropertyField(showType);
        DoTweenPopup popup = (DoTweenPopup)target;
        switch (showType.enumValueFlag)
        {
            case (int)PopupShowOptions.TRANSITION:
                {
                    SetStyle("Transition", "- Set Start Point : Set điểm bắt đầu khi mở popup và kết thúc khi đóng popup, " +
                        "cần kéo content ra điểm bạn muốn bắt đầu và Click vào 'Set Start Point', sau đó bạn cần kéo content về vị trí trước khi set", true);

                    if (GUILayout.Button("Set Start Point ( " + popup.startPoint.x + " , " + popup.startPoint.y + " )", GUILayout.Height(40)))
                    {
                        popup.SetStartPoint();
                    }
                }
                break;
            case (int)PopupShowOptions.SCALE_AND_FADE:
                {
                    SetStyle("Scale and fade", "", true);
                    SerializedProperty startScale = serializedObject.FindProperty("startScale");
                    if (GUILayout.Button("Set Start Scale ( " + popup.startScale.x + "," + popup.startScale.y + " )", GUILayout.Height(40)))
                    {
                        popup.SetStartScale();
                    }
                    SerializedProperty startFade = serializedObject.FindProperty("startFade");
                    startFade.floatValue = EditorGUILayout.Slider("Start Fade", startFade.floatValue, 0, 1);
                    SerializedProperty durationFade = serializedObject.FindProperty("durationFade");
                    durationFade.floatValue = EditorGUILayout.Slider("Duration Fade", durationFade.floatValue, 0, 1);
                }
                break;
            case (int)PopupShowOptions.SCALE:
                {
                    SetStyle("Scale", "Lười viết tiếp =))", true);
                    SerializedProperty startScale = serializedObject.FindProperty("startScale");
                    if (GUILayout.Button("Set Start Scale ( " + popup.startScale.x + "," + popup.startScale.y + " )", GUILayout.Height(40)))
                    {
                        popup.SetStartScale();
                    }
                }
                break;
            case (int)PopupShowOptions.FADE:
                {
                    SetStyle("Fade", "Lười viết tiếp =))", true);
                    SerializedProperty startScale = serializedObject.FindProperty("startFade");
                    startScale.floatValue = EditorGUILayout.Slider("Start Fade", startScale.floatValue, 0, 1);
                }
                break;
            case (int)PopupShowOptions.ROTATE:
                {
                    SetStyle("Rotate", "Lười viết tiếp =))", true);
                    SerializedProperty startRotate = serializedObject.FindProperty("startRotate");
                    if (GUILayout.Button("Set Start Rotate ( " + popup.startRotate.x + "," + popup.startRotate.y + "," + popup.startRotate.z + " )", GUILayout.Height(40)))
                    {
                        popup.SetStartRotate();
                    }
                }
                break;
            case (int)PopupShowOptions.COLOR:
                {
                    SetStyle("Color", "Lười viết tiếp =))", true);
                    SerializedProperty startColor = serializedObject.FindProperty("startColor");
                    startColor.colorValue = EditorGUILayout.ColorField("Set Color Start", startColor.colorValue);
                }
                break;
            case (int)PopupShowOptions.SCALE_TRANSITION:
                {
                    if (GUILayout.Button("Hướng dẫn", GUILayout.Height(40)))
                    {
                        string baseHelp = "-Delay : Delay mở và đóng popup" +
                            "\n------------------------------\n- Duration : Thời gian thực hiện chuyển động " +
                            "\n------------------------------\n- Easing : Các dạng chuyển động, xem thêm ở https://easings.net/vi \n------------------------------\n" +
                            "\n------------------------------\n- SCALE đồng thời TRANSITION : ";
                        EditorUtility.DisplayDialog("Hướng dẫn Scale Transition", baseHelp, "Đóng");
                    }
                    GUILayout.Space(10);

                    SetStyle("Scale");

                    SerializedProperty startScale = serializedObject.FindProperty("startScale");
                    GUILayout.Space(5);
                    startScale.vector2Value = EditorGUILayout.Vector2Field("Start Scale", startScale.vector2Value);
                    // if (GUILayout.Button("Set Start Scale ( " + popup.startScale.x + "," + popup.startScale.y + " )", GUILayout.Height(40)))
                    // {
                    //     popup.SetStartScale();
                    // }
                    GUILayout.Space(10);
                    // Transition
                    GUIStyle label = new GUIStyle(EditorStyles.textField);
                    label.normal.textColor = Color.green;
                    label.fontSize = 14;
                    label.fontStyle = FontStyle.Bold;
                    label.alignment = TextAnchor.UpperCenter;
                    EditorGUILayout.LabelField("Transition", label);
                    SerializedProperty delay = serializedObject.FindProperty("delay1");
                    delay.floatValue = EditorGUILayout.Slider("Delay", delay.floatValue, 0, 5);
                    SerializedProperty duration = serializedObject.FindProperty("duration1");
                    duration.floatValue = EditorGUILayout.Slider("Duration", duration.floatValue, 0, 3);
                    SerializedProperty open = serializedObject.FindProperty("open1");
                    open.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Open Style", (Ease)open.enumValueFlag);
                    SerializedProperty close = serializedObject.FindProperty("close1");
                    close.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Close Style", (Ease)close.enumValueFlag);
                    GUILayout.Space(5);
                    if (GUILayout.Button("Set Start Point ( " + popup.startPoint.x + " , " + popup.startPoint.y + " )", GUILayout.Height(40)))
                    {
                        popup.SetStartPoint();
                    }
                }
                break;
            case (int)PopupShowOptions.TRANS_SCALE_AND_FADE:
                {
                    if (GUILayout.Button("Hướng dẫn", GUILayout.Height(40)))
                    {
                        string baseHelp = "-Delay : Delay mở và đóng popup" +
                            "\n------------------------------\n- Duration : Thời gian thực hiện chuyển động " +
                            "\n------------------------------\n- Easing : Các dạng chuyển động, xem thêm ở https://easings.net/vi" +
                            "\n------------------------------\n- TRANSITION - SCALE - TRANSITION : Chạy xong transition rồi chạy đồng thời scale và transition" +
                             "\n------------------------------\n- Thư tự chạy từ trên xuống và phân biệt bằng màu sắc, cùng màu là chạy cùng nhau";
                        EditorUtility.DisplayDialog("Hướng dẫn Transition Scale Transition", baseHelp, "Đóng");
                    }
                    GUILayout.Space(10);
                    // Transition          
                    SetStyle("Transition chạy đầu tiên");
                    SerializedProperty startPoint = serializedObject.FindProperty("startPoint");
                    startPoint.vector2Value = EditorGUILayout.Vector2Field("Start Point", startPoint.vector2Value);
                    GUILayout.Space(10);
                    GUIStyle label = new GUIStyle(EditorStyles.textField);
                    label.normal.textColor = Color.yellow;
                    label.fontSize = 14;
                    label.fontStyle = FontStyle.Bold;
                    label.alignment = TextAnchor.UpperCenter;
                    // Scale
                    EditorGUILayout.LabelField("Scale chạy thứ 2", label);
                    SerializedProperty delay1 = serializedObject.FindProperty("delay1");
                    delay1.floatValue = EditorGUILayout.Slider("Delay", delay1.floatValue, 0, 5);
                    SerializedProperty duration1 = serializedObject.FindProperty("duration1");
                    duration1.floatValue = EditorGUILayout.Slider("Duration", duration1.floatValue, 0, 3);
                    SerializedProperty open1 = serializedObject.FindProperty("open1");
                    open1.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Open Style", (Ease)open1.enumValueFlag);
                    SerializedProperty close1 = serializedObject.FindProperty("close1");
                    close1.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Close Style", (Ease)close1.enumValueFlag);
                    SerializedProperty startScale = serializedObject.FindProperty("startScale");
                    GUILayout.Space(5);
                    startScale.vector2Value = EditorGUILayout.Vector2Field("Start Scale", startScale.vector2Value);
                    GUILayout.Space(10);
                    // Transition
                    EditorGUILayout.LabelField("Transition chạy cùng scale", label);
                    SerializedProperty delay2 = serializedObject.FindProperty("delay2");
                    delay2.floatValue = EditorGUILayout.Slider("Delay", delay2.floatValue, 0, 5);
                    SerializedProperty duration2 = serializedObject.FindProperty("duration2");
                    duration2.floatValue = EditorGUILayout.Slider("Duration", duration2.floatValue, 0, 3);
                    SerializedProperty open2 = serializedObject.FindProperty("open2");
                    open2.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Open Style", (Ease)open2.enumValueFlag);
                    SerializedProperty close2 = serializedObject.FindProperty("close2");
                    close2.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Close Style", (Ease)close2.enumValueFlag);
                    GUILayout.Space(5);
                    SerializedProperty startPoint1 = serializedObject.FindProperty("startPoint1");
                    startPoint1.vector2Value = EditorGUILayout.Vector2Field("Start Point1", startPoint1.vector2Value);
                    
                    EditorGUILayout.LabelField("Fade chạy cùng", label);
                    SerializedProperty startFade = serializedObject.FindProperty("startFade");
                    startFade.floatValue = EditorGUILayout.Slider("Start Fade", startFade.floatValue, 0, 1);
                    SerializedProperty durationFade = serializedObject.FindProperty("durationFade");
                    durationFade.floatValue = EditorGUILayout.Slider("Duration Fade", durationFade.floatValue, 0, 1);
                }
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void SetStyle(string type, string help = "", bool showHelp = false)
    {
        if (showHelp)
        {
            if (GUILayout.Button("Hướng dẫn", GUILayout.Height(40)))
            {
                string baseHelp = "-Delay : Delay mở và đóng popup" +
                    "\n------------------------------\n- Duration : Thời gian thực hiện chuyển động " +
                    "\n------------------------------\n- Easing : Các dạng chuyển động, xem thêm ở https://easings.net/vi \n------------------------------\n";
                EditorUtility.DisplayDialog("Hướng dẫn " + type, baseHelp + help, "Đóng");
            }
            GUILayout.Space(10);
        }

        GUIStyle label = new GUIStyle(EditorStyles.textField);
        label.normal.textColor = Color.green;
        label.fontSize = 14;
        label.fontStyle = FontStyle.Bold;
        label.alignment = TextAnchor.UpperCenter;
        EditorGUILayout.LabelField(type, label);
        GUILayout.Space(5);
        SerializedProperty delay = serializedObject.FindProperty("delay");
        delay.floatValue = EditorGUILayout.Slider("Delay", delay.floatValue, 0, 5);
        SerializedProperty duration = serializedObject.FindProperty("duration");
        duration.floatValue = EditorGUILayout.Slider("Duration", duration.floatValue, 0, 3);
        SerializedProperty open = serializedObject.FindProperty("open");
        open.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Open Style", (Ease)open.enumValueFlag);
        SerializedProperty close = serializedObject.FindProperty("close");
        close.enumValueFlag = (int)(Ease)EditorGUILayout.EnumPopup("Close Style", (Ease)close.enumValueFlag);
        GUILayout.Space(5);
    }

}

