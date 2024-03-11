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

      private int x=-1;
      private int y = -1;

      public bool IsOnPosition(int posX, int posY)
      {
         if (x == posX && y == posY)
            return true;
         for (int x1 = x; x1 <= (x + width - 1); x1++)
         {
            for (int y1 = y; y1 <= (y + height - 1); y1++)
            {
               if (x1 == posX && y1 == posY)
                  return true;
            }
         }
         return false;
      }

      private void OnEnable()
      {
         var localPosition = transform.localPosition;
         x=(int)localPosition.x;
         y=(int)localPosition.y;
         if (terrainData.sprites != null && terrainData.sprites.Count >= 1)
         {
            GetComponent<SpriteRenderer>().sprite = terrainData.sprites[Random.Range(0,terrainData.sprites.Count)];
         }
         
      }

      void Update()
      {
         var transform1 = transform;
         var localPosition = transform1.localPosition;
         localPosition = new Vector3((int) localPosition.x, (int) localPosition.y,
            (int) localPosition.z);
         transform1.localPosition = localPosition;
      }
      public int X => x;

      public int Y => y;
   }
}


