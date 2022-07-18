using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public int pixWidth;
    public int pixHeight;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    public float scale = 20;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        noiseTex.filterMode = FilterMode.Point;
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;
        CalcNoise();
    }

    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = x - noiseTex.width / 2;
                float yCoord = y - noiseTex.height / 2;
                float landscape = (Perlin.Fbm(xCoord / 400, yCoord / 400, 5) + 1.0f) / 2;

                pix[(int)y * noiseTex.width + (int)x] = new Color(landscape, landscape, landscape);

                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    void Update()
    {
        //CalcNoise();
    }

}
