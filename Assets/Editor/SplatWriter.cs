using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class SplatWriter : ScriptableWizard
{
    public Texture2D textureToSplatify;

    [MenuItem("Terrain/Splat Writer")]

    static void createWizard()
    {

        ScriptableWizard.DisplayWizard("Select texture to splatify", typeof(SplatWriter), "Splatify");

    }

    void OnWizardCreate()
    {

        Texture2D splatTexture;
        splatTexture = (Texture2D)Selection.activeObject;

        if (splatTexture == null)
        {
            Debug.Log("Apparently your selection is either void, of a format that can't be cast as texture2D, or who knows what. Aborting...");
            return;
        }

        if (splatTexture.width != textureToSplatify.width || splatTexture.height != textureToSplatify.height)
        {
            Debug.Log("The splat texture, and the texture to splatify, differs in size! Aborting...");
            return;

        }

        Color[] theseColors = textureToSplatify.GetPixels(0, 0, textureToSplatify.width, textureToSplatify.height);
        Color[] theseColorsSplat = splatTexture.GetPixels(0, 0, splatTexture.width, splatTexture.height);

        for (int i = 0; i < theseColors.Length; i++)
        {


            float alphaComponent = 1 - (theseColors[i].r + theseColors[i].g + theseColors[i].b);
            theseColors[i].a = alphaComponent;

            theseColorsSplat[i] = theseColors[i];

        }

        splatTexture.SetPixels(theseColorsSplat);
        splatTexture.Apply();

        Debug.Log("Done. Succes?");
    }
}
