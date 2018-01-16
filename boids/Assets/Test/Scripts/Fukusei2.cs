using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fukusei2 : MonoBehaviour {
    [SerializeField] Material m_mat;
    [SerializeField] MeshFilter m_meshFilter;
    private Mesh m_mesh;
    
    private List<Vector3> positions;
    private List<Vector3> targets;
    private List<Color> colors;
    
    public int num = 3000;

    private MaterialPropertyBlock propertyBlock;
    
    void Start ()
    {
        propertyBlock = new MaterialPropertyBlock();
        m_mesh = m_meshFilter.mesh;
        
        positions = new List<Vector3>();
        targets = new List<Vector3>();
        colors = new List<Color>();
        
        for (int i = 0; i < num; i++){
            positions.Add( new Vector3(Random.Range(-50f, 50f),0,Random.Range(-50f, 50f)) );
            targets.Add( new Vector3() );
        }

        for (int i = 0; i < 8; i++)
        {
            colors.Add(new Color(Random.value,Random.value,Random.value,1f));
        }
    }



    private void Update()
    {



        //マウスが押されたら、適当にターゲット位置を入れ替える
        if (Input.GetMouseButtonDown(0))
        {
            var flag = Random.value < 0.5f ? true : false;
            var flag2 =  Random.value < 0.3f ? true : false;
            Debug.Log("mousedown");
            for (int i = 0; i < num; i++)
            {
                var t = targets[i];
                if (flag)
                {
                    t.x = (i % 8f - 3.5f) * 10f + 10.5f * (Random.value - 0.5f); // 
                    t.z = Random.Range(-50f, 50f);
                }
                else
                {
                    //t.x = Random.Range(-50f, 50f);
                    //t.z = Random.Range(-50f, 50f);

                    //Debug.Log("au");
                    var pp = ImgPos.GetRandomPoint();
                    t.x = -pp.x / 2f;
                    t.z = -pp.y / 2f;

                    if (Random.value < 0.3f)
                    {
                        t.x = Random.Range(-50f, 50f);
                        t.z = Random.Range(-50f, 50f);                        
                    }
                }

                t.y = 0;//Random.Range(-0.1f, 0.1f);
                

                if (flag2)
                {
                    t.x = t.z = 0;
                }
                
                targets[i] = t;
            }
        }

        //足をじたばたする2コマぶんの計算
        var n = (Mathf.FloorToInt(Time.frameCount * 0.2f) % 2);

        for (int i = 0; i < num; i++)
        {
            //位置の更新            
            var p = positions[i];
            
            var t = targets[i];
            var v = t - p;
            v = v / 20f;

            var lim = 0.1f + 0.05f * ((float) i / (float) num);
            if (v.magnitude > lim)
            {
                v = v.normalized * lim;
            }
            p += v;
            positions[i] = p;
            
            //uniformを1個ずつ指定
            propertyBlock.SetFloat("_DT", i / 1000f);
            propertyBlock.SetColor("_Col", colors[i % colors.Count]);

            var q = Quaternion.Euler(
                new Vector3(
                    0,
                    ( -Mathf.Atan2(v.z,v.x) + Mathf.PI/2f ) / Mathf.PI*180f,
                    0
                )
            );
            
            //描画
            Graphics.DrawMesh(m_mesh, p, q, m_mat, 0, null, 0, propertyBlock);


            //var mtx = new Matrix4x4();
           // var mtx = Matrix4x4.Translate(p);
            //mtx.tr
            //Graphics.DrawMeshInstanced(m_mesh,0,m_mat,mtx,propertyBlock,true,false);
            
        }

    }


}