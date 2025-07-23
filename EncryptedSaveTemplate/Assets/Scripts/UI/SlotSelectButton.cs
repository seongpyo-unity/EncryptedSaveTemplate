using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotSelectButton : MonoBehaviour
{
    [SerializeField] private int slotIndex = 0;

    public void OnClick()
    {
        PlayerPrefs.SetInt("SelectedSaveSlot", slotIndex);
        PlayerPrefs.Save();

        Debug.Log($"[MainMenu] ΩΩ∑‘ {slotIndex} º±≈√µ . BootScene¿∏∑Œ ¿Ãµø.");
        SceneManager.LoadScene(Settings.BootSceneName);
    }
}