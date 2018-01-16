using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Fukusei3 : MonoBehaviour {
    
    [SerializeField] Material m_mat;
    [SerializeField] MeshFilter m_meshFilter;
    private Mesh m_mesh;
    
    private List<Vector3> positions;
    private List<Vector3> targets;
    private List<Vector4> colors;
    private Matrix4x4[] matrices;

    
    private int num = 1023;

    private MaterialPropertyBlock propertyBlock;
    
    void Start ()
    {
        propertyBlock = new MaterialPropertyBlock();
        m_mesh = m_meshFilter.mesh;
        
        positions = new List<Vector3>();
        targets = new List<Vector3>();
        colors = new List<Vector4>();
        matrices = new Matrix4x4[num];
        
        var dtList = new float[num];
        var colList = new Vector4[num];
        
        for (int i = 0; i < 2; i++)
        {
            colors.Add(new Vector4(Random.value,Random.value,Random.value,1f));
        }        
        
        for (int i = 0; i < num; i++){
            positions.Add( new Vector3(Random.Range(-50f, 50f),0,Random.Range(-50f, 50f)) );
            targets.Add( new Vector3() );
            matrices[i] = new Matrix4x4();

            dtList[i] = i / 1000f;
            colList[i] = colors[i % colors.Count];
        }
        
        propertyBlock.SetFloatArray("_DT",dtList);
        propertyBlock.SetVectorArray("_Col", colList);
    }

    public void ChangeTarget(int type)
    {
       
        for (int i = 0; i < num; i++)
        {
            var t = targets[i];

            if (type == 0)
            {
                t.x = (i % 8f - 3.5f) * 10f + 3.5f * (Random.value - 0.5f); // + 
                t.z = Random.Range(-50f, 50f);
            }else if (type == 1)
            {
                t.x = Random.Range(-50f, 50f);
                t.z = Random.Range(-50f, 50f);
            }
            else
            {
                var pp = ImgPos.GetRandomPoint();
                t.x = -pp.x / 2f;
                t.z = -pp.y / 2f;

                if (Random.value < 0.3f)
                {
                    t.x = Random.Range(-50f, 50f);
                    t.z = Random.Range(-50f, 50f);
                }
            }
            
            

            targets[i] = t;
        }        
    }
    

    private void Update()
    {

        //足をじたばたする2コマぶんの計算
        var n = (Mathf.FloorToInt(Time.frameCount * 0.2f) % 2);
        var s = Vector3.one;
        for (int i = 0; i < num; i++)
        {
            //位置の更新            
            var p = positions[i];
            
            var t = targets[i];
            var v = t - p;
            
            var lim = 0.05f + 0.05f * ((float) i / (float) num);
            if (v.magnitude > lim)
            {
                v = v.normalized * lim;
            }
            p += v;
            
            var q = Quaternion.Euler(
                new Vector3(0,( -Mathf.Atan2(v.z,v.x) + Mathf.PI/2f ) / Mathf.PI*180f,0)
            );
            positions[i] = p;
            matrices[i] = Matrix4x4.TRS(p,q,s);
            
        }
         
        Graphics.DrawMeshInstanced(
            m_mesh, 0, m_mat, matrices, num, propertyBlock, ShadowCastingMode.On, true
        );

    }


}