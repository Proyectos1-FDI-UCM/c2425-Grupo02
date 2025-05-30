//---------------------------------------------------------
// Script que se encarga de ejecutar el ataque del enemigo 1 cuando su máquina de estados lo sitúe en el estado de ataque.
// Sergio López Gómez
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase de ataque del enemigo. Contiene un método público que se llamará desde el script de comportamiento del enemigo
/// cuando tenga que atacar. Extiende "IAttack"
/// </summary>
public class Enemy1_Attack : MonoBehaviour, IAttack
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Cantidad de vidas que resta el enemigo al jugador con cada golpe de su ataque
    /// </summary>
    [SerializeField] int Damage;
    /// <summary>
    /// Tiempo que pasa entre cada golpe del ataque
    /// </summary>
    [SerializeField] float AttackCooldown;
    /// <summary>
    /// Sonido de daño al jugador
    /// </summary>
    [SerializeField]
    private AudioClip LilithInjuredSFX;
    [SerializeField] private Sprite RangeHorizontal;
    [SerializeField] private Sprite RangeVertical;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Objeto hijo del enemigo que tiene la hitbox del atque
    /// </summary>
    GameObject _hitbox;
    /// <summary>
    /// Máquina de estados del enemigo
    /// </summary>
    Enemy_StateMachine _stateMachine;
    /// <summary>
    /// Dirección en la que se mueve el enemigo
    /// </summary>
    Vector2 _dir;    
    /// <summary>
    /// Número de veces que golpea el enemigo por ataque
    /// </summary>
    int _timesAttack = 3;
    /// <summary>
    /// Distancia que se mueve la hitbox del centro del enemigo cuando va a golpear
    /// </summary>
    float _hitboxOffset = 0.25f;
    /// <summary>
    /// Tiempo que está activo el trigger de la hitbox durante cada golpe del ataque
    /// </summary>
    float _hitboxActiveTime = 0.1f;
    private SpriteRenderer _rangeSpriteRenderer;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _hitbox = transform.GetChild(0).gameObject;
        _rangeSpriteRenderer = _hitbox.GetComponent<SpriteRenderer>();
        _stateMachine = GetComponent<Enemy_StateMachine>();
        _hitbox.SetActive(false);
    }

    /// <summary>
    /// Si la hitbox colisiona con el jugador, llama el método para dañarle
    /// </summary>
    /// <param name="collision"> Colisión del objeto con el que se ha colisionado </param>
    private void OnTriggerEnter2D(Collider2D collision) {
        Player_Health health = collision.GetComponent<Player_Health>();
        if (health != null)
        {
            health.Damage(Damage);
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    /// <summary>
    /// Método que llama a la corrutina encargada de activar la hitbox y la desactiva pasado un breve lapso de tiempo 
    /// (tantas veces como _timesAttack indique).
    /// Antes de eso, también mueve la hitbox de ataque llamando al método "SetDir"
    /// </summary>
    public IEnumerator Attack() {
        _dir = _stateMachine.GetDir();
        SetDir(_dir, _hitboxOffset);
        SetRangeSprite();
        for (int i = 0; i < _timesAttack; i++)
        {
            _hitbox.SetActive(true);
            AudioManager.Instance.PlayAudio(LilithInjuredSFX, 0.5f);
            yield return new WaitForSeconds(_hitboxActiveTime);
            _hitbox.SetActive(false);
            yield return new WaitForSeconds(AttackCooldown);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método que dictamina la posición y rotación de la hitbox del ataque
    /// </summary>
    /// <param name="v"> Vector de dirección del jugador. Es usado para determinar la posición y rotación de la hitbox </param>
    /// <param name="offset"> Float que indica cuánto desplazamos la hitbox con respecto al centro del enemigo </param>
    void SetDir(Vector2 v, float offset) {
        Vector2 res;

        if(v.y == 0)
        {
            if (v.x > 0)
            {
                res = new Vector2(offset, 0);
            }
            else if (v.x < 0)
            {
                res = new Vector2(-offset, 0);
            }
            else
            {
                res = Vector2.zero;
            }
        }
        else
        {
            if (v.y > 0)
            {
                res = new Vector2(0, offset);
            }
            else
            {
                res = new Vector2(0, -offset);
            }
        }
        _hitbox.transform.localPosition = res;
    }
    void SetRangeSprite()
    {
        if (_dir.y > 0 || _dir.y < 0)
        {
            _rangeSpriteRenderer.sprite = RangeHorizontal;
            _rangeSpriteRenderer.flipY = _dir.y > 0;
        }
        else
        {
            _rangeSpriteRenderer.sprite = RangeVertical;
            _rangeSpriteRenderer.flipX = _dir.x < 0;
        }
    }
    #endregion   

} // class Enemy1_attack 
// namespace
