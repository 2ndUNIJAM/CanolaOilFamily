using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    [SerializeField] private Image _purchaseBtn;
    [SerializeField] private TextMeshProUGUI _currentMoney;

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
            if (value != null)
            {
                _infoImage.sprite = ResourcesDictionary.Load<Sprite>(value.ImagePath);
                var tmp = _purchaseBtn.GetComponentInChildren<TextMeshProUGUI>();
                var player = GameManager.Instance.Player;
                if (value.LvConstraint > player.Level)
                {
                    _purchaseBtn.sprite = ResourcesDictionary.Load<Sprite>("gray_btn");
                    tmp.text = $"가게 확장 Lv.{value.LvConstraint} 필요";
                }
                else if (player.HasUpgrade(value))
                {
                    _purchaseBtn.sprite = ResourcesDictionary.Load<Sprite>("gray_btn");
                    tmp.text = "Lv.MAX";
                }
                else if (player.Money < value.Price)
                {
                    _purchaseBtn.sprite = ResourcesDictionary.Load<Sprite>("gray_btn");
                    tmp.text = "소지금 부족";
                }
                else
                {
                    _purchaseBtn.sprite = ResourcesDictionary.Load<Sprite>("yellow_btn");
                    tmp.text = "강화";
                }
            }
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
        if (store.HasUpgrade(list.Last())) return list.Last();
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
        var player = GameManager.Instance.Player;
        if (c == null || player.HasUpgrade(c) || player.Money < c.Price || c.LvConstraint > player.Level) return;

        if (c.ToLevel != -1)
            player.Level = c.ToLevel;

        player.BuyUpgrade(c);
        CurrentSelection = null;
        UpdateUi();
    }

    private void ChangeLockState(bool isDisabled, bool isLocked, GameObject g)
    {
        var parent = g.GetComponent<Image>();

        Color parentColor = parent.color;
        parentColor.a = isDisabled ? 0.3f : 1f;
        parent.color = parentColor;

        foreach (Transform child in g.transform)
        {
            if (child.CompareTag("Lock"))
                child.gameObject.SetActive(isLocked);
            else
            {
                var color = child.GetComponent<Image>();
                if (color != null)
                {
                    color.color = parentColor;
                }
                else
                {
                    var tmp = child.GetComponent<TextMeshProUGUI>();
                    if (tmp != null)
                    {
                        var tmpColor = tmp.color;
                        tmpColor.a = isDisabled ? 0.3f : 1f;
                        tmp.color = tmpColor;
                    }
                }
            }
        }
    }

    private void UpdateUiInternal(Store player, List<Upgrade> list, GameObject go)
    {
        var current = GetCurrentUpgrade(list, player);
        var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = current.Price.ToString(CultureInfo.InvariantCulture);
        var isLocked = current.LvConstraint > player.Level;
        var isDisabled = isLocked || player.HasUpgrade(current) || player.Money < current.Price;
        ChangeLockState(isDisabled, isLocked, go);
    }

    private void UpdateUi()
    {
        var player = GameManager.Instance.Player;
        _currentMoney.text = player.Money.ToString(CultureInfo.InvariantCulture);
        UpdateUiInternal(player, _store, _storeIcon);
        UpdateUiInternal(player, _delivery, _deliveryIcon);
        UpdateUiInternal(player, _ingredient, _ingredientIcon);
        UpdateUiInternal(player, _vip, _vipIcon);
        UpdateUiInternal(player, _rent, _rentIcon);
        UpdateUiInternal(player, _versus, _versusIcon);
    }
}