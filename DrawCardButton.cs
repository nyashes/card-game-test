using Godot;
using System;

public class DrawCardButton : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private hand playerHand;
    private Random randomizer;
    private deck deck;
    public override void _Ready()
    {
        playerHand = (hand)GetParent().FindNode("playerHand");
        randomizer = new Random();
        deck = new deck();
        deck.populate();
        Connect("pressed", this, "_draw_card");
        this.playerHand.AddChild(this.deck.draw());
        this.playerHand.AddChild(this.deck.draw());
        this.playerHand.AddChild(this.deck.draw());
        this.playerHand.AddChild(this.deck.draw());
        EndTurnButton.onTick += _OnTick;
    }

    public virtual void _OnTick(object sender, EventArgs e)
    {
        this.playerHand.AddChild(this.deck.draw());
    }
    public void _draw_card()
    {
        card_base card = this.deck.draw();
        this.playerHand.AddChild(card);
        card.refreshDescription();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
