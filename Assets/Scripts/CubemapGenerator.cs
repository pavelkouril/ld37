﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubemapGenerator : ScriptableWizard
{
    public Transform renderPosition;
    public Cubemap cubemap;

    void OnWizardUpdate()
    {
        helpString = "Select transform to render" +
            " from and cubemap to render into";
        if (renderPosition != null && cubemap != null)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
    }

    void OnWizardCreate()
    {
        GameObject go = new GameObject("CubeCam", typeof(Camera));

        go.transform.position = renderPosition.position;
        go.transform.rotation = Quaternion.identity;
        go.GetComponent<Camera>().RenderToCubemap(cubemap);

        DestroyImmediate(go);
    }

    [MenuItem("Zgragselus/Render Cubemap")]
    static void RenderCubemap()
    {
        ScriptableWizard.DisplayWizard("Render CubeMap", typeof(CubemapGenerator), "Render!");
    }
}
