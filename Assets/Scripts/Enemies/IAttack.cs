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
/// Interfaz de ataque de los enemigos. Sirve para que varios enemigos, 
/// con scripts de ataque distintos, puedan heredar de la misma clase
/// </summary>
public interface IAttack
{
    // ---- MÉTODOS ----
    #region Métodos

    /// <summary>
    /// Corrutina de ataque (presente en el script de ataque) del enemigo
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack();

    #endregion   

} // class IAttack 
// namespace
