using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    private static Matrix4x4 isometricMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIsometric(this Vector3 input) => isometricMatrix.MultiplyPoint3x4(input);
}
