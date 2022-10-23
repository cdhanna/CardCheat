using BrewedInk.CardCheat.Core;
using BrewedInk.CardCheat.Data;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GameData))]
public class GameDataEditor : Editor
{

    private static int width, height;

    private static bool inspecting = false;
    
    public override void OnInspectorGUI()
    {
        var data = target as GameData;
        inspecting = EditorGUILayout.Foldout(inspecting, "Editor");

        if (!inspecting)
        {
            base.OnInspectorGUI();
            return;
        }

        EditorGUI.indentLevel++;
        width = EditorGUILayout.IntField("width", width);
        height = EditorGUILayout.IntField("height", height);

        var isCreate = GUILayout.Button("Create...");
        var placeCards = GUILayout.Button("Shuffle...");
        //
        // if (isCreate)
        // {
        //     Undo.RecordObject(serializedObject.targetObject, "Setting width and height");
        //     data.internalGameData.board = new Board();
        //     for (var x = 0; x < width; x++)
        //     {
        //         for (var y = 0; y < height; y++)
        //         {
        //             var cell = new Cell { position = new Vector2Int(x, y) };
        //             data.internalGameData.board.cells.Add(cell);
        //         }
        //     }
        // }
        //
        // if (placeCards)
        // {
        //     Undo.RecordObject(serializedObject.targetObject, "Setting random cards");
        //
        //
        //     data.internalGameData.enemies.Clear();
        //     foreach (var cell in data.internalGameData.board.cells)
        //     {
        //         if (cell.position == data.internalGameData.player.position) continue;
        //
        //         data.internalGameData.enemies.Add(new GameCardEnemy
        //         {
        //             position = cell.position,
        //             data = new CardEnemy
        //             {
        //                 card = new Card
        //                 {
        //                     suit = CardExtensions.Random<CardSuit>(),
        //                     value = CardExtensions.RandomValue
        //                 }
        //             }
        //         });
        //     }
        //     
        // }
        //
        
        EditorGUI.indentLevel--;
        EditorGUILayout.LabelField("Base Properties...");
        base.OnInspectorGUI();
    }

    // public override VisualElement CreateInspectorGUI()
    // {
    // // Each editor window contains a root VisualElement object
    //     VisualElement root = new VisualElement();
    //
    //     // VisualElements objects can contain other VisualElement following a tree hierarchy.
    //     VisualElement label = new Label("Hello World! From C#");
    //     root.Add(label);
    //
    //     // Import UXML
    //     var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Data/Editor/GameDataEditor.uxml");
    //     VisualElement labelFromUXML = visualTree.Instantiate();
    //     root.Add(labelFromUXML);
    //     //
    //     // var container = root.Q<IMGUIContainer>();
    //     //
    //     // Debug.Log("Creating editor...");
    //     //
    //     //
    //     // var editor = Editor.CreateEditor(target);
    //
    //     //editor.CreateInspectorGUI();
    //     
    //     
    //     return root;
    // }

    // [MenuItem("Window/UI Toolkit/GameDataEditor")]
    // public static void ShowExample()
    // {
    //     GameDataEditor wnd = GetWindow<GameDataEditor>();
    //     wnd.titleContent = new GUIContent("GameDataEditor");
    // }
    //
    // public void CreateGUI()
    // {
    //     // Each editor window contains a root VisualElement object
    //     VisualElement root = rootVisualElement;
    //
    //     // VisualElements objects can contain other VisualElement following a tree hierarchy.
    //     VisualElement label = new Label("Hello World! From C#");
    //     root.Add(label);
    //
    //     // Import UXML
    //     var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Data/Editor/GameDataEditor.uxml");
    //     VisualElement labelFromUXML = visualTree.Instantiate();
    //     root.Add(labelFromUXML);
    //
    //     // A stylesheet can be added to a VisualElement.
    //     // The style will be applied to the VisualElement and all of its children.
    //     var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Data/Editor/GameDataEditor.uss");
    //     VisualElement labelWithStyle = new Label("Hello World! With Style");
    //     labelWithStyle.styleSheets.Add(styleSheet);
    //     root.Add(labelWithStyle);
    // }
}