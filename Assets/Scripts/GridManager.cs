using UnityEngine;
using Utilies;

public class GridManager : MonoBehaviour
{
    public GameObject block;
    public int slices = 10;

    private void Start()
    {
        var cellsSize = CamManager.camHeight / slices;
        for (var i = 0; i < CamManager.camWidth/cellsSize; i++)
        {
            var position = new Vector3(i * cellsSize - CamManager.camWidth / 2, -CamManager.camHeight / 2);

            var newBlock = Instantiate(block, position, Quaternion.identity);

            var spriteRendered = newBlock.GetComponent<SpriteRenderer>();
            var texture2D = Resources.Load<Texture2D>("Images/ground");
            var sprite = Sprite.Create(
                texture: texture2D,
                rect: new Rect(0, 0, texture2D.width, texture2D.height),
                pivot: new Vector2(0, 0), //<--- IMPORTANT!
                pixelsPerUnit: 1
            );
            spriteRendered.sprite = sprite;
            Size.newScale(newBlock, cellsSize);

            var boxCollider2D = newBlock.GetComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(texture2D.width, texture2D.height);
            boxCollider2D.offset = new Vector2((float)texture2D.width / 2, (float)texture2D.height / 2);
        }
    }
}