using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject _infoParent;
    [SerializeField] private TextMeshProUGUI _infoTitle;
    [SerializeField] private TextMeshProUGUI _infoContent;
    [SerializeField] private Image _infoImage;
    [SerializeField] private GameObject _storeIcon;
    [SerializeField] private GameObject _ingredientIcon;
    [SerializeField] private GameObject _vipIcon;
    [SerializeField] private GameObject _rentIcon;
    [SerializeField] private GameObject _deliveryIcon;
    [SerializeField] private GameObject _versusIcon;

    private Upgrade _currentSelection = null;
    public Upgrade CurrentSelection
    {
        get => _currentSelection;
        set
        {
            _currentSelection = value;
            _infoParent.SetActive(value != null);
            _infoTitle.text = value?.Title;
            _infoContent.text = value?.Description;
            _infoImage.sprite = Resources.Load<Sprite>(value?.ImagePath);
        }
    }

    private List<Upgrade> _store = new()
        { new StoreLv1Upgrade(), new StoreLv2Upgrade(), new StoreLv3Upgrade(), new StoreLv4Upgrade() };

    private List<Upgrade> _delivery = new() { new FreeDeliveryUpgrade(), new HalfDeliveryCostUpgrade() };

    private List<Upgrade> _ingredient = new()
    {
        new IngredientCostLv1Upgrade(), new IngredientCostLv2Upgrade(), new IngredientCostLv3Upgrade(),
        new IngredientCostLv4Upgrade()
    };

    private List<Upgrade> _versus = new()
        { new VersusPriorityUpgrade(), new VersusCostBenefitsLv3Upgrade(), new VersusCostBenefitsLv4Upgrade() };

    private List<Upgrade> _vip = new() { new QuickVipUpgrade(), new VipCostBenefitsUpgrade() };

    private List<Upgrade> _rent = new()
        { new RentCostLv1Upgrade(), new RentCostLv2Upgrade(), new RentCostLv3Upgrade() };

    private void Start()
    {
        UpdateUi();
    }

    private Upgrade GetCurrentUpgrade(List<Upgrade> list, Store store)
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (store.IsNextUpgrade(list[i]))
            {
                return list[i];
            }
        }

        return list[0];
    }

    private void ChangeCurrentSelection(List<Upgrade> list)
    {
        CurrentSelection = GetCurrentUpgrade(list, GameManager.Instance.Player);
        UpdateUi();
    }

    public void OnClickStoreButton() => ChangeCurrentSelection(_store);
    public void OnClickDeliveryButton() => ChangeCurrentSelection(_delivery);
    public void OnClickIngredientButton() => ChangeCurrentSelection(_ingredient);
    public void OnClickVersusButton() => ChangeCurrentSelection(_versus);
    public void OnClickVipButton() => ChangeCurrentSelection(_vip);
    public void OnClickRentButton() => ChangeCurrentSelection(_rent);

    public void OnPurchaseUpgrade()
    {
        var c = CurrentSelection;
        if (c == null) return;
        var player = GameManager.Instance.Player;

        if (c.ToLevel != -1)
            player.Level = c.ToLevel;

        player.BuyUpgrade(c);
        CurrentSelection = null;
        UpdateUi();
    }

    private void ChangeLockState(bool isLocked, GameObject g)
    {
        var parent = g.GetComponent<Image>();
        
        Color parentColor = parent.color;
        parentColor.a = isLocked ? 0.3f : 1f;
        parent.color = parentColor;

        foreach (Transform child in g.transform)
        {
            if (child.CompareTag("Lock"))
                child.gameObject.SetActive(isLocked);
            else
            {
                var color = child.GetComponent<Image>();
                if (color != null)
                    color.color = parentColor;
            }
        }
    }

    private void UpdateUiInternal(Store player, List<Upgrade> list, GameObject go)
    {
        var current = GetCurrentUpgrade(list, player);
        var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = current.Price.ToString(CultureInfo.InvariantCulture);
        ChangeLockState(current.LvConstraint > player.Level, go);
    }
    private void UpdateUi()
    {
        var player = GameManager.Instance.Player;
        UpdateUiInternal(player, _store, _storeIcon);
        UpdateUiInternal(player, _delivery, _deliveryIcon);
        UpdateUiInternal(player, _ingredient, _ingredientIcon);
        UpdateUiInternal(player, _vip,_vipIcon);
        UpdateUiInternal(player, _rent, _rentIcon);
        UpdateUiInternal(player, _versus, _versusIcon);
    }
}