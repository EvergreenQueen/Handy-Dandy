using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerControls player;
    public Image screen;
    private static MainMenu instance = null;
    private bool started = false;
    private bool sceneSwitching = false;
    void Awake()
    {
        // Enforce the singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Color temp = screen.color;
        temp.a = 0.0f;
        screen.color = temp;

        // Start detectItemGrab coroutine only once
        if (!started)
        {
            StartCoroutine(detectItemGrab());
        }
    }

    IEnumerator detectItemGrab()
    {
        Debug.Log("DetectItemGrab coroutine started");

        while (!started)
        {
            if ((player.amountOfItemsHeldLeft == 1 || player.amountOfItemsHeldRight == 1) && !started)
            {
                started = true;
                Debug.Log("Item detected, starting fade and scene switch coroutines");
                StartCoroutine(switchScene());
                yield break; // Exit the coroutine to prevent further checks
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator switchScene()
    {
        yield return new WaitForSeconds(2);
        // fade to black
        float fadeDuration = 1.0f;
        float elapsedTime = 0.0f;
        Color temp = screen.color;

        while (elapsedTime < fadeDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            temp.a = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeDuration);
            screen.color = temp;
        }

        temp.a = 1.0f;
        screen.color = temp;

        // switch scene
        Destroy(player.GetComponent<PlayerControls>());
        if (sceneSwitching)
        {
            Debug.Log("Scene switch already in progress, exiting coroutine");
            yield break;
        }
        sceneSwitching = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("AppleOrchard");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("MainMenu");

        player = null;
        Destroy(player);
    }
}
