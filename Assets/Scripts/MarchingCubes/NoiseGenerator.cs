using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    ComputeBuffer _weightsBuffer;
    public ComputeShader NoiseShader;

    [SerializeField] float noiseScale = 0.08f;
    [SerializeField] float amplitude = 8;
    [SerializeField] float frequency = 0.004f;
    [SerializeField] int octaves = 6;
    [SerializeField, Range(0f, 1f)] float groundPercent = 0.5f;


    private void Awake() {
        CreateBuffers();
    }

    private void OnDestroy() {
        ReleaseBuffers();
    }

    public float[] GetNoise() {
        float[] noiseValues =
            new float[GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk];

        NoiseShader.SetBuffer(0, "_Weights", _weightsBuffer);

        NoiseShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        NoiseShader.SetFloat("_NoiseScale", noiseScale);
        NoiseShader.SetFloat("_Amplitude", amplitude);
        NoiseShader.SetFloat("_Frequency", frequency);
        NoiseShader.SetInt("_Octaves", octaves);
        NoiseShader.SetFloat("_GroundPercent", groundPercent);


        int tempNoise = GridMetrics.PointsPerChunk / GridMetrics.NumThreads;
        NoiseShader.Dispatch(
            0, tempNoise, tempNoise, tempNoise
        );

        _weightsBuffer.GetData(noiseValues);
        // var rnd = new System.Random();
        // for (int i = 0; i < noiseValues.Length; i++) {
        //     if (i <= 143){
        //         noiseValues[i] = -2;
        //     }
        //     else {
        //         noiseValues[i] = 2;
        //     }
        // }

        //Slanted Plane
        // int marchCubevar = 0;
        // for(int x = 1; x<= Mathf.Pow(noiseValues.Length, 1f/3f); x++){
        //     for(int y = 1; y<= Mathf.Pow(noiseValues.Length, 1f/3f); y ++){
        //         for(int z = 1; z<= Mathf.Pow(noiseValues.Length, 1f/3f); z ++){
        //             if (x >= 4 && x <= 9){
        //                 if(y == z){
        //                     noiseValues[marchCubevar] = 2;
        //                 }
        //                 else{
        //                     noiseValues[marchCubevar] = -2;
        //                 }
        //             }
        //             else{
        //                 noiseValues[marchCubevar] = -2;
        //             }
        //             marchCubevar += 1;
        //         }
        //     }
        // }

        //low poly Sphere
        // int marchCubevar = 0;
        // for(int x = 1; x <= 12; x++){
        //     for(int y = 1; y <= 12; y++){
        //         for(int z = 1; z <= 12; z++){
        //             if (x != 1 && x != 12 && ((x-6.5)*(x-6.5)) + ((y-6.5)*(y-6.5)) + ((z-6.5)*(z-6.5)) <= 25){
        //                 noiseValues[marchCubevar] = 2;
        //             }
        //             else{
        //                 noiseValues[marchCubevar] = -2;
        //             }
        //             marchCubevar += 1;
        //         }
        //     }
        // }

        // Half Pipe
        // int marchCubevar = 0;
        // for(int x = 1; x <= 12; x++){
        //     for(int y = 1; y <= 12; y++){
        //         for(int z = 1; z <= 12; z++){
        //             if (x != 1 && x != 12 && ((y-6.5)*(y-6.5)) + ((z-6.5)*(z-6.5)) >= 25){
        //                 noiseValues[marchCubevar] = 2;
        //             }
        //             else{
        //                 noiseValues[marchCubevar] = -2;
        //             }
        //             marchCubevar += 1;
        //         }
        //     }
        // }

        // Parabola
        // int marchCubevar = 0;
        // for(int x = 1; x <= 12; x++){
        //     for(int y = 1; y <= 12; y++){
        //         for(int z = 1; z <= 12; z++){
        //             if (x != 1 && x != 12 && y != 1 && y != 12 && z != 1 && z != 12 && -0.3*((z-6.5)*(z-6.5)) + 11>= y && -0.3*((x-6.5)*(x-6.5)) + 11>= y){
        //                 noiseValues[marchCubevar] = 2;
        //             }
        //             else{
        //                 noiseValues[marchCubevar] = -2;
        //             }
        //             marchCubevar += 1;
        //         }
        //     }
        // }
        return noiseValues;
    }

    void CreateBuffers() {
        _weightsBuffer = new ComputeBuffer(
            GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk, sizeof(float)
        );
    }

    void ReleaseBuffers() {
        _weightsBuffer.Release();
    }
}
