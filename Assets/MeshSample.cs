using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSample : MonoBehaviour
{
    //直方体の数
        const int slices =2;
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
        int nVertices = 12 * slices +12 ;

        //メッシュの頂点の位置配列
        Vector3[] myVertices = new Vector3[nVertices];

        //メッシュの頂点の方線の配列
        Vector3[] myNormals = new Vector3[nVertices];

        //長方形の高さ
        float y1=y0 + height;

        //長方形の奥行き
        float z1=z0 + depth;

        myVertices[0].Set(x0,y0,0);
        myVertices[1].Set(x0,y1,0);
        myVertices[2].Set(x0,y0,z1);
        myVertices[3].Set(x0,y1,z1);

        for(int i=0;i <= slices; ++i)
        {
            //横方向のパラメータ
            float u =(float)i/slices;
            //左からi番目の四角形の左下の頂点のx座標の値
            float xi = x0 + width * u;
            //左からi番目の四角形の左下の頂点番号
            int pi = i * 8;
            //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
            myVertices[pi+4].Set(xi,y0,0);
            

            //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
            myVertices[pi+5].Set(xi,y1,0);
            

            //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
            myVertices[pi+6].Set(xi,y0,z1);
           

            //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
            myVertices[pi+7].Set(xi,y1,z1);
            

            //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
            myVertices[pi+8].Set(xi,y0,0);
            

            //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
            myVertices[pi+9].Set(xi,y1,0);
            

            //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
            myVertices[pi+10].Set(xi,y0,z1);
            

            //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
            myVertices[pi+11].Set(xi,y1,z1);   
        }
            //パラメータ
            int ai= slices * 8;
            //横方向のパラメータ
            float q =slices /slices;
            //左からi番目の四角形の左下の頂点のx座標の値
            float xii = x0 + width * q;

            //一番右の四角形の右下の頂点(右から見るのに使う)
            myVertices[ai+12].Set(xii,y0,0);
            

            //一番右の四角形の右上の頂点(右から見るのに使う)
            myVertices[ai+13].Set(xii,y1,0);
            

            //一番右の四角形の奥右下の頂点(右から見るのに使う)
            myVertices[ai+14].Set(xii,y0,z1);
           

            //一番右の四角形のて奥右上の頂点(右から見るのに使う)
            myVertices[ai+15].Set(xii,y1,z1);



        //三角形の頂点の数
        int nTriangles = slices * 24 + 12;
        //三角形のデータ
        int[] myTriangles = new int[nTriangles];
        //左から見た時の四角形の頂点番号
        myTriangles[0]=2;
        myTriangles[1]=3;
        myTriangles[2]=0;

        myTriangles[3]=0;
        myTriangles[4]=3;
        myTriangles[5]=1;

        //三角形の頂点の番号を求める
        for (int i=0; i< slices; ++i)
        {
            //左からi番目の四角形の左下の頂点番号の収納先
            int fi = i*24;
            //左からi番目の四角形の左下の頂点番号
            int pi = i * 8 +4;

            //1.1つ目の三角形の頂点番号(正面)
            myTriangles[fi+6]=pi;
            myTriangles[fi+7]=pi+1;
            myTriangles[fi+8]=pi+8;

            //1.2つ目の三角形の頂点番号(正面)
            myTriangles[fi+9]=pi+8;
            myTriangles[fi+10]=pi+1;
            myTriangles[fi+11]=pi+9;

             //2.1つ目の三角形の頂点番号(裏側)
            myTriangles[fi+12]=pi+10;
            myTriangles[fi+13]=pi+11;
            myTriangles[fi+14]=pi+2;

             //2.2つ目の三角形の頂点番号(裏側)
            myTriangles[fi+15]=pi+2;
            myTriangles[fi+16]=pi+11;
            myTriangles[fi+17]=pi+3;

             //3.1つ目の三角形の頂点番号(上から)
            myTriangles[fi+18]=pi+5;
            myTriangles[fi+19]=pi+7;
            myTriangles[fi+20]=pi+13;

             //3.2つ目の三角形の頂点番号(上から)
            myTriangles[fi+21]=pi+13;
            myTriangles[fi+22]=pi+7;
            myTriangles[fi+23]=pi+15;

             //4.1つ目の三角形の頂点番号(下から)
            myTriangles[fi+24]=pi+6;
            myTriangles[fi+25]=pi+4;
            myTriangles[fi+26]=pi+14;

             //4.2つ目の三角形の頂点番号(下から)
            myTriangles[fi+27]=pi+14;
            myTriangles[fi+28]=pi+4;
            myTriangles[fi+29]=pi+12;
        }

        //左からi番目の四角形の左下の頂点番号の収納先
            int ti = (slices-1) *24;
            //左からi番目の四角形の左下の頂点番号
            int bi = (slices-1) * 8 +4;

        //右から見た時の四角形の頂点番号
        myTriangles[ti+30]=bi+16;
        myTriangles[ti+31]=bi+17;
        myTriangles[ti+32]=bi+18;

        myTriangles[ti+33]=bi+18;
        myTriangles[ti+34]=bi+17;
        myTriangles[ti+35]=bi+19;


        
        Mesh myMesh =new Mesh();

        myMesh.SetVertices(myVertices);
        myMesh.RecalculateNormals();
        myMesh.SetTriangles(myTriangles,0);
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh=myMesh;

       


 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
