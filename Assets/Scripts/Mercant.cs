using DefaultNamespace;
using DefaultNamespace.Model;
using UnityEngine;

public class Mercant : MonoBehaviour
{
    public MercantModel Model;
    [SerializeField] private GameObject _messageBox;
    [SerializeField] private SpriteRenderer _soSpriteRenderer;

    private Transform _canvas;
    private readonly InventoryManager _inventoryManager = InventoryManager.getInstance;

    private void Start()
    {
        _canvas = GameObject.FindWithTag("Canvas").transform;
    }

    public void Initialize()
    {
        _soSpriteRenderer.sprite = Resources.Load<Sprite>(Model.Sprite);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
        {
            if (_inventoryManager.IsThereMessageBox)
                return;
            if(Time.timeSinceLevelLoad-_inventoryManager.LastSpacebarMessageBox<2f)
                return;
            _inventoryManager.MessageBox = Instantiate(_messageBox, _canvas);
            _inventoryManager.IsThereMessageBox = true;
            _inventoryManager.Shop = _inventoryManager.MessageBox.GetComponent<Shop>();
            _inventoryManager.Shop.Model = Model;
            _inventoryManager.Shop.Initialize();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
            if (!_inventoryManager.IsThereMessageBox)
                return;
        _inventoryManager.Shop.Close();
    }
}