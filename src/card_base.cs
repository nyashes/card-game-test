using Godot;
using System;

class card_base : Control
{
    [Export]
    public int lustDamage = 3;
    [Export]
    public int HPDamage = 2;

    [Export]
    public int actionCost = 1;
    [Export]
    public int energyCost = 2;

    [Export]
    public string name;

    [Export]
    public bool isFriendly = false;

    [Export]
    public bool isConstantPower = false;
    [Export]
    public int cooldown = 0;
    protected int currentCooldown;
    
    protected RichTextLabel cardName;
    protected RichTextLabel description;
    private Button playButton;

    protected agent player;
    protected agent badGuy;
    

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cardName = (RichTextLabel)this.FindNode("cardName");
        description = (RichTextLabel)this.FindNode("description");
        playButton = (Button)this.FindNode("playButton");

        player = (agent)GetParent().GetParent().FindNode("player");
        badGuy = (agent)GetParent().GetParent().FindNode("badGuy");

        playButton.Connect("pressed", this, "_card_played", new Godot.Collections.Array { false });
        refreshDescription();

        EndTurnButton.onTick += _OnTick;
    }

    public virtual void _OnTick(object sender, EventArgs e)
    {
        if (isConstantPower && currentCooldown > 0)
        {
            currentCooldown--;
        }
    }

    public virtual void _card_played(bool enemy = false)
    {
        if (isConstantPower && currentCooldown > 0) return;

        agent target = enemy ? player : badGuy;
        agent user = enemy ? badGuy : player;

        if (isFriendly) target = user;
      
        if (user.canUseAction(this.actionCost) && user.canUseEnergy(this.energyCost)) 
        {
            user.tryUseAction(this.actionCost);
            user.tryUseEnergy(this.energyCost);

            target.damageHealth(this.HPDamage);
            target.damageLust(this.lustDamage);
            if (isConstantPower)
            {
                this.currentCooldown = this.cooldown;
            }
            else
            {
                this.QueueFree();
            }
        }
    }

    public virtual void refreshDescription()
    {
        string desc = "";

        if (this.currentCooldown > 0)
        {
            desc += "cooldown: " + this.currentCooldown + "\n";
        }
        if (this.HPDamage > 0)
        {
            desc += "Deals " + this.HPDamage + " damage\n";
        }
        if (this.HPDamage > 0)
        {
            desc += "Deals " + this.lustDamage + " lust\n";
        }
        if (this.actionCost != 0)
        {
            desc += "Costs " + this.actionCost + " action\n";
        }
        if (this.energyCost != 0)
        {
            desc += "Costs " + this.energyCost + " energy\n";
        }


        this.description.Text = desc;
        this.cardName.Text = this.name;
    }

    public static card_base MakeDamageCard(string name, int hp = 0, int lust = 0, int energy = 2, int action = 1)
    {
        card_base card = GD.Load<PackedScene>("res://card.tscn").Instance<card_base>();
        card.HPDamage = hp;
        card.lustDamage = lust;
        card.energyCost = energy;
        card.actionCost = action;
        card.name = name;

        return card;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
