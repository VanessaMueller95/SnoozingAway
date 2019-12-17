using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{

    //Zum Aktivieren oder Deaktivieren
    public bool timerActive = false;

    //Übergabe des MenüObjekts, damit es aktiviert werden kann
    public GameObject restartMenuUI;

    Image fillImg;

    //Zielzeit
    public float timeAmt = 50;
    //aktuelle Zeit
    public float time;

    //Variablen zur Animation der Skybox
    Material skybox_Material;
    private float t = 0.0f;

    //Animator für das Pulsieren
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
        // Zeit wird reduziert und das Bild aktualisiert
        if (time > 0 && timerActive == true)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt;
        }
        //Timer wird beendet wenn die Zeit unter 0 sinkt
        else if (time <= 0 && timerActive == true)
        {
            timerEnd();
        }
        //sinkt die Zeit unter 15 Sekunden beginnen die Hinweisanimationen
        if(time < 15 && timerActive == true && t<1.0)
        {
            //Skybox Shader wird animiert
            float blendStep = Mathf.Lerp(0, 1, t);
            t += 0.3f * Time.deltaTime;
            skybox_Material.SetFloat("_Blend", blendStep);
            //Blinkanimation wird gestartet
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