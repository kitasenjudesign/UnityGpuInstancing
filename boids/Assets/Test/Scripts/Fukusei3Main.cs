
    using System.Collections.Generic;
    using UnityEngine;

public class Fukusei3Main : MonoBehaviour
{

    public Fukusei3 obj;
    public int num = 10;
    
    private List<Fukusei3> _list;
    
    void Start()
    {
        _list = new List<Fukusei3>();
        for (int i = 0; i < num; i++)
        {
            var a = Instantiate(obj);
            a.enabled = true;
            _list.Add(a);
        }

        obj.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //ChangeTarget();
            var n = Mathf.FloorToInt(Random.value * 3f);
            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].ChangeTarget(n);
            }
                        
        }
        
    }
}
