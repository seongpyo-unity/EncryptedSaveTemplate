using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// 게임 시작 시 저장 데이터를 불러오고 로딩 화면을 처리하는 부트 매니저 클래스
/// </summary>
public class BootManager : SingletonMonobehaviour<BootManager>
{
    [SerializeField] private GameObject loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingBar;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(LoadGameRoutine());
    }

    /// <summary>
    /// 세이브 데이터를 불러오고 게임 씬으로 전환하는 로딩 루틴 코루틴 함수
    /// </summary>
    private IEnumerator LoadGameRoutine()
    {
        loadingText.text = "Loading...";
        loadingBar.fillAmount = 0f;

        yield return new WaitForSeconds(0.3f);

        int slot = PlayerPrefs.GetInt(Settings.selectSlotHashKey, 0);
        bool success = GameSaveSystem.TryLoadGame<GameData>(out var loadedData, slot);

        if (!success)
        {
            loadingText.text = "Load Failed. Returning...";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(Settings.StartMenu);
            yield break;
        }

        GameDataManager.Instance.Overwrite(loadedData);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Settings.GameScene);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingBar.fillAmount = progress;
            loadingText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";
            yield return null;
        }

        loadingBar.fillAmount = 1f;
        loadingText.text = "Loading Complete!";
        loadingSlider.SetActive(false);

        yield return new WaitForSeconds(0.4f);

        asyncLoad.allowSceneActivation = true;
    }
}