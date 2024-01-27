using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private Upgrade currentSelection = null;

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
        currentSelection = GetCurrentUpgrade(list, GameManager.Instance.Player);
        UpdateUi();
    }

    public void OnClickStoreButton() => ChangeCurrentSelection(_store);
    public void OnClickDeliveryButton() => ChangeCurrentSelection(_delivery);
    public void OnClickIngredientButton() => ChangeCurrentSelection(_ingredient);
    public void OnClickVersusButton() => ChangeCurrentSelection(_versus);
    public void OnClickVipButton() => ChangeCurrentSelection(_vip);
    public void OnClickRentButton() => ChangeCurrentSelection(_rent);

    private void OnPurchaseUpgrade()
    {
        var c = currentSelection;
        if (c == null) return;
        var player = GameManager.Instance.Player;

        if (c.ToLevel != -1)
            player.Level = c.ToLevel;

        player.BuyUpgrade(c);
        currentSelection = null;
        UpdateUi();
    }

    private void UpdateUi()
    {
        // TODO: update ui based on currentSelection, each lists' GetCurrentUpgrade return value.
    }
}