using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//noiseManager
public class TerrainManager : MonoBehaviour
{
    public RawImage noiseImage;
    public Terrain noiseTerrain;
    private float _scale;
    private float _lastScale;
    private Noise _noise;
    public int width = 256;
    public int height = 256;
    private void Awake()
    {
        _scale = 0.1f;
        _noise = new PerlinNoise();
        _RecomputeNoise();    
    }

    private void _RecomputeNoise(){
        float[,] noise = new float[width, height];

        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x++){
                noise[x,y] = _noise.GetNoiseMap(x, y, _scale);
            }
        }
        setnoise(noise);
    }
    private void _UpdateUI()
    {
        if (_scale == _lastScale) {
            return;
        }
        else{
            _RecomputeNoise();

            _lastScale = _scale;
        }
    }

    private void OnGUI()
    {
        _scale = GUI.HorizontalSlider(new Rect(120f, 0f, 100f, 20f), _scale, 0.01f, 0.3f);
        if (GUI.changed) {
            _UpdateUI();
        }
    }

    public void setnoise(float[,] noise){
        Color[] pixels = new Color[width * height];
        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x ++){
                pixels[x + width * y] = Color.Lerp(Color.black, Color.white, noise[y, x]);
            }
        }
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        noiseImage.texture = texture;
        noiseTerrain.terrainData.SetHeights(0, 0, noise);
    }
}
