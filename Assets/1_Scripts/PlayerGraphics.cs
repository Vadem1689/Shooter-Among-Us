using BRAmongUS.Skins;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    [SerializeField]    private Rigidbody2D rb;

    [SerializeField]    private Animator animator;
    
    [SerializeField]    private SpriteRenderer face;
    
    private Vector3 initialScale;

    private Vector2 lookDirection;

    private void Start()
    {
        initialScale = transform.localScale;
    }
    
    public void SetPlayerSkin(in SkinData skinData) 
    {
        animator.runtimeAnimatorController = skinData.Animator;

        //face.sprite = skinData.FaceSprite;

        //if(face.sprite.name == "Background")
        //{
        //    face.gameObject.SetActive(false);
        //    print("получ");
        //}
    }
    
    public void Look(Vector2 direction) {
        lookDirection = direction - rb.position;
        if (lookDirection.x > 0.1f)
        {
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            return;
        }

        if (lookDirection.x < 0.1f)
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            return;
        }
    }

    void Update()
    {

        if (rb.velocity.magnitude > 1f)
        {
            animator.Play("Walk");            
        }
        else {
            animator.Play("Idle");
        }        
    }

    public void Die() {
        //Debug.Log("GFX Dead");
        animator.Play("Death");
        Destroy(this);
    }
}
