using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

    /*
     * El layermask es un entero, y el layer también, con lo que tenemos que probar solo un bit.
     * Lo que hace este método es mover el bit que tenemos que comparar hacia la izquierda, el operador &
     * hace que todos los bits menos ese se pongan a cero. El valor en el entero es el valor del bit que queremos
     * probar. Si es cero, es falso que el layer esté en el mask, y en  caso contrario es cierto.
	*/
    public static bool isInLayerMask(GameObject go, LayerMask mask)
    {
        return (mask.value & (1 << go.layer))>0;
    }
}
