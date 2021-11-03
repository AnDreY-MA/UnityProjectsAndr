using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderLevel : MonoBehaviour
{
    [SerializeField] private int iLevelToLoad;
    [SerializeField] private string sLevelToLoad;

    [SerializeField] private bool useIntToLoadLevel = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
            LoadScene(); 
    }

    private void LoadScene()
    {
        if (useIntToLoadLevel)
            SceneManager.LoadScene(iLevelToLoad);
        else
            SceneManager.LoadScene(sLevelToLoad);
    }
}
