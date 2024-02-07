using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private UIManager m_UIManager;

    private void Start() {
        m_UIManager = GameObject.Find("UI Canvas").GetComponent<UIManager>();
    }

    public void StartLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelProcess(levelIndex));
    }

    public IEnumerator LoadLevelProcess(int levelIndex)
    {
        m_UIManager.ToggleScreen("down");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(levelIndex);
    }
}
