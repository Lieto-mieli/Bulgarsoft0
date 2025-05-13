using SuperTiled2Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SuperCustomPropertiesExtensions
{
    public static bool HasProperty(this SuperCustomProperties props, string key)
    {
        if (props.m_Properties == null) return false;
        foreach (var prop in props.m_Properties)
        {
            if (prop.m_Name == key)
                return true;
        }
        return false;
    }

    public static int GetInt(this SuperCustomProperties props, string key)
    {
        if (props.m_Properties == null) return 0;
        foreach (var prop in props.m_Properties)
        {
            if (prop.m_Name == key)
            {
                if (int.TryParse(prop.m_Value, out int result))
                    return result;
            }
        }
        return 0;
    }
}
