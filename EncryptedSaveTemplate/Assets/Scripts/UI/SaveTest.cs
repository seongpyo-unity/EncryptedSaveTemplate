using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text saveText;

    private void Awake()
    {
        UpdateSave();
    }

    public void OnSubmit()
    {
        string userInput = inputField.text;

        GameData gameData = new GameData();

        gameData.saveTest = inputField.text;

        GameDataManager.Instance.Overwrite(gameData);

        UpdateSave();

        inputField.text = "";
    }

    public void UpdateSave()
    {
        saveText.text = $"Save Data : {GameDataManager.Instance.CurrentData.saveTest}";
    }
}
