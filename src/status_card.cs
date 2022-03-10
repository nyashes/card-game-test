using Godot;
using System;

class status_card : card_base
{
    [Export]
    private string status = "exposed";

    public override void _Ready()
    {
        base._Ready();
    }
    public override void _card_played(bool enemy = false)
    {
        if (isConstantPower && currentCooldown > 0) return;

        agent target = enemy ? player : badGuy;
        agent user = enemy ? badGuy : player;

        if (isFriendly) target = user;

        if (user.canUseAction(this.actionCost) && user.canUseEnergy(this.energyCost)) 
        {
            target.addStatus(status);
        }

        base._card_played();
    }

    public override void refreshDescription()
    {
        base.refreshDescription();
        string desc = this.description.Text;

        desc += "Applies " + status + "\n";

        this.description.Text = desc;
    }

    public static status_card MakeStatusCard(string name, string status, int hp = 0, int lust = 0, int energy = 2, int action = 1)
    {
        PackedScene obj = GD.Load<PackedScene>("res://card.tscn");
        

        card_base cardBase = obj.Instance<card_base>();
        ulong objId = cardBase.GetInstanceId();
        cardBase.SetScript(ResourceLoader.Load("src/status_card.cs"));
        status_card card = GD.InstanceFromId(objId) as status_card;

        card.HPDamage = hp;
        card.lustDamage = lust;
        card.energyCost = energy;
        card.actionCost = action;
        card.status = status;
        card.name = name;

        return card;
    }
}