//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Enemy1 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] float movement_speed;
    [SerializeField] float rest_time;
    [SerializeField] float attack_cooldown;
    [SerializeField] int health;

    #endregion


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    GameObject player;
    Rigidbody2D rb;
    float timer = 0f;
    bool is_resting = false;
    bool on_range = false;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start() {
        player = FindObjectOfType<Movement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        /*
        if (!is_resting)
        {
            if (!on_range)
            {
                Debug.Log("Persiguiendo");
                Vector2 player_position = player.transform.position;
                Vector2 dir = (player_position - (Vector2)transform.position).normalized;
                rb.velocity = dir * movement_speed;
            }
            else
            {
                Attack();
                Debug.Log("Ataque");
                is_resting = true;
            }
        }
        else
        {
            Debug.Log("Descansando");
            timer += Time.deltaTime;

            if (timer >= rest_time)
            {
                timer = 0f;
                is_resting = false;
            }
        }
         */
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision) {
        on_range = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        on_range = false;
    }
     */



    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Esta función se debe llamar desde el script que se encarge del ataque del jugador.
    /// Una vez llamado, resta el daño pasado por parámetro a la vida actual del enemigo.
    /// </summary>
    /// <param name="dmg"> daño que le hace el jugador al enemigo </param>
    public void Damage(int dmg) {
        health -= dmg;
        if (health <= 0) Destroy(gameObject);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    void Attack() {

    }

    #endregion

} // class Enemy1 
// namespace
