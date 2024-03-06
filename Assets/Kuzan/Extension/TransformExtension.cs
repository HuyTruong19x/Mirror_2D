using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    private static Vector3 _position;
    private static Vector3 _localPosition;
    private static Quaternion _quaternion;
    private static Quaternion _localQuaternion;
    private static Vector3 _localScale;
    private static bool _isBackuped = false;

    public static void Backup(this Transform transform)
    {
        _isBackuped = true;
        _position = transform.position;
        _localPosition = transform.localPosition;
        _quaternion = transform.rotation;
        _localQuaternion = transform.localRotation;
        _localScale = transform.localScale;
    }

    public static void Recover(this Transform transform)
    {
        if(_isBackuped)
        {
            transform.position = _position;
            transform.rotation = _quaternion;
            transform.localScale = _localScale;
        }
        else
        {
            throw new System.Exception("You must backup before recovering");
        }
    }

    public static void ResetToOriginPosition(this Transform transform)
    {
        transform.position = _position;
    }
    public static Vector3 GetOriginPosition(this Transform transform)
    {
        return _position;
    }
    public static void ResetToOriginLocalPosition(this Transform transform)
    {
        transform.localPosition = _localPosition;
    }
    public static Vector3 GetOriginLocalPosition(this Transform transform)
    {
        return _localPosition;
    }
    public static void ResetToOriginRotation(this Transform transform)
    {
        transform.rotation = _quaternion;
    }
    public static Quaternion GetOriginRotation(this Transform transform)
    {
        return _quaternion;
    }
    public static void ResetToOriginLocalRotation(this Transform transform)
    {
        transform.localRotation = _localQuaternion;
    }
    public static Quaternion GetOriginLocalRotation(this Transform transform)
    {
        return _localQuaternion;
    }
    public static void ResetToOriginLocalScale(this Transform transform)
    {
        transform.localScale = _localScale;
    }
    public static Vector3 GetOriginLocalScale(this Transform transform)
    {
        return _localScale;
    }
}
