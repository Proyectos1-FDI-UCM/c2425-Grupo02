//---------------------------------------------------------
// Gestor de la UI
// Lucía Mei Domínguez López, Isabel Serrano Martín
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

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

    [SerializeField] private GameObject DialogueUI;
    [SerializeField] private Animator FaderAnimator;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject ControlsScreen;
    [SerializeField] private Text quest_objects_count_inspector;
    [SerializeField] private GameObject questUI;
    [SerializeField] private Image Health_sprite;
    [SerializeField] private Sprite[] Health;
    [SerializeField] private Image ShootIcon;
    [SerializeField] private Image ShootDarker;
    [SerializeField] private Image DashIcon;
    [SerializeField] private Image DashDarker;
    [SerializeField] private GameObject CheckpointNotif;
    [SerializeField] private GameObject CheckpointBarNotif;
    /// <summary>
    /// Tiempo durante el que se muestra la notificación del checkpoint
    /// </summary>
    [SerializeField] private float CheckNotifDuration;
    [SerializeField] private float CheckNotifBarDuration;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private static UIManager _instance;
    private static Text quest_objects_count;
    /// <summary>
    /// Indica si hay una notificación en curso
    /// </summary>
    private bool _notifOnGoing = false;
    private bool _notifBarOnGoing = false;
    /// <summary>
    /// Timer de la notificación
    /// </summary>
    private float _notifTimer = 0;
    private bool _paused = false;
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
        if (!GameManager.Instance.HasScythe)
        {
            ShootIcon.gameObject.SetActive(false);
            ShootDarker.gameObject.SetActive(false);
        }
        DashIcon.fillAmount = 1;
        DialogueUI.SetActive(false);
        PauseMenu.SetActive(false);
        

        if(ControlsScreen == null)
        {
            Debug.LogError("el canvas no está asignado uwu");
        }
        else
        {
            ControlsScreen.SetActive(false);
        }


        if (CheckpointNotif != null)
        {
            CheckpointNotif.SetActive(false);
        }
        if (CheckpointBarNotif != null)
        {
            CheckpointBarNotif.SetActive(false);
        }
        quest_objects_count = quest_objects_count_inspector;
        if (GameManager.Instance.questState() != 1)
        {
            questUI.gameObject.SetActive(false);
        }
        else
        {
            quest_objects_count.text = "x" + GameManager.Instance.questObjectsCount();
        }
    }
    /// <summary>
    /// Si el juego está en marcha y pulsa el botón para pausar, te muestra los controles. 
    /// Si no, te oculta la pantalla de controles.
    /// </summary>
    private void Update()
    {
        if (InputManager.Instance.PauseWasPressedThisFrame()) 
        { 
            if (!_paused)
            {
                ShowMenu();
                _paused = true;
            }
            else if (ControlsScreen.activeSelf)
            {
                ShowMenu();
                HideControls();
            }
            else
            {
                HideMenu();
                _paused = false;
            }
        }

        if (_notifOnGoing)
        {
            _notifTimer += Time.deltaTime;
            if (_notifTimer >= CheckNotifDuration)
            {   
                HideCheckpointNotif();
            }
            
        }
        else if (_notifBarOnGoing)
        {
            _notifTimer += Time.deltaTime;
            if (_notifTimer >= CheckNotifBarDuration)
            {
                HideCheckpointBarNotif();
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
    /// <summary>
    /// Getter que indica si el juego está pausado
    /// </summary>
    public bool IsPaused
    {
        get { return _paused; }
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShowDialogueUI()
    {
        HideCooldownIcons();
        DialogueUI.SetActive(true);
    }
    /// <summary>
    /// Oculta UI de diálogos
    /// </summary>
    public void HideDialogueUI()
    {
        DialogueUI.SetActive(false);
        ShowCooldownIcons();
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShowQuestUI()
    {
        questUI.SetActive(true);
    }
    /// <summary>
    /// 
    /// </summary>
    public void HideQuestUI()
    {
        questUI.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="health"></param>
    public void UpdateHealth(int health)
    {
        if (health >= 1)
        {
            Health_sprite.sprite = Health[health - 1];
            Debug.Log("Indice sprite:" + (health - 1));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    public void UpdateQuestProgress(int count)
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
    /// <summary>
    /// Muestra el icono de cooldown del disparo
    /// </summary>
    public void ShowShootIcon()
    {
        ShootIcon.gameObject.SetActive(true);
        ShootDarker.gameObject.SetActive(true);
        ShootIcon.fillAmount = 1;
    }
    /// <summary>
    /// Muestra el cooldown en pantalla
    /// </summary>
    public void StartShootCooldown()
    {
        ShootIcon.fillAmount = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="cooldown"></param>
    public void UpdateShootCooldown(float timer, float cooldown)
    {
        ShootIcon.fillAmount = timer / cooldown;
    }
    /// <summary>
    /// 
    /// </summary>
    public void StartDashCooldown()
    {
        DashIcon.fillAmount = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="cooldown"></param>
    public void UpdateDashCooldown(float timer, float cooldown)
    {
        DashIcon.fillAmount = timer / cooldown;
    }
    /// <summary>
    /// Muestra la notificación de checkpoint
    /// </summary>
    public void ShowCheckpointNotif()
    {
        if (CheckpointNotif != null)
        {
            CheckpointNotif.SetActive(true);
            _notifOnGoing = true;
        }
    }
    /// <summary>
    /// Muestra la notificación de checkpoint del bartender
    /// </summary>
    public void ShowCheckpointBarNotif()
    {
        if (CheckpointBarNotif != null)
        {
            CheckpointBarNotif.SetActive(true);
            _notifBarOnGoing = true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShowControls()
    {
        if (ControlsScreen == null)
        {
            Debug.LogError("controls es null al intentar mostrarlo");
            return;
        }
       
        ControlsScreen.SetActive(true);
        PauseMenu.SetActive(false);
        
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShowMenu()
    {
        PauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        LevelManager.Instance.DisableBehaviours();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// 
    /// </summary>
    private void HideMenu()
    {
        PauseMenu.gameObject.SetActive(false);
        if (!DialogueManager.Instance.DialogueIsOnGoing)
        {
            Time.timeScale = 1;
            LevelManager.Instance.EnableBehaviours();
        }
        HideControls();
    }
    /// <summary>
    /// 
    /// </summary>
    private void HideControls()
    {
        ControlsScreen.SetActive(false);
    }
    /// <summary>
    /// Esconde la notificación de checkpoint
    /// </summary>
    private void HideCheckpointNotif()
    {
        if (CheckpointNotif != null)
        {
            _notifOnGoing = false;
            _notifTimer = 0;
            CheckpointNotif.SetActive(false);
        }
    }
    /// <summary>
    /// Esconde la notificación del checkpoint del bartender
    /// </summary>
    private void HideCheckpointBarNotif()
    {
        if (CheckpointBarNotif != null)
        {
            _notifBarOnGoing = false;
            _notifTimer = 0;
            CheckpointBarNotif.SetActive(false);
        }
    }
    /// <summary>
    /// Muestra los cooldowns
    /// </summary>
    private void ShowCooldownIcons()
    {
        DashIcon.gameObject.SetActive(true);
        DashDarker.gameObject.SetActive(true);
        if (GameManager.Instance.HasScythe)
        {
            ShootIcon.gameObject.SetActive(true);
            ShootDarker.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Esconde los cooldowns
    /// </summary>
    private void HideCooldownIcons()
    {
        DashIcon.gameObject.SetActive(false);
        DashDarker.gameObject.SetActive(false);
        ShootIcon.gameObject.SetActive(false);
        ShootDarker.gameObject.SetActive(false);
    }
    #endregion   

} // class UIManager 
// namespace
