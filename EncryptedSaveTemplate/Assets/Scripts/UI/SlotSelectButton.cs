using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotSelectButton : MonoBehaviour
{
    [SerializeField] private int slotIndex = 0;
    public TMP_Text previewText;

    private void Awake()
    {
        LoadPreview();
    }

    public void OnClick()
    {
        PlayerPrefs.SetInt(Settings.selectSlotHashKey, slotIndex);
        PlayerPrefs.Save();

        Debug.Log($"[MainMenu] ΩΩ∑‘ {slotIndex} º±≈√µ . BootScene¿∏∑Œ ¿Ãµø.");
        SceneManager.LoadScene(Settings.BootSceneName);
    }

    public void LoadPreview()
    {
        if(GameSaveSystem.TryLoadPreview(out GameDataPreview data, slotIndex))
        {
            previewText.text = data.lastSaveTime.ToString();
        }
        else
        {
            previewText.text = "Empty";
        }
    }
}