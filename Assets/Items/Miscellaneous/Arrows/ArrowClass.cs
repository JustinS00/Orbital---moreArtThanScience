using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrow Class", menuName = "Item/Miscellaneous/Arrow")]
public class ArrowClass :  MiscClass {
    
    public ArrowType arrowType;
    public enum ArrowType {wooden, fire};

    public int damage;
}
