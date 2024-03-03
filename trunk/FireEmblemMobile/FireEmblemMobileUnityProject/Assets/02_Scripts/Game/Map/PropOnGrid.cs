using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Map
{
   [ExecuteInEditMode]
   public class PropOnGrid : MonoBehaviour
   {
      public GridTerrainData terrainData;

      public int width = 1;
      public int height = 1;

      public bool IsOnPosition(int x, int y)
      {
         for (int x1 = X; x1 <= (X + width - 1); x1++)
         {
            for (int y1 = Y; y1 <= (Y + height - 1); y1++)
            {
               if (x1 == x && y1 == y)
                  return true;
            }
         }
         return false;
      }

      private void OnEnable()
      {
         if (terrainData.sprites != null && terrainData.sprites.Count >= 1)
         {
            GetComponent<SpriteRenderer>().sprite = terrainData.sprites[Random.Range(0,terrainData.sprites.Count)];
         }
         
      }

      void Update()
      {
         transform.localPosition = new Vector3((int) transform.localPosition.x, (int) transform.localPosition.y,
            (int) transform.localPosition.z);
      }
      public int X
      {
         get
         {
            return (int)transform.localPosition.x;
         }
      }
      public int Y
      {
         get
         {
            return (int)transform.localPosition.y;
         }
      }
   }
}


