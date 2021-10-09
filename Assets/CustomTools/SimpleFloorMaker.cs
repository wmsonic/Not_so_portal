using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SimpleFloorMaker : EditorWindow {

    private GameObject _originalItemToDuplicate;

    private string _baseName = "ex: Brick";
    private string _nameModelX = "ex: X";
    private string _nameModelY = "ex: Y";

    private bool _duplicateInRight;

    private bool _duplicateInLeft;

    private bool _duplicateInUp;

    private bool _duplicateInDown;

    private int _amountToDuplicateRight;

    private int _amountToDuplicateLeft;

    private int _amountToDuplicateUp;

    private int _amountToDuplicateDown;

    private Vector2 _sizeOfItem;

    private string[] _typesOfDuplication = new string[] { "Row and column only", "Complete grid" };
    private int _selectedDuplicationStyle = 0; // 0 == row & columns only ; 1 == Complete grid

    private Rect _windowRectangle = new Rect(500, 100, 200, 200);
    private bool _showPreview;


    [MenuItem("Custom Tools/SimpleFloorMaker")]
    private static void ShowWindow() {
        var window = GetWindow<SimpleFloorMaker>();
        window.titleContent = new GUIContent("SimpleFloorMaker");
        window.Show();
    }

    private void OnGUI() {

        minSize = new Vector2(420, 560);
        EditorStyles.helpBox.fontSize = 14;
        GUIStyle customLabelStyle = new GUIStyle();
        customLabelStyle.richText = true;
        // EditorGUI.BeginFoldoutHeaderGroup(new Rect(10,10,position.width,100),true,"Info", EditorStyles.helpBox);
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("This tool will duplicate a given GameObject in the given direction(s) the given amount of times", MessageType.None, true);
        EditorGUILayout.HelpBox("Note: The tool uses the SpriteRenderer of the object to determine the size of the item", MessageType.Warning, true);
        // EditorGUI.EndFoldoutHeaderGroup();
        // Makes a field in which we can give an object. We ask for the object in the label, we give "_originalItemToDuplicate" to store it, the object as to be of type GameObject, and it can be a scene object
        GUILayout.Space(10);
        _originalItemToDuplicate = (GameObject)EditorGUILayout.ObjectField("Object to duplicate:", _originalItemToDuplicate, typeof(GameObject), true);

        GUILayout.Space(20);
        GUILayout.Label("<size=13>Name model for duplicated objects:</size>", customLabelStyle);
        _baseName = EditorGUILayout.TextField("Base name of duplicates:", _baseName);
        _nameModelX = EditorGUILayout.TextField("Prefix for X coordinate:", _nameModelX); //to see if it's better to say "prefix for" or "model for"
        _nameModelY = EditorGUILayout.TextField("Prefix for Y coordinate:", _nameModelY);

        GUILayout.Space(20);
        GUILayout.Label("<size=13>Select direction(s) of duplication:</size>", customLabelStyle);
        _duplicateInRight = EditorGUILayout.Toggle("Duplicate in X+", _duplicateInRight);
        _amountToDuplicateRight = EditorGUILayout.IntField("Amount to duplicate in X+", _amountToDuplicateRight);
        GUILayout.Space(5);
        _duplicateInLeft = EditorGUILayout.Toggle("Duplicate in X-", _duplicateInLeft);
        _amountToDuplicateLeft = EditorGUILayout.IntField("Amount to duplicate in X-", _amountToDuplicateLeft);
        GUILayout.Space(5);
        _duplicateInUp = EditorGUILayout.Toggle("Duplicate in Y+", _duplicateInUp);
        _amountToDuplicateUp = EditorGUILayout.IntField("Amount to duplicate in Y+", _amountToDuplicateUp);
        GUILayout.Space(5);
        _duplicateInDown = EditorGUILayout.Toggle("Duplicate in Y-", _duplicateInDown);
        _amountToDuplicateDown = EditorGUILayout.IntField("Amount to duplicate in Y-", _amountToDuplicateDown);

        GUILayout.Space(20);
        GUILayout.Label("<size=13>Select type of duplication:</size>", customLabelStyle);
        _selectedDuplicationStyle = GUILayout.SelectionGrid(_selectedDuplicationStyle, _typesOfDuplication, 2, EditorStyles.radioButton); //radio that says if we only make the column and row or a complete grid

        //make preview panel
        // GUILayout.Space(20);
        // if(GUILayout.Button("Preview")){
        //     _showPreview = true;
        //     // BeginWindows();
        //     // _windowRectangle = GUILayout.Window(1,_windowRectangle,DrawPreviewWindow,"Preview:");
        //     // EndWindows();
        // }
        // if(_showPreview){
        //     DrawPreviewWindow();
        // }

        GUILayout.Space(20);
        if (GUILayout.Button("Duplicate")) {
            if (_originalItemToDuplicate == null) {
                EditorUtility.DisplayDialog("Could not start duplication", "A GameObject to duplicate has to be given in the object field for the duplication to start", "Close");
            } else if (_baseName == "ex: Brick" || _nameModelX == "ex: X" || _nameModelY == "ex: Y") {
                EditorUtility.DisplayDialog("Could not start duplication", "All the name model fields have to be filled for duplication to start", "Close");
            } else {
                MakeFloor();
            }
        }


    }

    private void DrawPreviewWindow() {
        // GUILayout.Button("Hi");
        if (GUILayout.Button("close preview")) {
            _showPreview = false;
        }
    }

    private void MakeFloor() {
        // _sizeOfItem = new Vector2(_originalItemToDuplicate.GetComponent<Collider2D>().bounds.size.x, _originalItemToDuplicate.GetComponent<Collider2D>().bounds.size.y);
        _sizeOfItem = new Vector2(_originalItemToDuplicate.GetComponent<SpriteRenderer>().bounds.size.x, _originalItemToDuplicate.GetComponent<SpriteRenderer>().bounds.size.y);
        if (_duplicateInRight && _duplicateInLeft && _duplicateInUp && _duplicateInDown) {
            DuplicateRight();
            DuplicateLeft();
            DuplicateUp();
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right, " + _amountToDuplicateLeft + " to the left, " + _amountToDuplicateUp + " upwards, " + _amountToDuplicateDown + " downwards");
        } else if (_duplicateInRight && _duplicateInLeft && _duplicateInUp) {
            DuplicateRight();
            DuplicateLeft();
            DuplicateUp();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right, " + _amountToDuplicateLeft + " to the left, " + _amountToDuplicateUp + " upwards");
        } else if (_duplicateInRight && _duplicateInLeft && _duplicateInDown) {
            DuplicateRight();
            DuplicateLeft();
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right, " + _amountToDuplicateLeft + " to the left, " + _amountToDuplicateDown + " downwards");
        } else if (_duplicateInRight && _duplicateInUp && _duplicateInDown) {
            DuplicateRight();
            DuplicateUp();
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right, " + _amountToDuplicateUp + " upwards, " + _amountToDuplicateDown + " downwards");
        } else if (_duplicateInLeft && _duplicateInUp && _duplicateInDown) {
            DuplicateLeft();
            DuplicateUp();
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateLeft + " " + _originalItemToDuplicate.name + " to the left, " + _amountToDuplicateUp + " upwards, " + _amountToDuplicateDown + " downwards");
        } else if (_duplicateInRight && _duplicateInLeft) {
            DuplicateRight();
            DuplicateLeft();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right, " + _amountToDuplicateLeft + " to the left");
        } else if (_duplicateInRight && _duplicateInUp) {
            DuplicateRight();
            DuplicateUp();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right, " + _amountToDuplicateUp + " upwards");
        } else if (_duplicateInRight && _duplicateInDown) {
            DuplicateRight();
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right, " + _amountToDuplicateDown + " downwards");
        } else if (_duplicateInLeft && _duplicateInUp) {
            DuplicateLeft();
            DuplicateUp();
            // Debug.Log("Duplicated " + _amountToDuplicateLeft + " " + _originalItemToDuplicate.name + " to the left, " + _amountToDuplicateUp + " upwards");
        } else if (_duplicateInLeft && _duplicateInDown) {
            DuplicateLeft();
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateLeft + " " + _originalItemToDuplicate.name + " to the left, " + _amountToDuplicateDown + " downwards");
        } else if (_duplicateInUp && _duplicateInDown) {
            DuplicateUp();
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateUp + " " + _originalItemToDuplicate.name + " upwards, " + _amountToDuplicateDown + " downwards");
        } else if (_duplicateInRight) {
            DuplicateRight();
            // Debug.Log("Duplicated " + _amountToDuplicateRight + " " + _originalItemToDuplicate.name + " to the right");
        } else if (_duplicateInLeft) {
            DuplicateLeft();
            // Debug.Log("Duplicated " + _amountToDuplicateLeft + " " + _originalItemToDuplicate.name + " to the left");
        } else if (_duplicateInUp) {
            DuplicateUp();
            // Debug.Log("Duplicated " + _amountToDuplicateUp + " " + _originalItemToDuplicate.name + " upwards");
        } else if (_duplicateInDown) {
            DuplicateDown();
            // Debug.Log("Duplicated " + _amountToDuplicateDown + " " + _originalItemToDuplicate.name + " downwards");
        }
    }
    private void DuplicateRight() {
        GameObject itemToDuplicate = _originalItemToDuplicate;
        for (int r = 0; r < _amountToDuplicateRight; r++) {
            itemToDuplicate = Instantiate(itemToDuplicate, new Vector3(itemToDuplicate.transform.position.x + _sizeOfItem.x, itemToDuplicate.transform.position.y, itemToDuplicate.transform.position.z), itemToDuplicate.transform.rotation);
            itemToDuplicate.name = _baseName + _nameModelX + (r + 1) + _nameModelY + "0";
        }
    }
    private void DuplicateLeft() {
        GameObject itemToDuplicate = _originalItemToDuplicate;
        for (int l = 0; l < _amountToDuplicateLeft; l++) {
            itemToDuplicate = Instantiate(itemToDuplicate, new Vector3(itemToDuplicate.transform.position.x - _sizeOfItem.x, itemToDuplicate.transform.position.y, itemToDuplicate.transform.position.z), itemToDuplicate.transform.rotation);
            itemToDuplicate.name = _baseName + _nameModelX + -(l + 1) + _nameModelY + "0";
        }
    }
    private void DuplicateUp() {
        GameObject itemToDuplicate = _originalItemToDuplicate;
        for (int u = 0; u < _amountToDuplicateUp; u++) {
            itemToDuplicate = Instantiate(itemToDuplicate, new Vector3(itemToDuplicate.transform.position.x, itemToDuplicate.transform.position.y + _sizeOfItem.y, itemToDuplicate.transform.position.z), itemToDuplicate.transform.rotation);
            itemToDuplicate.name = _baseName + _nameModelX + "0" + _nameModelY + (u + 1);
            if (_selectedDuplicationStyle == 1) {
                MakeGrid(itemToDuplicate, u);
            }
        }
    }
    private void DuplicateDown() {
        GameObject itemToDuplicate = _originalItemToDuplicate;
        for (int d = 0; d < _amountToDuplicateDown; d++) {
            itemToDuplicate = Instantiate(itemToDuplicate, new Vector3(itemToDuplicate.transform.position.x, itemToDuplicate.transform.position.y - _sizeOfItem.y, itemToDuplicate.transform.position.z), itemToDuplicate.transform.rotation);
            itemToDuplicate.name = _baseName + _nameModelX + "0" + _nameModelY + -(d + 1);
            if (_selectedDuplicationStyle == 1) {
                MakeGrid(itemToDuplicate, d);
            }
        }
    }

    private void MakeGrid(GameObject itemToDuplicate, int Ycoordinate) {
        GameObject itemToDuplicateCopy = itemToDuplicate;
        for (int r = 0; r < _amountToDuplicateRight; r++) {
            itemToDuplicateCopy = Instantiate(itemToDuplicateCopy, new Vector3(itemToDuplicateCopy.transform.position.x + _sizeOfItem.x, itemToDuplicateCopy.transform.position.y, itemToDuplicateCopy.transform.position.z), itemToDuplicate.transform.rotation);
            itemToDuplicateCopy.name = _baseName + _nameModelX + (r + 1) + _nameModelY + (Ycoordinate + 1);
        }
        itemToDuplicateCopy = itemToDuplicate;
        for (int l = 0; l < _amountToDuplicateLeft; l++) {
            itemToDuplicateCopy = Instantiate(itemToDuplicateCopy, new Vector3(itemToDuplicateCopy.transform.position.x - _sizeOfItem.x, itemToDuplicateCopy.transform.position.y, itemToDuplicateCopy.transform.position.z), itemToDuplicate.transform.rotation);
            itemToDuplicateCopy.name = _baseName + _nameModelX + -(l + 1) + _nameModelY + (Ycoordinate + 1);
        }
    }
}
