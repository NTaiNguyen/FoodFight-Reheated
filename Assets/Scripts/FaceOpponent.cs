    using UnityEngine;

    public class FaceOpponent : MonoBehaviour
    {
        public Transform opponent;
        public Transform sprite;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>().transform;
            
            if (CompareTag("Player1"))
            {
                opponent = GameObject.FindGameObjectWithTag("Player2")?.transform;
            }
            else if (CompareTag("Player2"))
            {
                opponent = GameObject.FindGameObjectWithTag("Player1")?.transform;
            }

            if (sprite == null)
            sprite = transform.Find("Sprite");
        }

        // Update is called once per frame
        void Update()
        {
            if (opponent == null || sprite == null)
            {
                return;
            }

            if (opponent.position.x > transform.position.x)
            {
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                sprite.localScale = new Vector3(-1, 1, 1);
            }
            
        }
    }
