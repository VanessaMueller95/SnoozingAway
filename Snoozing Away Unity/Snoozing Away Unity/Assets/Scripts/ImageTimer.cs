using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{

    //Zum Aktivieren oder Deaktivieren
    public bool timerActive = false;

    //Übergabe des MenüObjekts, damit es nach Wunsch aktiviert werden kann
    public GameObject restartMenuUI;

    Image fillImg;
    public float timeAmt = 50;
    public float time;

    Material skybox_Material;

    static float t = 0.0f;

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
        skybox_Material = RenderSettings.skybox;
        skybox_Material.SetFloat("_Blend", 0);
        t = 0;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(time);
        Debug.Log(timeAmt);
        if (time > 0 && timerActive == true)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt;
        }
        else if (time <= 0 && timerActive == true)
        {
            timerEnd();
        }
        if(time < 40 && timerActive == true && t<1.0)
        {
            float blendStep = Mathf.Lerp(0, 1, t);
            t += 0.3f * Time.deltaTime;
            Debug.Log(blendStep);
            skybox_Material.SetFloat("_Blend", blendStep);
            animator.SetBool("active", true);
        }
    }

    void timerStart()
    {
        timerActive = true;
    }

    void timerEnd()
    {
        //Status wird aktualisiert, die Zeit angehalten und das Menü aktiviert
        timerActive = false;
        Time.timeScale = 0f;
        restartMenuUI.SetActive(true);
        FindObjectOfType<AudioManager>().Stop("Ticking");
        FindObjectOfType<AudioManager>().Play("Ring");
    }
}