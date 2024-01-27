using System;
using JetBrains.Annotations;

public abstract class Upgrade
{
    public virtual string Name => "";
    public virtual string Title => "";
    public virtual string Description => "";
    public virtual decimal Price => 0m;

    public virtual int LvConstraint => 0;
    public virtual Type UpgradeConstraint => null;
    public virtual bool IsReplaceConstraint => false;
    public virtual string IconPath => null;
    public virtual string ImagePath => null;
    public virtual int ToLevel => -1;
    public virtual UpgradeStat Stat => new();
}