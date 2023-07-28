using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSample : MonoBehaviour
{
    //直方体の数
        const int slices =2;

    // Start is called before the first frame update
    void Start()
    {
        //はじめの位置
        float[] x = new float[slices * 4 + 4 ];
        float[] y = new float[slices * 4 + 4 ];
        float[] z = new float[slices * 4 + 4 ];

        x[0]= 0;
        x[1]= 0;
        x[2]= 0;
        x[3]= 0;
        x[4]= 10.0f;
        x[5]= 10.0f;
        x[6]= 10.0f;
        x[7]= 10.0f;
        x[8]= 20.0f;
        x[9]= 20.0f;
        x[10]= 20.0f;
        x[11]= 20.0f;
        

        y[0]= 0;
        y[1]= 10.0f;
        y[2]= 0;
        y[3]= 10.0f;
        y[4]= 0;
        y[5]= 10.0f;
        y[6]= 0;
        y[7]= 10.0f;
        y[8]= 0;
        y[9]= 10.0f;
        y[10]= 0;
        y[11]= 10.0f;


        z[0]= 0;
        z[1]= 0;
        z[2]= 10.0f;
        z[3]= 10.0f;
        z[4]= 0;
        z[5]= 0;
        z[6]= 10.0f;
        z[7]= 10.0f;
        z[8]= 0;
        z[9]= 0;
        z[10]= 10.0f;
        z[11]= 10.0f;

        //頂点の数
        int nVertices = 16 * slices +16 ;

        //メッシュの頂点の位置配列
        Vector3[] myVertices = new Vector3[nVertices];

        //メッシュの頂点の方線の配列
        Vector3[] myNormals = new Vector3[nVertices];


        for(int i=0;i <= slices; ++i)
        {
            //左からi番目の四角形の左下の頂点番号
            int pi = i * 16;

            int oi = i * 4;

            //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
            myVertices[pi].Set(x[oi],y[oi],z[oi]);
            

            //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
            myVertices[pi+1].Set(x[oi+1],y[oi+1],z[oi+1]);
            

            //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
            myVertices[pi+2].Set(x[oi+2],y[oi+2],z[oi+2]);
           

            //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
            myVertices[pi+3].Set(x[oi+3],y[oi+3],z[oi+3]);
            

            //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
            myVertices[pi+4].Set(x[oi],y[oi],z[oi]);
            

            //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
            myVertices[pi+5].Set(x[oi+1],y[oi+1],z[oi+1]);
            

            //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
            myVertices[pi+6].Set(x[oi+2],y[oi+2],z[oi+2]);
            

            //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
            myVertices[pi+7].Set(x[oi+3],y[oi+3],z[oi+3]);   


            //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
            myVertices[pi+8].Set(x[oi],y[oi],z[oi]);
            

            //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
            myVertices[pi+9].Set(x[oi+1],y[oi+1],z[oi+1]);
            

            //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
            myVertices[pi+10].Set(x[oi+2],y[oi+2],z[oi+2]);
            

            //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
            myVertices[pi+11].Set(x[oi+3],y[oi+3],z[oi+3]);  

            //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
            myVertices[pi+12].Set(x[oi],y[oi],z[oi]);
            

            //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
            myVertices[pi+13].Set(x[oi+1],y[oi+1],z[oi+1]);
            

            //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
            myVertices[pi+14].Set(x[oi+2],y[oi+2],z[oi+2]);
            

            //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
            myVertices[pi+15].Set(x[oi+3],y[oi+3],z[oi+3]); 

        }
            



        //三角形の頂点の数
        int nTriangles = slices * 36 + 6;
        //三角形のデータ
        int[] myTriangles = new int[nTriangles];

        //三角形の頂点の番号を求める
        for (int i=0; i< slices; ++i)
        {
            //左からi番目の四角形の左下の頂点番号の収納先
            int fi = i*36;
            //左からi番目の四角形の左下の頂点番号
            int pi = i * 16;

            //1.1つ目の三角形の頂点番号(正面)
            myTriangles[fi]=pi;
            myTriangles[fi+1]=pi+1;
            myTriangles[fi+2]=pi+16;

            //1.2つ目の三角形の頂点番号(正面)
            myTriangles[fi+3]=pi+16;
            myTriangles[fi+4]=pi+1;
            myTriangles[fi+5]=pi+17;

             //2.1つ目の三角形の頂点番号(裏側)
            myTriangles[fi+6]=pi+18;
            myTriangles[fi+7]=pi+19;
            myTriangles[fi+8]=pi+2;

             //2.2つ目の三角形の頂点番号(裏側)
            myTriangles[fi+9]=pi+2;
            myTriangles[fi+10]=pi+19;
            myTriangles[fi+11]=pi+3;

             //3.1つ目の三角形の頂点番号(上から)
            myTriangles[fi+12]=pi+5;
            myTriangles[fi+13]=pi+7;
            myTriangles[fi+14]=pi+21;

             //3.2つ目の三角形の頂点番号(上から)
            myTriangles[fi+15]=pi+21;
            myTriangles[fi+16]=pi+7;
            myTriangles[fi+17]=pi+23;

             //4.1つ目の三角形の頂点番号(下から)
            myTriangles[fi+18]=pi+6;
            myTriangles[fi+19]=pi+4;
            myTriangles[fi+20]=pi+22;

             //4.2つ目の三角形の頂点番号(下から)
            myTriangles[fi+21]=pi+22;
            myTriangles[fi+22]=pi+4;
            myTriangles[fi+23]=pi+20;

             //5.1つ目の三角形の頂点番号(左から)
            myTriangles[fi+24]=pi+10;
            myTriangles[fi+25]=pi+11;
            myTriangles[fi+26]=pi+8;

             //5.2つ目の三角形の頂点番号(左から)
            myTriangles[fi+27]=pi+8;
            myTriangles[fi+28]=pi+11;
            myTriangles[fi+29]=pi+9;

             //6.1つ目の三角形の頂点番号(右から)
            myTriangles[fi+30]=pi+12;
            myTriangles[fi+31]=pi+13;
            myTriangles[fi+32]=pi+14;

             //6.2つ目の三角形の頂点番号(右から)
            myTriangles[fi+33]=pi+14;
            myTriangles[fi+34]=pi+13;
            myTriangles[fi+35]=pi+15;
        }

        //左からi番目の四角形の左下の頂点番号の収納先
        int ti = (slices-1) *36;
        //左からi番目の四角形の左下の頂点番号
        int bi = (slices-1) * 16;

        //右から見た時の四角形の頂点番号
        myTriangles[ti+36]=bi+28;
        myTriangles[ti+37]=bi+29;
        myTriangles[ti+38]=bi+30;

        myTriangles[ti+39]=bi+30;
        myTriangles[ti+40]=bi+29;
        myTriangles[ti+41]=bi+31;


        
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