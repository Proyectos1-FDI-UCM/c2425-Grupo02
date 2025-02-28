//---------------------------------------------------------
// Componente de disparo del jugador
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Componente de prueba que se comunica con el InputManager
/// para mostrar por consola los eventos de la acción Fire.
/// Como los eventos IsPressed se muestran cada frame y
/// saturan la consola, tenemos un tick en el editor para
/// habilitarlos
/// </summary>
public class Shoot : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Si está activado, se muestran todos los eventos de que
    /// la acción está siendo realizada (uno por frame)
    /// </summary>
    //[SerializeField] private bool displayIsPressed = false;
    [SerializeField] private GameObject bullet; //prefab de la bala
    [SerializeField] private float posMod; //modificador posición de instancia de la bala


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    private GameObject newBullet;
    private Movement playerMovement;
    private bool noBulletInGame = true;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour
    void Awake()
    {
        playerMovement = GetComponent<Movement>();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.FireWasPressedThisFrame())
        {
            //Debug.Log($"{Time.frameCount}[{Time.deltaTime}]: Fire was pressed this frame");
            Vector2 lastDir = playerMovement.GetLastDir();
            
            if (noBulletInGame) //dispara si el player no está mirando a un obstáculo adyacente y no hay otra bala instanciada
            {
                Quaternion bulletRotation;
                Vector3 instancePos = transform.position; //posición en la que se instancia la bala

                if (lastDir.x > 0) { instancePos.x += posMod; bulletRotation = Quaternion.Euler(0, 0, 90); } //derecha
                else if (lastDir.x < 0) { instancePos.x -= posMod; bulletRotation = Quaternion.Euler(0, 0, -90); } //izquierda
                else if (lastDir.y > 0) { instancePos.y += posMod; bulletRotation = Quaternion.Euler(0, 0, 180); } //arriba
                else { instancePos.y -= posMod; bulletRotation = Quaternion.Euler(0, 0, 0); } //abajo

                newBullet = Instantiate(bullet, instancePos, bulletRotation);
                noBulletInGame = false;
            }
        }
        if (newBullet == null) noBulletInGame = true;

        /*
        if (InputManager.Instance.FireWasReleasedThisFrame())
            Debug.Log($"{Time.frameCount}[{Time.deltaTime}]: Fire was released this frame");

        
        if (displayIsPressed && InputManager.Instance.FireIsPressed())
        {
            Debug.Log($"{Time.frameCount}[{Time.deltaTime}]: Fire was pressed");
            
        }
        */
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos



    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion
} // class TestFire 
// namespace
