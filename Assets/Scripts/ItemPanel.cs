using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    private void OnEnable()
    {
        Current = null;
        _selectedContent.SetActive(false);
        DeNotify();
    }

    private void Start()
    {
        _itemSprites = new[]
        {
            Resources.Load<Sprite>("Item/Skill_Icon_Flame"),
            Resources.Load<Sprite>("Item/Skill_Icon_Rob"),
            Resources.Load<Sprite>("Item/Skill_Icon_Shield")
        };
    }

    public enum ItemCode
    {
        FLAME, THEIF, SHIELD
    }

    private Sprite[] _itemSprites;

    private IItem _current;
    private IItem Current
    {
        get { return _current; }
        set
        {
            _current = value;
            _name.text = _current?.Name ?? string.Empty;
            _desc.text = _current?.Description ?? string.Empty;
        }
    }

    [SerializeField] private GameObject _selectedContent;
    [SerializeField] private Image _selectedImage;
    [SerializeField] private TMPro.TMP_Text _name;
    [SerializeField] private TMPro.TMP_Text _desc;
    [SerializeField] private TMPro.TMP_Text _extraNotify;

    public void SelectItem(int c)
    {
        _selectedImage.sprite = _itemSprites[c];
        _selectedContent.SetActive(true);
        DeNotify();
        switch ((ItemCode)c)
        {
            case ItemCode.FLAME:
                Current = new EnemyIngredientCostIncrease();
                break;
            case ItemCode.THEIF:
                Current = new ThiefItem();
                break;
            case ItemCode.SHIELD:
                Current = new Shield();
                break;
        }
    }

    public void Purchase()
    {
        if (Current == null) { return; }

        Store player = GameManager.Instance.Player;

        if (player.Money < Current.Price)
        {
            Notify("돈이 모자랍니다!");
            return;
        }

        player.Money -= Current.Price;
        player.ItemManager.BuyItem(Current);
        Notify("구매했습니다.");
    }

    private void Notify(string msg)
    {
        _extraNotify.text = msg;
        _extraNotify.gameObject.SetActive(true);
    }

    private void DeNotify()
    {
        _extraNotify.gameObject.SetActive(false);
    }
}