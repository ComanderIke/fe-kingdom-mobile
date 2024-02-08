using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Advertisements;

namespace LostGrace
{
    public class QueueEntry
    {
        public Transform Transform;
        public bool Direction;

        public int PositionIndex;
        public QueueEntry(Transform transform, bool direction, int positionIndex)
        {
            this.Transform = transform;
            this.Direction = direction;
            this.PositionIndex = positionIndex;
        } 
    }
    public class ShrineUnitController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private List<Transform> positions;
        [SerializeField] private GameObject toMove;
        [SerializeField] private SpriteRenderer sprite;
        private int currentPosition = 0;

        [SerializeField] private float movetime = 1.5f;
        [SerializeField] private AnimationSpriteSwapper spriteSwapper;
        [SerializeField] private Queue<QueueEntry> positionQueue;
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Idle = Animator.StringToHash("Idle");

        // Start is called before the first frame update
        void Start()
        {
            positionQueue = new Queue<QueueEntry>();
            Show();
        }

        public void Show()
        {
            animator.SetTrigger(Idle);
        }
        
        public void MoveRight()
        {
            currentPosition++;
            sprite.flipX = true;
            if (currentPosition > positions.Count - 1)
                currentPosition = 0;
            positionQueue.Enqueue(new QueueEntry(positions[currentPosition], false, currentPosition));
            if(positionQueue.Count==1)
                Move(LeanTweenType.easeInOutQuad);
            if(positionQueue.Count==2)
                Move(LeanTweenType.linear);
        }
        
        void Move(LeanTweenType ease)
        {
            LeanTween.cancel(toMove);
            LeanTween.moveX(toMove, positionQueue.Peek().Transform.position.x, movetime).setEase(positionQueue.Peek().Direction?(positionQueue.Peek().PositionIndex%2==0?LeanTweenType.easeInQuad:LeanTweenType.easeOutQuad):positionQueue.Peek().PositionIndex%2==0?LeanTweenType.easeInQuad:LeanTweenType.easeOutQuad).setOnComplete(()=>
            {
               
                   // animator.SetTrigger(Idle);
            });
            LeanTween.moveZ(toMove, positionQueue.Peek().Transform.position.z, movetime).setEase(positionQueue.Peek().Direction?(positionQueue.Peek().PositionIndex%2==0?LeanTweenType.easeOutQuad:LeanTweenType.easeInQuad):positionQueue.Peek().PositionIndex%2==0?LeanTweenType.easeOutQuad:LeanTweenType.easeInQuad).setOnComplete(()=>
            {
                 positionQueue.Dequeue();
                if (positionQueue.Count != 0)
                {
                    if(positionQueue.Count==1)
                        Move(LeanTweenType.easeOutQuad);
                    else
                    {
                        Move(LeanTweenType.linear);
                    }
                }
                else
                {
                    animator.SetTrigger(Idle);
                }
            });
            animator.SetTrigger(Walking);
        }

        public void MoveLeft()
        {
            sprite.flipX = false;
            currentPosition--;
            if (currentPosition <0)
                currentPosition = positions.Count-1;
            positionQueue.Enqueue(new QueueEntry(positions[currentPosition], true, currentPosition));
            if(positionQueue.Count==1)
                Move(LeanTweenType.easeInOutQuad);
            if(positionQueue.Count==2)
                Move(LeanTweenType.linear);
        }

        public void SetUnit(Unit unit)
        {
            spriteSwapper.Init(unit.visuals.CharacterSpriteSet);
        }
    }
}
