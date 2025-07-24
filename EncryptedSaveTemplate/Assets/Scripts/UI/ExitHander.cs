using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHander : MonoBehaviour
{
    /// <summary>
    /// 게임을 종료하는 함수 (에디터와 빌드 환경 모두 대응)
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서는 실행 중지
#else
        Application.Quit(); // 빌드 환경에서는 종료
#endif
        Debug.Log("[ExitHandler] 게임 종료 요청됨");
    }

    /// <summary>
    /// 메인 메뉴 씬으로 돌아가는 함수
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(Settings.StartMenu);
    }
}
