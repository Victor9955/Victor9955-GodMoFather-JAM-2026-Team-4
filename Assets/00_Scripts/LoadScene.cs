using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load (int index)
    {
        SceneManager.LoadScene(index);
    }
}
