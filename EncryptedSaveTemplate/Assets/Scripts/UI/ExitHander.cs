using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHander : MonoBehaviour
{
    /// <summary>
    /// ������ �����ϴ� �Լ� (�����Ϳ� ���� ȯ�� ��� ����)
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �����Ϳ����� ���� ����
#else
        Application.Quit(); // ���� ȯ�濡���� ����
#endif
        Debug.Log("[ExitHandler] ���� ���� ��û��");
    }

    /// <summary>
    /// ���� �޴� ������ ���ư��� �Լ�
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(Settings.StartMenu);
    }
}
