//---------------------------------------------------------
// Componente para los checkpoint
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Cuando el jugador entra en el trigger, le dice al gamemanager que actualice su indice de checkpoint, la posición de spawn después de un gameover y la escena en la que spawnea
/// </summary>
public class Checkpoint : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private int CheckpointNumber;
    [SerializeField] private int SceneIndex;
    [SerializeField] private Vector2 SpawnPos;
    [SerializeField] private Sprite ObtainedState;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    SpriteRenderer _spriteRenderer;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (GameManager.Instance.SavedCheckpoint >= CheckpointNumber)
        {
            _spriteRenderer.sprite = ObtainedState;
        }
    }
/// <summary>
/// Al entrar en el checkpoint, se guarda en el gamemanager el número del checkpoint, su posición de spawn para el player y el índice de la escena
/// donde debe spawnear la próxima vez
/// </summary>
/// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.SavedCheckpoint < CheckpointNumber)
        {
            GameManager.Instance.SetNewCheckpoint(CheckpointNumber, SpawnPos, SceneIndex);
            UIManager.Instance.ShowCheckpointNotif();
            _spriteRenderer.sprite = ObtainedState;
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion

} // class Checkpoint 
// namespace
