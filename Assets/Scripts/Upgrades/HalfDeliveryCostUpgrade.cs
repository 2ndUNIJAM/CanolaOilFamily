﻿using System;

public class HalfDeliveryCostUpgrade : Upgrade
{
    public override string Name => "HalfDeliveryCostUpgrade";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(FreeDeliveryUpgrade);
    public override float DeliveryCostFactor => 0.5f;
}