// ---------------------------------------------------------
// Componente de movimiento del jugador
// Isabel Serrano Martín
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Componente que comunica con el InputManager para 
/// permitir el movimiento del jugador
/// </summary>
public class Movement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private float Speed = 1.0f; //velocidad de movimiento

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Rigidbody2D rb; //rigidbody para colisiones
    private Animator animator;
    private Dash dash;
    private int control = 1;  //variable de control para que al hacer dash no se pueda mover el player
    private Vector2 lastDir;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //inicializamos rigidbody
        animator = GetComponent<Animator>();
        dash = GetComponent<Dash>();    //tomamos el código del dash
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Vector2 movement = InputManager.Instance.MovementVector * Speed * control;
        Vector2 worldPos = rb.position + movement * Time.fixedDeltaTime;

        Vector2 lastDir = GetLastDir();
            
        animator.SetFloat("moveX", lastDir.x);
        animator.SetFloat("moveY", lastDir.y);

        //TOROIDALIDAD
        Vector2 mapSize = LevelManager.Instance.GetMapSize();
        
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        //obtiene posición de la bala y lo convierte al espacio de la cámara
        //(0,1) = esquina superior izquierda    (1,1) = esquina superior derecha
        //(0,0) = esquina inferior izquierda    (1,0) = esquina inferior derecha

        if (viewPos.x > 1) worldPos.x -= mapSize.x; //de derecha a izquierda
        else if (viewPos.x < 0) worldPos.x += mapSize.x; //de izquierda a derecha
        else if (viewPos.y < 0) worldPos.y += mapSize.y; //de abajo a arriba
        else if (viewPos.y > 1) worldPos.y -= mapSize.y; //de arriba a abajo

        rb.MovePosition(worldPos);
       // bool ddash = dash.isdashing();   //la variable booleana ddash representa al método isdashing del scrpit dash, que detecta si se está en estado de dash o no
       // if (ddash == true) { control= 0; }  //si está dasheando el player no puede moverse, si no lo hace si puede
       // else { control = 1; }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public Vector2 GetLastDir()
    {
        Vector2 moveInput = InputManager.Instance.MovementVector; //vector movimiento

        if (moveInput != Vector2.zero)
        {
            //mira si se mueve más en vertical o en horizontal
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)) //horizontal
            {
                if (moveInput.x > 0) lastDir = new Vector2(1, 0); //derecha
                else lastDir = new Vector2(-1, 0); //izquierda
            }
            else //vertical
            {
                if (moveInput.y > 0) lastDir = new Vector2(0, 1); //arriba
                else lastDir = new Vector2(0, -1); //abajo
            }
        }
        return lastDir;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} //class Movement
  // namespace