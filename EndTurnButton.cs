using Godot;
using System;

public class EndTurnButton : Button
{
    private agent player;
    private agent badGuy;
    public static event EventHandler onTick;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.


    public override void _Ready()
    {
        player = (agent)GetParent().FindNode("player");
        badGuy = (agent)GetParent().FindNode("badGuy");
        Connect("pressed", this, "_end_turn");
    }

    public void _end_turn()
    {
        player.tryUseAction(-2);
        player.tryUseEnergy(-2);

        card_base card = card_base.MakeDamageCard("Heavy Attack", 3, 0, 3, 1);
        this.AddChild(card);
        card._card_played(true);
        if (!card.IsQueuedForDeletion()) card.QueueFree();

        badGuy.tryUseAction(-2);
        badGuy.tryUseEnergy(-2);

        onTick.Invoke(this, new EventArgs());
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
