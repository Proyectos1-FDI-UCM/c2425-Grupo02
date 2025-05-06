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
public class Enemy_Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Número de vidas del enemigo
    /// </summary>
    [SerializeField] int Health;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    // SpriteRenderer del enemigo
    private SpriteRenderer spriteRend;

    /// <summary>
    /// Spawn que instanció al enemigo
    /// </summary>
    Enemy_Spawn _spawn;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (InputManager.Instance.DestroyWasPressedThisFrame())
        {
            Damage(Health);
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Cuando se llama, se le resta el daño pasado por parametro a Health.
    /// Si Health llega a 0 el enmigo se destruye.
    /// Si el enemigo ha sido instanciado por un spawn, le comunica a dicho spawn si ha sido destruido
    /// </summary>
    /// <param name="dmg"> Entero que se le resta a Health </param>
    public void Damage(int dmg) {
        Health -= dmg;
        if (Health <= 0)
        {
            if(_spawn != null) 
            {
                _spawn.SubstractEnemy();
            }
            if (LevelManager.Instance.InitCombatStarted) 
            { 
                LevelManager.Instance.SubEnemyCount();
            }
            if (GetComponent<IramisPhase2_Attack>() != null)
            {
                GameManager.Instance.ChangeScene(12);
            }
            Destroy(gameObject);
        }

        else
        {
            StartCoroutine(EnemyHit());
        }
    }

    /// <summary>
    /// Método que asigna al enemigo un spawn
    /// </summary>
    /// <param name="spawn"> Spawn que instanció al enemigo </param>
    public void SetSpawn(Enemy_Spawn spawn) {
        _spawn = spawn;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método que muestra visualmente que el enemigo ha sido dañado.
    /// </summary>
    private IEnumerator EnemyHit()
    {
        spriteRend.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);
        spriteRend.color = Color.white;
    }

    #endregion   

} // class Enemy_health 
// namespace
