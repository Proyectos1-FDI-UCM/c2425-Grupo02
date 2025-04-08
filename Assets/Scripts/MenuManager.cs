//---------------------------------------------------------
// Componente para gestionar menús
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestión de acciones en menús
/// </summary>
public class MenuManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] GameObject ContinueButton;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private static MenuManager _instance;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
        }
    }
    private void Start()
    {
        if (GameManager.Instance.GetCheckpoint == 0)
        {
            ContinueButton.SetActive(false);
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public static MenuManager Instance
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
    
    public void StartNewGame()
    {
            GameManager.Instance.GetCheckpoint = 0;
            GameManager.Instance.ChangeScene(1);
            GameManager.Instance.SetSpawnPoint(new Vector2(0, 0));   
    }
    public void ContinueGame()
    {
        if (GameManager.Instance.GetCheckpoint == 1)
        {
            GameManager.Instance.ChangeScene(2);
            GameManager.Instance.SetSpawnPoint(new Vector2(0, 2));
        }
        else if (GameManager.Instance.GetCheckpoint == 2)
        {
            GameManager.Instance.ChangeScene(6);
            GameManager.Instance.SetSpawnPoint(new Vector2(-25, 0));
        }
        else if (GameManager.Instance.GetCheckpoint == 3)
        {
            GameManager.Instance.ChangeScene(8);
            GameManager.Instance.SetSpawnPoint(new Vector2(-32, -23));
        }
        else if (GameManager.Instance.GetCheckpoint == 4)
        {
            GameManager.Instance.ChangeScene(9);
            GameManager.Instance.SetSpawnPoint(new Vector2(-33, -25));
        }
        else if (GameManager.Instance.GetCheckpoint == 5)
        {
            GameManager.Instance.ChangeScene(4);
            GameManager.Instance.SetSpawnPoint(new Vector2(-1.5f, 7.25f));
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class MenuManager 
// namespace
