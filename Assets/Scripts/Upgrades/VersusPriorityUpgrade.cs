using System;

public class VersusPriorityUpgrade : Upgrade
{
    public override string Name => "VersusPriorityUpgrade";
    public override string Title => "치킨 요리법 개발";
    public override string Description => "구매 가격이 같다면 우리 치킨집에서 구매한다";
    public override decimal Price => 100m;
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(isPriorInVersus: true);
    public override string ImagePath => "Sprites/upgrade_versus_image";
    public override string IconPath => "Sprites/upgrade_versus_image";
}
