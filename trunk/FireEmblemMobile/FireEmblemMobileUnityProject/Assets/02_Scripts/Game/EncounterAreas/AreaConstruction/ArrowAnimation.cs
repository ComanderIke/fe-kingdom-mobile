using UnityEngine;

namespace Game.EncounterAreas.AreaConstruction
{
    public class ArrowAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
         Vector3 targetPosition;
         private Vector3 startPosition;
         private bool active;
         private bool init;
         [SerializeField]private Color selectedColor;
         [SerializeField]private Color attackColor;
         [SerializeField] private Color normalColor;
        void OnEnable()
        {
           

            if (active)
            {
                LeanTween.cancel(gameObject);
                LeanTween.move(gameObject, targetPosition, 1).setLoopPingPong(-1).setEaseInOutQuad();
            }
            else
            {
                LeanTween.cancel(gameObject);
                LeanTween.move(gameObject,startPosition, .25f).setEaseInQuad();

            }
        }

        public void SetTargetPosition(Vector3 targ)
        {
            if (!init)
            {
                this.startPosition = transform.position;
                init = true;
            }
            this.targetPosition = targ;

            //SetActive(true);
            OnEnable();
        }

        public void SetActive(bool value, bool attack = false)
        {
            this.active = value;
            
            GetComponent<SpriteRenderer>().color = active ? selectedColor : normalColor;
            if (attack)
                GetComponent<SpriteRenderer>().color = attackColor;
            OnEnable();
        }



    }
}
