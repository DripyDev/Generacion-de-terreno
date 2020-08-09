﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour {
    public int escalado;
    public float aumentoRuido;
    public int columnas;
    public int filas;
    //El ruido de perling va a usar este valor en lugar de x e y porque funciona mejor con un cambio mas leve que con numeros enteros
    public float comienzoRuido = 0.1f;
    //Mesh que vamos a crear y alterar con ruido perling
    private Mesh mesh;
    
    private List<Vector3> vertices;
    private int[] triangulos;

    void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //StartCoroutine(CreateShape());
        CreateShape();

        PrepararMesh();        
    }

    //IEnumerator CreateShape(){
    void CreateShape(){
        vertices = new List<Vector3>();
        float xR = comienzoRuido; float yR = comienzoRuido;
        
        for (int z = 0; z <= filas; z++) {
            for (int x = 0; x <= columnas; x++) {
                float y = Mathf.PerlinNoise(xR, yR) * escalado;
                //float y = Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 2;
                //NOTA: En unity la altura es Y asi que vamos a usar solo x y z
                vertices.Add(new Vector3(x, y, z));

                xR += aumentoRuido;
            }
            yR += aumentoRuido;
        }

        triangulos = new int[columnas * filas * 6];
        int triang = 0;
        int vert=0;
        for (int z = 0; z < filas; z++) {
            for (int x = 0; x < columnas; x++) {
                triangulos[triang + 0] = vert + 0;
                triangulos[triang + 1] = vert + columnas + 1;
                triangulos[triang + 2] = vert + 1;
                triangulos[triang + 3] = vert + 1;
                triangulos[triang + 4] = vert + columnas + 1;
                triangulos[triang + 5] = vert + columnas + 2;

                vert++;
                triang += 6;
            }
            vert++;
            //yield return new WaitForSeconds(0.1f);
        }
    }

    void PrepararMesh(){
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangulos;
        mesh.RecalculateNormals();
    }

    void Update() {
        if(Input.GetKeyDown("p")){
            CreateShape();
            PrepararMesh();
        }
    }

    public void SetAumentoRuido(float ruido){
        aumentoRuido = ruido;
    }

    public void SetEscaladoRuido(float escaladoRuido){
        escalado = (int) escaladoRuido;
    }

    public void SetColumnas(float col){
        columnas = (int) col;
    }

    public void SetFilas(float fil){
        filas = (int) fil;
    }

    void OnDrawGizmos(){
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Count; i++) {
            Gizmos.DrawSphere(vertices[i], 0.01f);
        }
    }
}