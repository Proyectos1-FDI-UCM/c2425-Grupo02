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
    [SerializeField] private float dashDistance = 1f;  // distancia de tp de dash
                                                       // Documentar cada atributo que aparece aquí.
                                                       // El convenio de nombres de Unity recomienda que los atributos
                                                       // públicos y de inspector se nombren en formato PascalCase
                                                       // (palabras con primera letra mayúscula, incluida la primera letra)
                                                       // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// __dash -> detecta si el jugador está o no dasheando
    /// _candash -> detecta si el jugador puede o no hacer dash
    /// </summary>
    private Rigidbody2D _rb;        
    private Movement _player;        
    private bool _dash;             
    private bool _candash = true;   
    Vector2 raylong;                

    #endregion
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Movement>();
    }

    /// <summary>
    /// si se pulsa el botón de dash, se inicia el dash
    /// raycast que evita el dash al estar a una determinada distancia de obstáculos
    /// </summary>
    void Update()
    {

        if (InputManager.Instance.DashWasPressedThisFrame())   
        {
            StartCoroutine(Dashh());
        }

        Raycast();


    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados


    /// <summary>
    /// Raycast que se activa al entrar en contacto con objetos de determinadas capas
    /// La longuitud del raycast depende de la distancia del dash
    /// Si el raycast se activa, se puede hacer dash
    /// </summary>
    private void Raycast()
    {
        raylong = _player.GetLastDir2().normalized;  
        LayerMask layerMask = LayerMask.GetMask("Obstacles");  
        RaycastHit2D hit = Physics2D.Raycast(_rb.position, raylong, dashDistance, layerMask); 

        if (hit.collider != null)  
        {
            _candash = false;
            Debug.DrawRay(_rb.position, raylong * dashDistance, Color.red);  

        }
        else   //si no
        {
            Debug.DrawRay(transform.position, raylong * dashDistance, Color.green);  
            _candash = true;

        }
    }

    /// <summary>
    /// Corrutina del dash, que regula lo que tarda en hacer el dash y el countdown del dash  
    /// lastDar es un método público del script movement que detecta la última posición del jugador
    /// </summary>
    private IEnumerator Dashh()
    {
        if (_candash == true && _dash == false)  
        {

            Vector2 lastDir = _player.GetLastDir2();  
            _candash = false;
            _dash = true;

            _rb.position += lastDir.normalized * dashDistance;
            _rb.MovePosition(_rb. position);                   
            yield return new WaitForSeconds(dashtime);  
            _dash = false;  //no está dasheando
            yield return new WaitForSeconds(dashtimec);  
            _candash = true;
        }
    }
    #endregion

}

