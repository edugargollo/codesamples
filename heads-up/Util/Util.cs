using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

    public static bool isInLayerMask(GameObject go, LayerMask mask)
    {
        return (mask.value & (1 << go.layer)) > 0;
    }

    
}
