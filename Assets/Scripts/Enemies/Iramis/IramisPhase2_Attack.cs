//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class IramisPhase2_Attack : MonoBehaviour, IAttack
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Cantidad de vidas que resta el enemigo al jugador con cada golpe de su ataque
    /// </summary>
    [SerializeField] int Damage;
    /// <summary>
    /// Tiempo que pasa entre cada golpe del ataque
    /// </summary>
    [SerializeField] float AttackCooldown;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Objeto hijo del enemigo que tiene la hitbox del atque
    /// </summary>
    GameObject _hitbox;
    /// <summary>
    /// Máquina de estados del jefe
    /// </summary>
    IramisPhase2_StateMachine _stateMachine;
    /// <summary>
    /// Dirección en la que se mueve el enemigo
    /// </summary>
    Vector2 _dir;
    /// <summary>
    /// Número de veces que golpea el enemigo por ataque
    /// </summary>
    int _timesAttack = 2;
    /// <summary>
    /// Distancia que se mueve la hitbox del centro del enemigo cuando va a golpear
    /// </summary>
    float _hitboxOffset = 0.25f;
    /// <summary>
    /// Tiempo que está activo el trigger de la hitbox durante cada golpe del ataque
    /// </summary>
    float _hitboxActiveTime = 0.1f;

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
        _hitbox = transform.GetChild(0).gameObject;
        _stateMachine = GetComponent<IramisPhase2_StateMachine>();
        _hitbox.SetActive(false);
    }

    /// <summary>
    /// Si la hitbox colisiona con el jugador, llama el método para dañarle
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Health health = collision.GetComponent<Player_Health>();
        if (health != null)
        {
            health.Damage(Damage);
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

    /// <summary>
    /// Método que llama a la corrutina encargada de activar la hitbox y la desactiva pasado un breve lapso de tiempo 
    /// (tantas veces como _timesAttack indique).
    /// Antes de eso, también mueve la hitbox de ataque llamando al método "SetDir"
    /// </summary>
    public IEnumerator Attack()
    {
        _dir = _stateMachine.GetDir();
        SetDir(_dir, _hitboxOffset);

        for (int i = 0; i < _timesAttack; i++)
        {
            _hitbox.SetActive(true);
            yield return new WaitForSeconds(_hitboxActiveTime);
            _hitbox.SetActive(false);
            yield return new WaitForSeconds(AttackCooldown);
        }
    }

    public IEnumerator Shoot()
    {
        _dir = _stateMachine.GetDir();
        SetDir(_dir, _hitboxOffset);

        for (int i = 0; i < _timesAttack; i++)
        {
            _hitbox.SetActive(true);
            yield return new WaitForSeconds(_hitboxActiveTime);
            _hitbox.SetActive(false);
            yield return new WaitForSeconds(AttackCooldown);
        }
    }



    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Método que dictamina la posición y rotación de la hitbox del ataque
    /// </summary>
    /// <param name="v"> Vector de dirección del jugador. Es usado para determinar la posición y rotación de la hitbox </param>
    /// <param name="offset"> Float que indica cuánto desplazamos la hitbox con respecto al centro del enemigo </param>
    void SetDir(Vector2 v, float offset)
    {
        Vector2 res;
        _hitbox.transform.rotation = transform.rotation;

        if (v.y == 0)
        {
            if (v.x > 0) res = new Vector2(offset, 0);
            else if (v.x < 0) res = new Vector2(-offset, 0);
            else res = Vector2.zero;
        }
        else
        {
            if (v.y > 0) res = new Vector2(0, offset);
            else res = new Vector2(0, -offset);

            _hitbox.transform.Rotate(0, 0, 90, Space.World);

        }
        _hitbox.transform.localPosition = res;
    }

    #endregion

} // class IramisPhase2Attacks 
// namespace
