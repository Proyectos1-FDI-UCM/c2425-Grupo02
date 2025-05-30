//---------------------------------------------------------
// Componente de ataque melee del jugador
// Jorge Augusto Blanco Fernández
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestiona la acción de ataque del jugador, así como su tiempo de enfriamiento.
/// 
/// </summary>
public class MeleeAttack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField]
    private Animator animator; // Componente Animator del _player

    [SerializeField]
    private AudioClip meleeSFX; // Sonido de ataque del jugador

    [SerializeField]
    private Transform scytheLocation; // Posición de la guadaña con respecto a _player

    [SerializeField]
    private Transform playerLocation; // Posición de _player

    [SerializeField]
    private LayerMask enemyLayer; // Capa en la que se encuentran los enemigos

    [SerializeField]
    private int damage = 1; // Daño del ataque 

    [SerializeField]
    private int attackRadius = 1; // Radio del ataque 

    [SerializeField]
    private float cooldownTime = 1f; // Tiempo de enfriamiento del ataque

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    Movement _movement;
    private float cooldownTimer = 0f; // Tiempo que debe transcurrir para poder llamar al método Attack

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        _movement = GetComponent<Movement>();
        if (!GameManager.Instance.HasScythe)
        {
            enabled = false;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        if (InputManager.Instance.AttackIsPressed() && cooldownTimer <= 0) Attack();

        else cooldownTimer -= Time.deltaTime;
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
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Método llamado en Update si se presiona el botón de ataque y el tiempo de enfriamiento ha transcurrido.
    /// Genera una colisión circular en la dirección en la que mira el jugador a través del GameObject Scythe. 
    /// </summary>
    private void Attack()
    {
        animator.SetTrigger("Attack");
        AudioManager.Instance.PlayAudio(meleeSFX, 0.3f);

        Vector2 lastDir = _movement.GetLastDir();

        scytheLocation.position = playerLocation.transform.position + new Vector3(lastDir.x, lastDir.y, 0);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(scytheLocation.position, attackRadius, enemyLayer);

        foreach (Collider2D collider in enemies)
        {
            if (collider.GetComponent<Enemy_Health>() != null)
            {
                collider.GetComponent<Enemy_Health>().Damage(damage);
            }

            else if (collider.GetComponent<Pillar>() != null)
            {
                collider.GetComponent<Pillar>().Damage(damage);
            }

            else if (collider.GetComponent<Boss_Life_Phase1>() != null)
            {
                collider.GetComponent<Boss_Life_Phase1>().Damage(damage);
            }
        }

        cooldownTimer = cooldownTime;
    }

    ///(Método Debug)
    /// <summary>
    /// Dibuja el radio de ataque en el editor de Unity.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (scytheLocation == null) return;

        Gizmos.DrawWireSphere(scytheLocation.position, attackRadius);
    }
    #endregion

} // class MeleeAttack 
// namespace
