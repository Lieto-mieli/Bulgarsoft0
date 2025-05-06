using UnityEngine;

public class DVDBounceSprite : MonoBehaviour
{
    public float speed = 4f;  // Movement speed 
    private Vector2 direction; // Current movement direction

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera; 
    private Vector2 spriteSize; 
    private Vector3 originalScale; // Stores the original scale to preserve size when flipping

    void Start()
    {
        // Pick a random movement direction
        direction = Random.insideUnitCircle.normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        
        // Store the original scale from the inspector
        originalScale = transform.localScale;

        // Get sprite size in world units
        Vector2 size = spriteRenderer.bounds.size;
        spriteSize = new Vector2(size.x /4, size.y /4); // 1/4 size for easier edge checking
    }  

    void Update()
    {
        // Move the sprite
        Vector3 pos = transform.position;
        pos += (Vector3)(direction * speed * Time.deltaTime);

        // Get screen boundaries (bottom left and top right corners)
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        // Bounce Horizontally
        if (pos.x - spriteSize.x < min.x || pos.x + spriteSize.x > max.x)
        {
            direction.x *= -1;
            pos.x = Mathf.Clamp(pos.x, min.x + spriteSize.x, max.x - spriteSize.x);
        }

        // Bounce Vertically
        if (pos.y - spriteSize.y < min.y || pos.y + spriteSize.y > max.y)
        {
            direction.y *= -1;
            pos.y = Mathf.Clamp(pos.y, min.y + spriteSize.y, max.y - spriteSize.y);
        }

        // Flip sprite depending on movement direction (preserve original scale)
        if (direction.x > 0)
        {
            // Flip horizontally by inverting x scale
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); //Flip
        }
        else
        {
            // Reset the original scale
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  //Normal
        }
            
        // Apply the new position
        transform.position = pos;
    }
}
