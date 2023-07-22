using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSample : MonoBehaviour
{
    //断面の数
        const int slices =5;
        //はじめの位置
        const float x0 = -5f;
        const float y0 = -1f;
        const float z0 = -1f;

        //長方形の設定
        const float width = 10f;
        const float height = 2f;
        const float depth = 2f;
    // Start is called before the first frame update
    void Start()
    {
        //頂点の数
        int nVertices = 4 * slices +4 ;

        //メッシュの頂点の位置配列
        Vector3[] myVertices = new Vector3[nVertices];

        //メッシュの頂点の方線の配列
        Vector3[] myNormals = new Vector3[nVertices];

        //長方形の高さ
        float y1=y0 + height;

        //長方形の奥行き
        float z1=z0 + depth;

        for(int i=0;i <= slices; ++i)
        {
            //横方向のパラメータ
            float u =(float)i/slices;
            //左からi番目の四角形の左下の頂点のx座標の値
            float xi = x0 + width * u;
            //左からi番目の四角形の左下の頂点番号
            int pi = i * 4;
            //左からi番目の手前の四角形の左下の頂点の位置と法線
            myVertices[pi].Set(xi,y0,0);
            myNormals[pi] = Vector3.back;

            //左からi番目の手前の四角形の左上の頂点の位置と法線
            myVertices[pi+1].Set(xi,y1,0);
            myNormals[pi+1] = Vector3.back;

            //左からi番目の奥の四角形の左下の頂点の位置と法線
            myVertices[pi+2].Set(xi,y0,z1);
            myNormals[pi+2] = new Vector3(0,0,1);

            //左からi番目の奥の四角形の左上の頂点の位置と法線
            myVertices[pi+3].Set(xi,y1,z1);
            myNormals[pi+3] = new Vector3(0,0,1);
        }

        //三角形の頂点の数
        int nTriangles = slices * 30;
        //三角形のデータ
        int[] myTriangles = new int[nTriangles];
        //三角形の頂点の番号を求める
        for (int i=0; i< slices; ++i)
        {
            //左からi番目の四角形の左下の頂点番号の収納先
            int fi = i*30;
            //左からi番目の四角形の左下の頂点番号
            int pi = i*4;

            //1.1つ目の三角形の頂点番号
            myTriangles[fi+0]=pi;
            myTriangles[fi+1]=pi+1;
            myTriangles[fi+2]=pi+4;

            //1.2つ目の三角形の頂点番号
            myTriangles[fi+3]=pi+4;
            myTriangles[fi+4]=pi+1;
            myTriangles[fi+5]=pi+5;

             //2.1つ目の三角形の頂点番号
            myTriangles[fi+6]=pi;
            myTriangles[fi+7]=pi+2;
            myTriangles[fi+8]=pi+4;

             //2.2つ目の三角形の頂点番号
            myTriangles[fi+9]=pi+4;
            myTriangles[fi+10]=pi+2;
            myTriangles[fi+11]=pi+6;

             //3.1つ目の三角形の頂点番号
            myTriangles[fi+12]=pi+1;
            myTriangles[fi+13]=pi+3;
            myTriangles[fi+14]=pi+5;

             //3.2つ目の三角形の頂点番号
            myTriangles[fi+15]=pi+5;
            myTriangles[fi+16]=pi+3;
            myTriangles[fi+17]=pi+7;

             //4.1つ目の三角形の頂点番号
            myTriangles[fi+18]=pi;
            myTriangles[fi+19]=pi+2;
            myTriangles[fi+20]=pi+4;

             //4.2つ目の三角形の頂点番号
            myTriangles[fi+21]=pi+4;
            myTriangles[fi+22]=pi+2;
            myTriangles[fi+23]=pi+6;

             //5.1つ目の三角形の頂点番号
            myTriangles[fi+24]=pi+4;
            myTriangles[fi+25]=pi+5;
            myTriangles[fi+26]=pi+6;

             //5.2つ目の三角形の頂点番号
            myTriangles[fi+27]=pi+6;
            myTriangles[fi+28]=pi+5;
            myTriangles[fi+29]=pi+7;


        }

        Mesh myMesh =new Mesh();

        myMesh.SetVertices(myVertices);
        myMesh.SetNormals(myNormals);
        myMesh.SetTriangles(myTriangles,0);
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh=myMesh;


 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
