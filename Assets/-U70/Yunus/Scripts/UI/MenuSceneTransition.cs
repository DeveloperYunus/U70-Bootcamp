using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneTransition : MonoBehaviour
{
    public static MenuSceneTransition ins;
    public float scenetransTime;

    private void Awake()
    {
        ins = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveX(0, 0);
        transform.DOMoveX(-3500, scenetransTime);
    }

    public void SceneFadeUp()
    {
        transform.DOMoveX(0, scenetransTime - 1f).OnComplete(()=>
        {
            SceneManager.LoadScene(1);
        });
    }
}
