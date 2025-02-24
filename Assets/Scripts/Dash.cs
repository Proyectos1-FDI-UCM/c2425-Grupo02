// ---------------------------------------------------------
// Mecánica de dash del jugador
// Pablo Cayuela de la Fuente
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Dash : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private float dashtime = 0.1f;  //tiempo que dura el dash
    [SerializeField] private float dashtimec = 1.5f;   //cooldown entre cada dash
    [SerializeField] private float dashforce = 3.0f;  // velocidad del dash
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Rigidbody2D rb;  //rigidbody para colisiones
    private Movement player;
    private bool dash;   // detecta si está dasheando
    private bool candash = true;   // si se puede dashear dash == true

    #endregion
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Movement>();
    }

    void Update()
    {
        if (InputManager.Instance.DashWasPressedThisFrame())   //si se pulsa cualquier shift o el r1 del mando de ps4/5 se activa el dash
        {
            

                StartCoroutine(_Dash());

            

        }
    }

    public bool isdashing()   //booleano que detecta si se está dasheando
    {
        bool ds = true;
        if (!dash)
        {
            ds = false;
        }
        return ds;
    }

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>

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
    private IEnumerator _Dash()
    {
        if (candash == true && dash == false)  // si se puede dashear y no se está dasheando
        {
            Vector2 lastDir = player.GetLastDir();
            Vector2 dashpos = transform.position;
            candash = false;
            dash = true;

            if (lastDir.x > 0 && lastDir.y == 0) { dashpos.x = 1; dashpos.y = 0; }  //arriba, abajo, izq y der
            else if (lastDir.x < 0 && lastDir.y == 0) { dashpos.x = -1; dashpos.y = 0; }
            else if (lastDir.x == 0 && lastDir.y > 0) { dashpos.x = 0; dashpos.y = 1; }
            else if (lastDir.x == 0 && lastDir.y < 0) { dashpos.x = 0; dashpos.y = -1; }

           // creo que no va por el movement, pero sería así 
           //                II
           //                II
           //                vv

         //   else if (lastDir.x > 0 && lastDir.y > 0) { dashpos.x = 1; dashpos.y = 1; }  //arriba+der, arriba+izq, abajo+der y abajo+izq
         //   else if (lastDir.x < 0 && lastDir.y > 0) { dashpos.x = -1; dashpos.y = 1; }
         //   else if (lastDir.x > 0 && lastDir.y < 0) { dashpos.x = 1; dashpos.y = -1; }
         //   else { dashpos.x = -1; dashpos.y = -1; }



            rb.velocity = new Vector2(dashpos.x * dashforce, dashpos.y * dashforce); // velocidad del dash
            yield return new WaitForSeconds(dashtime);   //espera a que pase el tiempo activo del dash
            dash = false;
            dashpos.x = dashpos.y = 0;    //cuando se deja de dashear, la velocidad se reestablece
            rb.velocity = new Vector2(dashpos.x, dashpos.y);

            yield return new WaitForSeconds(dashtimec);   //cooldown 

            candash = true;
        }
            }
                #endregion

            } 

