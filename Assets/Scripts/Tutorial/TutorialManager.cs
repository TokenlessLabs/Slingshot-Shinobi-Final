using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject joystickbg;
    private Joystick joystick;
    public GameObject arrowObject;
    private RectTransform arrow;
    public GameObject Panel;
    private RectTransform panelRect;
    private TutorialMovement playerMovement;
    public Text instructionText;
    public GameObject Dashbar;
    public int currStep = 0;
    private Coroutine currentCoroutine;
    public GameObject enemy;
    private GameObject[] clones = new GameObject[3];
    public GameObject player;
    public GameObject poweruppanel;

    private const string TutorialShownKey = "TutorialShown";

    void Start()
    {

        if (PlayerPrefs.GetInt(TutorialShownKey, 0) == 0)
        {
            // Tutorial hasn't been shown, proceed with tutorial
            arrow = arrowObject.GetComponent<RectTransform>();
            panelRect = Panel.GetComponent<RectTransform>();
            joystick = joystickbg.GetComponent<Joystick>();
            playerMovement = player.GetComponent<TutorialMovement>();
            ShowJoystickArrow();
            LockJoystick(false); // Unlock joystick to allow movement
            currentCoroutine = StartCoroutine(PlayerMoving());
        }
        else
        {
            // Tutorial has been shown, skip to the main scene
            Debug.Log("TutorialShown ");
            LoadNextScene();
        }
    }

    void ShowJoystickArrow()
    {
        arrow.anchoredPosition = new Vector3(-30, -310, 0);
        arrow.localScale = new Vector3(-1, 1, 1);
        panelRect.anchoredPosition = new Vector3(0, -300, 0);
        StartCoroutine(TypeText("Use the joystick to move your character"));
    }

    void ShowHealthBarArrow()
    {
        StartCoroutine(FadeArrow(new Vector3(30, -70, 0), new Vector3(1, -1, 1)));
        StartCoroutine(TypeText("This is your health bar. Keep an eye on it!"));
    }

    void ShowDashBarArrow()
    {
        StartCoroutine(FadePanel(new Vector3(0, -650, 0), TypeText("This is your dash cooldown bar. Once it fills up you can perform a dash")));
        StartCoroutine(FadeArrow(new Vector3(-30, 70, 0), new Vector3(-1, 1, 1)));
    }

    void ShowEnemyPrefabs()
    {
        StartCoroutine(FadePanel(new Vector3(0, -300, 0), TypeText("These enemies seem like good practice to learn the dash on")));
        Animator animator = arrowObject.GetComponent<Animator>();
        animator.SetTrigger("Out");
    }

    void ShowShurikens()
    {
        StartCoroutine(FadePanel(new Vector3(0, -650, 0), TypeText("Killed enemies drop collectable shurikens like so")));
        arrow.anchoredPosition = new Vector3(-49, 250, 0);
        Animator animator = arrowObject.GetComponent<Animator>();
        animator.SetTrigger("In");
    }

    void ShowCollectionBarArrow()
    {
        StartCoroutine(FadeArrow(new Vector3(-30, 280, 0), new Vector3(-1, -1, 1)));
        StartCoroutine(TypeText("Collected shurikens help fill up this bar"));
    }

    void ShowPowerupPanelArrow()
    {
        StartCoroutine(FadePanel(new Vector3(0, -830, 0), TypeText("The panel provides many choices of powerful routes. So choose wisely!")));
        StartCoroutine(FadeArrow(new Vector3(-30, 260, 0), new Vector3(-1, 1, 1)));
    }

    void ShowEndArrow()
    {
        StartCoroutine(FadePanel(new Vector3(0, -300, 0), TypeText("That's it for the basics. Good luck player!")));
        Animator animator = arrowObject.GetComponent<Animator>();
        animator.SetTrigger("Out");
    }

    void LockJoystick(bool locked)
    {
        playerMovement.SetJoystickLock(locked);
    }

    void Update()
    {
        if (currentCoroutine == null)
        {
            if (currStep == 1)
            {
                currentCoroutine = StartCoroutine(ShowHealthArrow());
            }
            else if (currStep == 2)
            {
                currentCoroutine = StartCoroutine(ShowDashArrow());
            }
            else if (currStep == 3)
            {
                currentCoroutine = StartCoroutine(ShowEnemies());
            }
            else if (currStep == 4)
            {
                currentCoroutine = StartCoroutine(ShowDash());
            }
            else if (currStep == 5)
            {
                currentCoroutine = StartCoroutine(ShowShuriken());
            }
            else if (currStep == 6)
            {
                currentCoroutine = StartCoroutine(ShowCollectionBar());
            }
            else if (currStep == 7)
            {
                currentCoroutine = StartCoroutine(ShowCollectionBar2());
            }
            else if (currStep == 8)
            {
                currentCoroutine = StartCoroutine(ShowPowerupPanel());
            }
            else if (currStep == 9)
            {
                currentCoroutine = StartCoroutine(ShowEnd());
            }
        }
    }

    IEnumerator PlayerMoving()
    {
        while (playerMovement.transform.position == Vector3.zero) yield return null;
        while (joystick.InputVector != Vector2.zero) yield return null;
        yield return new WaitForSeconds(0.5f);
        LockJoystick(true);
        currStep++;
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowHealthArrow()
    {
        ShowHealthBarArrow();
        while (!IsTouchDetected()) yield return null;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowDashArrow()
    {
        ShowDashBarArrow();
        StartCoroutine(FillDashBar());
        while (!IsTouchDetected()) yield return null;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowEnemies()
    {
        ShowEnemyPrefabs();
        StartCoroutine(SpawnEnemies());
        yield return new WaitForSeconds(4f);
        while (!IsTouchDetected()) yield return null;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowDash()
    {
        StartCoroutine(TypeText("Swipe in the direction of the enemies to dash and eliminate them"));
        TutorialDash dash = player.GetComponentInChildren<TutorialDash>();
        dash.enabled = true;
        Image dashbar = Dashbar.GetComponent<Image>();
        dashbar.fillAmount = 1.0f;
        while (clones[1]) yield return null;
        dash.enabled = false;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowShuriken()
    {
        ShowShurikens();
        while (!IsTouchDetected()) yield return null;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowCollectionBar()
    {
        ShowCollectionBarArrow();
        GameObject[] shurikens = GameObject.FindGameObjectsWithTag("Shuriken");
        foreach (GameObject shuriken in shurikens)
        {
            Shuriken script = shuriken.GetComponent<Shuriken>();
            Collider2D collider = player.GetComponent<Collider2D>();
            script.ForcedAbsorb(collider);
        }
        TutorialPlayerCollecting collecting = player.GetComponent<TutorialPlayerCollecting>();
        collecting.CollectShuriken();
        collecting.CollectShuriken();
        while (!IsTouchDetected()) yield return null;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowCollectionBar2()
    {
        StartCoroutine(TypeText("Filling up this bar presents the player with a choice to power"));
        while (!IsTouchDetected()) yield return null;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
        TutorialPlayerCollecting collecting = player.GetComponent<TutorialPlayerCollecting>();
        collecting.CollectShuriken();
        collecting.CollectShuriken();
    }

    IEnumerator ShowPowerupPanel()
    {
        ShowPowerupPanelArrow();
        while (currStep == 8) yield return null;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
    }

    IEnumerator ShowEnd()
    {
        ShowEndArrow();
        while (!IsTouchDetected()) yield return null;
        currStep++;
        StopAllCoroutines();
        Text text = Panel.GetComponentInChildren<Text>();
        text.text = "";
        currentCoroutine = null; // Mark coroutine as finished
        Panel.SetActive(false);
        PlayerPrefs.SetInt(TutorialShownKey, 1); // Mark tutorial as shown
        LoadNextScene(); // Load the next scene
    }

    IEnumerator FillDashBar()
    {
        Image dashBarImage = Dashbar.GetComponent<Image>();
        float duration = 5f; // Duration in seconds
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float fillAmount = elapsed / duration;
            dashBarImage.fillAmount = fillAmount;
            yield return null;
        }

        dashBarImage.fillAmount = 1.0f; // Ensure it is fully filled at the end
    }

    IEnumerator TypeText(string message)
    {
        instructionText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            instructionText.text += letter;
            yield return new WaitForSeconds(0.06f); // Adjust typing speed here
        }
    }

    bool IsTouchDetected()
    {
        // Check for touch on mobile devices
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                return true;
            }
        }
        // Check for mouse click (for testing in the editor or on desktop)
        else if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }

    IEnumerator FadeArrow(Vector3 pos, Vector3 scale)
    {
        Animator animator = arrowObject.GetComponent<Animator>();
        animator.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        arrow.anchoredPosition = pos;
        arrow.localScale = scale;
        animator.SetTrigger("In");
    }

    IEnumerator FadePanel(Vector3 pos, IEnumerator next)
    {
        Animator animator = Panel.GetComponent<Animator>();
        animator.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        panelRect.anchoredPosition = pos;
        animator.SetTrigger("In");
        yield return new WaitForSeconds(1);
        StartCoroutine(next);
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 2; i++)
        {
            clones[i] = Instantiate(enemy, new Vector3(playerMovement.transform.position.x, playerMovement.transform.position.y + (-(i + 1) * 4), 0), Quaternion.identity);
            EnemyMovement movement = clones[i].GetComponent<EnemyMovement>();
            movement.enabled = false;
            EnemyAttack attack = clones[i].GetComponent<EnemyAttack>();
            attack.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes to load.");
        }
    }
}
