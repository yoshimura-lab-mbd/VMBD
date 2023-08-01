using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MeshSample : MonoBehaviour
{
    //CSVファイル
    private TextAsset _csvFile;   
    //CSVファイルの中身を入れるリスト
    private List<string[]> _csvData = new List<string[]>();  
    //断面数
    public int section;
    int slices;
    private float a;
    private float b;
    private int height = 0;
    int i,j,k,l,n,m;
    public bool counter_one = false;
    public bool counter_two = false;
    public bool counter_three = false;
    public bool counter_fouth = false;
    float elapseTime;
    int step = 1;

    public float[,,] x;
    public float[,,] y;
    public float[,,] z;
    float[] time;
    Vector3[] myVertices;
    Vector3[] myNormals;
    int[] myTriangles;
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Slider slider;
    int vvv;


    // Start is called before the first frame update
    void Start()
    {
      
    }
        

    // Update is called once per frame
    void Update()
    {  
        //csv 取り込み
       if(counter_one == false && counter_two == false)
       {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    //Resourceにある指定のパスのCSVファイルを格納
                    _csvFile = Resources.Load("data") as TextAsset; 

                    //TextAssetをStringReaderに変換
                    StringReader reader = new StringReader(_csvFile.text);      

                    while(reader.Peek() != -1)
                    {  
                    //１行ずつ読む             
                    string line = reader.ReadLine(); 
                    //読みこんだDataをリストにAddする   
                    _csvData.Add(line.Split(','));
                    height++;              
                }
                //断面数
                section = int.Parse(_csvData[0][3]);
                //断面の縦長さ
                a = 100f*float.Parse(_csvData[0][4]);
                //断面の横長さ
                b = 100f*float.Parse(_csvData[0][5]);
                //時間配列.[i]にはステップ数が入りその時の時間が収納されている
                time = new float[height-2];

                for(i=0; i<=height-3; i++)
                {
                    time[i]=float.Parse(_csvData[i+2][0]);
                }

                //断面中心点のx座標.[k][j]で,kには時間,jには断面の番号
                float[,] centerx = new float[height-2,section];
                //断面中心点のy座標.[k][j]で,kには時間,jには断面の番号
                float[,] centery = new float[height-2,section];
                //断面中心点のz座標.[k][j]で,kには時間,jには断面の番号
                float[,] centerz = new float[height-2,section];
                //断面のクォータニオン要素0.[k][j]で,kには時間,jには断面の番号
                float[,] q0 = new float[height-2,section];
                //断面のクォータニオン要素1.[k][j]で,kには時間,jには断面の番号
                float[,] q1 = new float[height-2,section];
                //断面のクォータニオン要素2.[k][j]で,kには時間,jには断面の番号
                float[,] q2 = new float[height-2,section];
                //断面のクォータニオン要素3.[k][j]で,kには時間,jには断面の番号
                float[,] q3 = new float[height-2,section];

                for(j=0; j<= height-3; j++)
                {
                    for(k=0;k<=section-1; k++)
                        {
                            centerx[j,k] =float.Parse(_csvData[j+2][k*7+1]);
                            centery[j,k] =float.Parse(_csvData[j+2][k*7+2]);
                            centerz[j,k] =float.Parse(_csvData[j+2][k*7+3]);
                            q0[j,k] =float.Parse(_csvData[j+2][k*7+4]);
                            q1[j,k] =float.Parse(_csvData[j+2][k*7+5]);
                            q2[j,k] =float.Parse(_csvData[j+2][k*7+6]);
                            q3[j,k] =float.Parse(_csvData[j+2][k*7+7]);
                        }
                }

                //最終的に求めたい慣性系で観測した頂点の座標([a][b][c]はそれぞれaがステップ数、bがどこの断面か、cが頂点のポイント)
                x = new float [height-2,section,4];
                y = new float [height-2,section,4];
                z = new float [height-2,section,4];

                //ボディー座標系で観測した慣性系からボディー座標系へのベクトル
                Vector3 vv = new Vector3(0.00f,0.00f,0.00f);

                //ボディー座標系で観測した頂点へのベクトル
                Vector3 v0 = new Vector3(0.00f,a/2,-b/2);
                Vector3 v1 = new Vector3(0.00f,a/2,b/2);
                Vector3 v2 = new Vector3(0.00f,-a/2,-b/2);
                Vector3 v3 = new Vector3(0.00f,-a/2,b/2);

                //読み込んだクォータニオンの要素を代入するために使う
                Quaternion q = new Quaternion(0.0f,0.0f,0.0f,1.0f);

                //慣性系で観測した中心点から頂点へのベクトル
                Vector3 newv0 = new Vector3(0.00f,0.00f,0.00f);
                Vector3 newv1 = new Vector3(0.00f,0.00f,0.00f);
                Vector3 newv2 = new Vector3(0.00f,0.00f,0.00f);
                Vector3 newv3 = new Vector3(0.00f,0.00f,0.00f);

                for(l=0; l<=height-3; l++)
                {
                    for(n=0; n<=section-1; n++)
                        {   
                            //ボディー座標系で観測した慣性系からボディー座標系へのベクトルを再計算
                            vv.x=centerx[l,n];
                            vv.y=centery[l,n];
                            vv.z=centerz[l,n];
                            //ボディー座標系から慣性系へ移すクォータニオンを再計算
                            q.x=q1[l,n];
                            q.y=q2[l,n];
                            q.z=q3[l,n];
                            q.w=q0[l,n];
                            //慣性系で観測した中心点から角頂点へのベクトルを計算
                            newv0= q * v0;
                            newv1= q * v1;
                            newv2= q * v2;
                            newv3= q * v3;
                            //慣性系で記述した慣性系からボディー座標系へのベクトル(point 0)
                            x[l,n,0]=100*(vv.x + newv0.x);
                            y[l,n,0]=100*(vv.y + newv0.y);
                            z[l,n,0]=100*(vv.z + newv0.z);
                            //慣性系で記述した慣性系からボディー座標系へのベクトル(point 1)
                            x[l,n,1]=100*(vv.x + newv1.x);
                            y[l,n,1]=100*(vv.y + newv1.y);
                            z[l,n,1]=100*(vv.z + newv1.z);
                            //慣性系で記述した慣性系からボディー座標系へのベクトル(point 2)
                            x[l,n,2]=100*(vv.x + newv2.x);
                            y[l,n,2]=100*(vv.y + newv2.y);
                            z[l,n,2]=100*(vv.z + newv2.z);
                            //慣性系で記述した慣性系からボディー座標系へのベクトル(point 3)
                            x[l,n,3]=100*(vv.x + newv3.x);
                            y[l,n,3]=100*(vv.y + newv3.y);
                            z[l,n,3]=100*(vv.z + newv3.z);
                        }
                }

                //直方体の数
                slices = section-1 ;

                //頂点の数
                int nVertices = 16 * slices +16 ;

                //メッシュの頂点の位置配列
                myVertices = new Vector3[nVertices];

                //メッシュの頂点の方線の配列
                myNormals = new Vector3[nVertices];

                //頂点の設定
                for(int i=0;i <= slices; ++i)
                {
                    //左からi番目の四角形の左下の頂点番号
                    int pi = i * 16;

                    //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
                    myVertices[pi].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
            

                    //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
                    myVertices[pi+1].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
            

                    //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
                    myVertices[pi+2].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
           

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
                    myVertices[pi+3].Set(x[0,i,3],y[0,i,3],z[0,i,3]);
            

                    //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
                    myVertices[pi+4].Set(x[0,i,0],y[0,i,0],z[0,i,0]);


                    //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
                    myVertices[pi+5].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
            

                    //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
                    myVertices[pi+6].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
            

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
                    myVertices[pi+7].Set(x[0,i,3],y[0,i,3],z[0,i,3]);   


                    //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
                    myVertices[pi+8].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
            

                    //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
                    myVertices[pi+9].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
            

                    //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
                    myVertices[pi+10].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
            

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
                    myVertices[pi+11].Set(x[0,i,3],y[0,i,3],z[0,i,3]);  

                    //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
                    myVertices[pi+12].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
            

                    //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
                    myVertices[pi+13].Set(x[0,i,1],y[0,i,1],z[0,i,1]);


                    //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
                    myVertices[pi+14].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
            

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
                    myVertices[pi+15].Set(x[0,i,3],y[0,i,3],z[0,i,3]); 

                }
        
                //三角形の頂点の数
                int nTriangles = slices * 36 + 6;
                //三角形のデータ
                myTriangles = new int[nTriangles];

                //三角形の頂点の番号を求める
                for (int m=0; m< slices; ++m)
                {
                    //左からi番目の四角形の左下の頂点番号の収納先
                    int fi = m*36;
                    //左からi番目の四角形の左下の頂点番号
                    int pi = m * 16;

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
                slider.maxValue = height-3;

                counter_two = !counter_two;

                }   
            }
            //モード選択画面
            else if(counter_one ==false && counter_two == true)
            {
                text3.text ="chose the mode!";
                text1.text ="Space is 'slider mode'";
                text2.text ="Enter is 'animation mode'";
                //スライダー画面に変える
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    counter_one = !counter_one;
                }
                //アニメーション画面に移る
                else if(Input.GetKeyDown(KeyCode.Return))
                {
                    counter_one =!counter_one;
                    counter_two = !counter_two;
                    //時間を計測中にする
                    counter_three = !counter_three;
                }
            }

            //アニメーションモード
            else if (counter_one == true && counter_two == false)
            {
                text1.text="Space is to return 'chose the mode'!";
                text2.text="Enter is to stop or play animation!";
                //計測中
                if(counter_three == true)
                {
                    text3.text ="In animation";
                    elapseTime += Time.deltaTime;
                    text4.text ="Time is";
                    text5.text = elapseTime.ToString();

                    //時間になったらmeshを更新する
                    if(time[step] <= elapseTime)
                    {
                        for(int i=0;i <= slices; ++i)
                        {
                            //左からi番目の四角形の左下の頂点番号
                            int pi = i * 16;

                            //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
                            myVertices[pi].Set(x[step,i,0],y[step,i,0],z[step,i,0]);

                            //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
                            myVertices[pi+1].Set(x[step,i,1],y[step,i,1],z[step,i,1]);
            

                            //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
                            myVertices[pi+2].Set(x[step,i,2],y[step,i,2],z[step,i,2]);
           

                            //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
                            myVertices[pi+3].Set(x[step,i,3],y[step,i,3],z[step,i,3]);
            

                            //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
                            myVertices[pi+4].Set(x[step,i,0],y[step,i,0],z[step,i,0]);


                            //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
                            myVertices[pi+5].Set(x[step,i,1],y[step,i,1],z[step,i,1]);
                    

                            //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
                            myVertices[pi+6].Set(x[step,i,2],y[step,i,2],z[step,i,2]);
                    

                            //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
                            myVertices[pi+7].Set(x[step,i,3],y[step,i,3],z[step,i,3]);   


                            //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
                            myVertices[pi+8].Set(x[step,i,0],y[step,i,0],z[step,i,0]);
                    

                            //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
                            myVertices[pi+9].Set(x[step,i,1],y[step,i,1],z[step,i,1]);
                    

                            //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
                            myVertices[pi+10].Set(x[step,i,2],y[step,i,2],z[step,i,2]);
                    

                            //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
                            myVertices[pi+11].Set(x[step,i,3],y[step,i,3],z[step,i,3]);  

                            //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
                            myVertices[pi+12].Set(x[step,i,0],y[step,i,0],z[step,i,0]);
                    

                            //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
                            myVertices[pi+13].Set(x[step,i,1],y[step,i,1],z[step,i,1]);


                            //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
                            myVertices[pi+14].Set(x[step,i,2],y[step,i,2],z[step,i,2]);
                    

                            //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
                            myVertices[pi+15].Set(x[step,i,3],y[step,i,3],z[step,i,3]); 
                        }
                        Mesh myMesh = GetComponent<MeshFilter>().mesh;
                        myMesh.SetVertices(myVertices);
                        myMesh.RecalculateNormals();
                        step = step + 1;
                    }

                    //アニメーションが終わったらモード選択画面へ戻る
                    else if(step == height -3 )
                    {
                        for(int i=0;i <= slices; ++i)
                            {
                                //左からi番目の四角形の左下の頂点番号
                                int pi = i * 16;

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi].Set(x[0,i,0],y[0,i,0],z[0,i,0]);

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi+1].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
                                myVertices[pi+2].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                    

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
                                myVertices[pi+3].Set(x[0,i,3],y[0,i,3],z[0,i,3]);
                        

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+4].Set(x[0,i,0],y[0,i,0],z[0,i,0]);


                                //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
                                myVertices[pi+5].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+6].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
                                myVertices[pi+7].Set(x[0,i,3],y[0,i,3],z[0,i,3]);  


                                //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+8].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+9].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+10].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
                                myVertices[pi+11].Set(x[0,i,3],y[0,i,3],z[0,i,3]);   

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+12].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+13].Set(x[0,i,1],y[0,i,1],z[0,i,1]);


                                //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+14].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
                                myVertices[pi+15].Set(x[0,i,3],y[0,i,3],z[0,i,3]);  
                            }
                            Mesh myMesh = GetComponent<MeshFilter>().mesh;
                            myMesh.SetVertices(myVertices);
                            myMesh.RecalculateNormals();
                            step = 1;
                            elapseTime = 0.000000000f;
                            text4.text="";
                            text5.text="";
                            counter_one = !counter_one;
                            counter_two = !counter_two;
                            counter_three = !counter_three;
                        }
                    //enter で一時停止
                    else if(Input.GetKeyDown(KeyCode.Return))
                    {
                        counter_three =!counter_three;
                    }

                    //モード選択画面へ
                    else if(Input.GetKeyDown(KeyCode.Space))
                    {
                        for(int i=0;i <= slices; ++i)
                            {
                                //左からi番目の四角形の左下の頂点番号
                                int pi = i * 16;

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi].Set(x[0,i,0],y[0,i,0],z[0,i,0]);

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi+1].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
                                myVertices[pi+2].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                    

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
                                myVertices[pi+3].Set(x[0,i,3],y[0,i,3],z[0,i,3]);
                        

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+4].Set(x[0,i,0],y[0,i,0],z[0,i,0]);


                                //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
                                myVertices[pi+5].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+6].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
                                myVertices[pi+7].Set(x[0,i,3],y[0,i,3],z[0,i,3]);  


                                //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+8].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+9].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+10].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
                                myVertices[pi+11].Set(x[0,i,3],y[0,i,3],z[0,i,3]);   

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+12].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+13].Set(x[0,i,1],y[0,i,1],z[0,i,1]);


                                //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+14].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
                                myVertices[pi+15].Set(x[0,i,3],y[0,i,3],z[0,i,3]);

                            }
                            Mesh myMesh = GetComponent<MeshFilter>().mesh;
                            myMesh.SetVertices(myVertices);
                            myMesh.RecalculateNormals();
                            step = 0;
                            elapseTime = 0.000000000f;
                            text4.text="";
                            text5.text="";
                            counter_one =!counter_one;
                            counter_two =!counter_two;
                            counter_three =!counter_three;
                    }
                }

                else if(counter_three == false)
                {
                    text3.text="animation stopped";
                    if(Input.GetKeyDown(KeyCode.Return))
                        {
                            for(int i=0;i <= slices; ++i)
                            {
                                //左からi番目の四角形の左下の頂点番号
                                int pi = i * 16;

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi].Set(x[0,i,0],y[0,i,0],z[0,i,0]);

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi+1].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
                                myVertices[pi+2].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                    

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
                                myVertices[pi+3].Set(x[0,i,3],y[0,i,3],z[0,i,3]);
                        

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+4].Set(x[0,i,0],y[0,i,0],z[0,i,0]);


                                //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
                                myVertices[pi+5].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+6].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
                                myVertices[pi+7].Set(x[0,i,3],y[0,i,3],z[0,i,3]);  


                                //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+8].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+9].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+10].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
                                myVertices[pi+11].Set(x[0,i,3],y[0,i,3],z[0,i,3]);   

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+12].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+13].Set(x[0,i,1],y[0,i,1],z[0,i,1]);


                                //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+14].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
                                myVertices[pi+15].Set(x[0,i,3],y[0,i,3],z[0,i,3]);

                            }
                            Mesh myMesh = GetComponent<MeshFilter>().mesh;
                            myMesh.SetVertices(myVertices);
                            myMesh.RecalculateNormals();
                            counter_three =!counter_three;
                        }
                    else if(Input.GetKeyDown(KeyCode.Space))
                        {
                             for(int i=0;i <= slices; ++i)
                            {
                                //左からi番目の四角形の左下の頂点番号
                                int pi = i * 16;

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi].Set(x[0,i,0],y[0,i,0],z[0,i,0]);

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
                                myVertices[pi+1].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
                                myVertices[pi+2].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                    

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
                                myVertices[pi+3].Set(x[0,i,3],y[0,i,3],z[0,i,3]);
                        

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+4].Set(x[0,i,0],y[0,i,0],z[0,i,0]);


                                //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
                                myVertices[pi+5].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
                                myVertices[pi+6].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
                                myVertices[pi+7].Set(x[0,i,3],y[0,i,3],z[0,i,3]);  


                                //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+8].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+9].Set(x[0,i,1],y[0,i,1],z[0,i,1]);
                        

                                //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
                                myVertices[pi+10].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
                                myVertices[pi+11].Set(x[0,i,3],y[0,i,3],z[0,i,3]);   

                                //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+12].Set(x[0,i,0],y[0,i,0],z[0,i,0]);
                        

                                //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+13].Set(x[0,i,1],y[0,i,1],z[0,i,1]);


                                //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
                                myVertices[pi+14].Set(x[0,i,2],y[0,i,2],z[0,i,2]);
                        

                                //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
                                myVertices[pi+15].Set(x[0,i,3],y[0,i,3],z[0,i,3]);

                            }
                            Mesh myMesh = GetComponent<MeshFilter>().mesh;
                            myMesh.SetVertices(myVertices);
                            myMesh.RecalculateNormals();
                            step = 0;
                            elapseTime = 0.000000000f;
                            text4.text="";
                            text5.text="";
                            counter_one =!counter_one;
                            counter_two= !counter_two;
                        }
                }
            }
            
            //スライダーモード
            else if(counter_one == true && counter_two == true)
            {
                text3.text ="Slider mode";
                text1.text ="Move the slider!";
                text2.text ="Enter is to return 'chose mode'";

                if(Input.GetKeyDown(KeyCode.Return))
                {
                            counter_one =!counter_one;
                }

                vvv= (int)slider.value;
                for(int i=0;i <= slices; ++i)
                {
                    //左からi番目の四角形の左下の頂点番号
                    int pi = i * 16;

                    //左からi番目の手前の四角形の左下の頂点の位置と法線(正面から見るのに使う)
                    myVertices[pi].Set(x[vvv,i,0],y[vvv,i,0],z[vvv,i,0]);

                    //左からi番目の手前の四角形の左上の頂点の位置と法線(正面から見るのに使う)
                    myVertices[pi+1].Set(x[vvv,i,1],y[vvv,i,1],z[vvv,i,1]);
            

                    //左からi番目の奥の四角形の左下の頂点の位置と法線(裏側から見るのに使う)
                    myVertices[pi+2].Set(x[vvv,i,2],y[vvv,i,2],z[vvv,i,2]);
           

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(裏側から見るに使う)
                    myVertices[pi+3].Set(x[vvv,i,3],y[vvv,i,3],z[vvv,i,3]);
            

                    //左からi番目の手前の四角形の左下の頂点の位置と法線(下から見るのに使う)
                    myVertices[pi+4].Set(x[vvv,i,0],y[vvv,i,0],z[vvv,i,0]);


                    //左からi番目の手前の四角形の左上の頂点の位置と法線(上から見るのに使う)
                    myVertices[pi+5].Set(x[vvv,i,1],y[vvv,i,1],z[vvv,i,1]);
            

                    //左からi番目の奥の四角形の左下の頂点の位置と法線(下から見るのに使う)
                    myVertices[pi+6].Set(x[vvv,i,2],y[vvv,i,2],z[vvv,i,2]);
            

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(上から見るに使う)
                    myVertices[pi+7].Set(x[vvv,i,3],y[vvv,i,3],z[vvv,i,3]);  


                    //左からi番目の手前の四角形の左下の頂点の位置と法線(左から見るのに使う)
                    myVertices[pi+8].Set(x[vvv,i,0],y[vvv,i,0],z[vvv,i,0]);
            

                    //左からi番目の手前の四角形の左上の頂点の位置と法線(左から見るのに使う)
                   myVertices[pi+9].Set(x[vvv,i,1],y[vvv,i,1],z[vvv,i,1]);
            

                    //左からi番目の奥の四角形の左下の頂点の位置と法線(左から見るのに使う)
                    myVertices[pi+10].Set(x[vvv,i,2],y[vvv,i,2],z[vvv,i,2]);
            

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(左から見るに使う)
                    myVertices[pi+11].Set(x[vvv,i,3],y[vvv,i,3],z[vvv,i,3]);   

                    //左からi番目の手前の四角形の左下の頂点の位置と法線(右から見るのに使う)
                    myVertices[pi+12].Set(x[vvv,i,0],y[vvv,i,0],z[vvv,i,0]);
            

                    //左からi番目の手前の四角形の左上の頂点の位置と法線(右から見るのに使う)
                    myVertices[pi+13].Set(x[vvv,i,1],y[vvv,i,1],z[vvv,i,1]);


                    //左からi番目の奥の四角形の左下の頂点の位置と法線(右から見るのに使う)
                    myVertices[pi+14].Set(x[vvv,i,2],y[vvv,i,2],z[vvv,i,2]);
            

                    //左からi番目の奥の四角形の左上の頂点の位置と法線(右から見るに使う)
                    myVertices[pi+15].Set(x[vvv,i,3],y[vvv,i,3],z[vvv,i,3]);

                }
                    text4.text="Time is";
                    text5.text=time[vvv].ToString();
                    Mesh myMesh = GetComponent<MeshFilter>().mesh;
                    myMesh.SetVertices(myVertices);
                    myMesh.RecalculateNormals();
            }


    }

    public void Method()
    {
    float vvvv = slider.value;
    vvv = (int)vvvv;
    }

}


