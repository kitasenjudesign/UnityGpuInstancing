
    using System.Collections.Generic;
    using UnityEngine;

    public class ImgPos : MonoBehaviour
    {
        public Texture2D sourceTex;
        private static List<Vector2> points; 
        
        
        void Start()
        {

            points = new List<Vector2>();

            var ww = sourceTex.width;
            var hh = sourceTex.height;
            
            for (int i = 0; i < ww; i++)
            {
                for (int j = 0; j < hh; j++)
                {
                    if( sourceTex.GetPixel(i, j).r > 0 )
                    {
                        points.Add(new Vector2(i-(float)ww/2f,j-(float)hh/2f));
                    }
                }
            }


            Debug.Log("----");
            Debug.Log(points.Count);
            
        }

        public static Vector2 GetRandomPoint()
        {
            return points[ Mathf.FloorToInt(Random.value * points.Count) ];
        }
        
        
    }
