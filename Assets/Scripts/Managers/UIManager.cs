//---------------------------------------------------------
// Gestor de la UI
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private Canvas MissionCompletedCanvas;
    [SerializeField] private GameObject DialogueUI;
    [SerializeField] private Animator FaderAnimator;
    [SerializeField] private Canvas Controls;
    //[SerializeField] private GameObject HealthSprite;
    //[SerializeField] private Sprite[] health_inspector = new Sprite[3];
    //[SerializeField] private Image health_sprite_inspector;
    [SerializeField] private Text quest_objects_count_inspector;
    [SerializeField] private GameObject questUI;
    [SerializeField] private Image Health_sprite;
    [SerializeField] private Sprite[] Health;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private static UIManager _instance;
    //private SpriteRenderer spriteRenderer;
    private static Text quest_objects_count;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// 
    /// </summary>
    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// 
    /// Desactiva el canvas de diálogos, opciones y de misión al inicio
    /// </summary>
    void Start()
    {
        MissionCompletedCanvas.gameObject.SetActive(false);
        DialogueUI.SetActive(false);
        Controls.gameObject.SetActive(false);
        quest_objects_count = quest_objects_count_inspector;
        if (GameManager.Instance.questState() != 1)
        {
            questUI.gameObject.SetActive(false);
        }
        else
        {
            quest_objects_count.text = "x" + GameManager.Instance.questObjectsCount();
        }
        /*
        spriteRenderer = GetComponent<SpriteRenderer>();
        health_sprite = health_sprite_inspector;
        health = health_inspector;
        */
        Health_sprite.sprite = Health[2];
        
    }
    /// <summary>
    /// Si el juego está en marcha y pulsa el botón para pausar, te muestra los controles. 
    /// Si no, te oculta la pantalla de controles.
    /// </summary>
    private void Update()
    {
        if (InputManager.Instance.PauseWasPressedThisFrame()) 
        { 
            if (Time.timeScale == 1)
            {
                ShowControls();
            }
            else
            {
                HideControls();
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public static UIManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }
    public static bool HasInstance()
    {
        return _instance != null;
    }

    public void Inform()
    {
        MissionCompletedCanvas.gameObject.SetActive(true);
    }
    /// <summary>
    /// Muestra UI de diálogos
    /// </summary>
    public void ShowDialogueUI()
    {
        DialogueUI.SetActive(true);
    }
    /// <summary>
    /// Oculta UI de diálogos
    /// </summary>
    public void HideDialogueUI()
    {
        DialogueUI.SetActive(false);
    }
    public void ShowQuestUI()
    {
        questUI.SetActive(true);
    }
    public void HideQuestUI()
    {
        questUI.SetActive(false);
    }

    public void UpdateHealth(int health)
    {
        if (health >= 1)
        {
            Health_sprite.sprite = Health[health - 1];
            Debug.Log("Indice sprite:" + (health - 1));
        }
    }

    public static void UpdateQuestProgress(int count)
    {
        quest_objects_count.text = "x" + count;
    }

        public void SceneTransition()
    {
        if (FaderAnimator != null)
        {
            FaderAnimator.SetTrigger("FadeOut");
        }
    }



    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private void ShowControls()
    {
        Controls.gameObject.SetActive(true);
        Time.timeScale = 0;
        LevelManager.Instance.DisableBehaviours();
    }
    private void HideControls()
    {
        Controls.gameObject.SetActive(false);
        Time.timeScale = 1;
        LevelManager.Instance.EnableBehaviours();
    }
    #endregion   

} // class UIManager 
// namespace
