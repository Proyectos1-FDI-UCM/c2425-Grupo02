//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Pablo Cayuela de la Fuente
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Permite salir de la escena de créditos, usando el tiempo o presionando x botón
/// </summary>
public class Credits : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Variables del timer
    /// </summary>
    private float _animtime = 0f;
    private float _animlim = 29f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.PauseWasPressedThisFrame() || _animtime>= _animlim)
        {
            WaitToEnd();
            _animtime = 0;
        }
        else
        {
            _animtime += Time.deltaTime;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Carga la escena Main_Menu
    /// </summary>
    private void WaitToEnd()
    {
        SceneManager.LoadScene("Main_Menu");
    }
    #endregion   

} // class Credits 
// namespace
