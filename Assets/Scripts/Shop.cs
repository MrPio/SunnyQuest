using System;
using DefaultNamespace;
using DefaultNamespace.Model;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] public GameObject LeftArrow;
    [SerializeField] public GameObject RightArrow;
    [SerializeField] public TextMeshProUGUI CostText;
    [SerializeField] public TextMeshProUGUI ContentText;
    [SerializeField] public TextMeshProUGUI NameText;
    [SerializeField] public SpriteRenderer MercantImage;
    [SerializeField] private AudioClip arrowMoveSound;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip noBuySound;
    private AudioSource _audioSource;
    private bool answer = true;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;
    public MercantModel Model;

    private void Start()
    {
        _audioSource = CamManager.mainCam.GetComponent<AudioSource>();
    }

    public void SetLeftArrow()
    {
        if (answer)
            return;
        LeftArrow.SetActive(true);
        RightArrow.SetActive(false);
        _audioSource.PlayOneShot(arrowMoveSound);
        answer = true;
    }

    public void SetRightArrow()
    {
        if (!answer)
            return;
        LeftArrow.SetActive(false);
        RightArrow.SetActive(true);
        _audioSource.PlayOneShot(arrowMoveSound);
        answer = false;
    }

    public void Confirm()
    {
        if (answer)
        {
            if (_inventoryManager.Coins >= Model.CalculateCost)
            {
                _audioSource.PlayOneShot(buySound);
                Model.Buy();
            }
            else
                _audioSource.PlayOneShot(noBuySound);
        }
        Close();
    }

    public void Close()
    {
        _inventoryManager.MessageBox = null;
        _inventoryManager.Shop = null;
        _inventoryManager.IsThereMessageBox = false;
        Destroy(gameObject);
    }

    public void Initialize()
    {
        NameText.text = Model.GetType().Name;
        ContentText.text = Model.Message;
        MercantImage.sprite = Resources.Load<Sprite>(Model.SpriteCut);
        CostText.text = Model.CalculateCost.ToString();
    }
}