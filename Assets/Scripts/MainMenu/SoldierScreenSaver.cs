using UnityEngine;

public class DVDBounceSprite : MonoBehaviour
{
    public float speed = 5f; // World units per second
    private Vector2 direction;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector2 spriteSize;
    private Vector3 originalScale;

    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        originalScale = transform.localScale;

        // Get sprite size in world units
        Vector2 size = spriteRenderer.bounds.size;
        spriteSize = new Vector2(size.x /4, size.y /4); // half-size for easier edge checking
    }  

    void Update()
    {
        Vector3 pos = transform.position;
        pos += (Vector3)(direction * speed * Time.deltaTime);

        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        // Bounce X
        if (pos.x - spriteSize.x < min.x || pos.x + spriteSize.x > max.x)
        {
            direction.x *= -1;
            pos.x = Mathf.Clamp(pos.x, min.x + spriteSize.x, max.x - spriteSize.x);
        }

        // Bounce Y
        if (pos.y - spriteSize.y < min.y || pos.y + spriteSize.y > max.y)
        {
            direction.y *= -1;
            pos.y = Mathf.Clamp(pos.y, min.y + spriteSize.y, max.y - spriteSize.y);
        }

        // Flip sprite on X based on movement direction
        if (direction.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); //Flip
        else
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  //Normal

        transform.position = pos;
    }
}
