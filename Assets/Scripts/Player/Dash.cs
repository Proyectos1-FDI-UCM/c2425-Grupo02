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
    ///cooldown entre cada dash
    [SerializeField] private float dashtimec;  
    /// distancia de tp de dash
    [SerializeField] private float dashDistance;
    /// Sonido de dash del jugador
    [SerializeField]
    private AudioClip LilithDashSFX;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// _candash -> detecta si el jugador puede o no hacer dash
    /// </summary>
    private Rigidbody2D _rb;        
    private Movement _player;           
    private bool _candash = true;
    private float _dashCooldownTimer = 1.5f;
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
        
        if (InputManager.Instance.DashWasPressedThisFrame() && _dashCooldownTimer >= dashtimec)   
        {
            TryDash();
            _dashCooldownTimer = 0;
        }

        else if (_dashCooldownTimer < dashtimec)
        {
            _dashCooldownTimer += Time.deltaTime;
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
    /// Si el raycast se activa, se puede hacer dash (_candash = true)
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
        else  
        {
            Debug.DrawRay(transform.position, raylong * dashDistance, Color.green);  
            _candash = true;
        }

    }

    /// <summary>
    /// Si puede dashear, se activa el sonido del dash y se cambia la posición del player
    /// </summary>
    private void TryDash()
    {
        if (_candash == true)
        {
            AudioManager.Instance.PlayAudio(LilithDashSFX, 0.2f);
            Vector2 lastDir = _player.GetLastDir2();
            _rb.position += lastDir.normalized * dashDistance;
            _rb.MovePosition(_rb.position);
        }

    }
    #endregion

}

