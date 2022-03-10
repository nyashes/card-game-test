using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class agent : Control
{
    private ProgressBar HealthBar;
    private ProgressBar LustBar;
    private ProgressBar EnergyBar;
    private ProgressBar ActionBar;
    private RichTextLabel StatusList;
    private Dictionary<string, int> Statuses;

    public override void _Ready()
    {
        HealthBar = (ProgressBar)this.FindNode("Health");
        LustBar = (ProgressBar)this.FindNode("Lust");
        EnergyBar = (ProgressBar)this.FindNode("Energy");
        ActionBar = (ProgressBar)this.FindNode("Action");
        StatusList = (RichTextLabel)this.FindNode("Status");
        Statuses = new Dictionary<string, int>();

        EndTurnButton.onTick += _OnTick;
    }

    public virtual void _OnTick(object sender, EventArgs e)
    {
        var keys = Statuses.Keys.ToArray();
        foreach (string status in keys)
        {
            int remaining = --Statuses[status];
            if (remaining <= 0)
            {
                Statuses.Remove(status);
            }
        }

        refreshStatusList();
    }

    public virtual bool tryUseEnergy(int amount)
    {
        if (this.EnergyBar.Value >= amount)
        {
            this.EnergyBar.Value -= amount;
            return true;
        }

        return false;
    }
    public virtual bool tryUseAction(int amount)
    {
        if (this.ActionBar.Value >= amount)
        {
            this.ActionBar.Value -= amount;
            return true;
        }

        return false;
    }

    public virtual bool canUseEnergy(int amount)
    {
        return this.EnergyBar.Value >= amount;
    }
    public virtual bool canUseAction(int amount)
    {
        return this.ActionBar.Value >= amount;
    }
    public virtual void damageHealth(int value)
    {
        if (value != 0)
        {
            this.HealthBar.Value -= value;
            if (this.Statuses.ContainsKey("exposed"))
            {
                this.HealthBar.Value -= 1;
            }
        }
    }

    public virtual void damageLust(int value)
    {
        if (value != 0)
        {
            this.LustBar.Value += value;
            if (this.Statuses.ContainsKey("entranced"))
            {
                this.LustBar.Value += 1;
            }
        }
    }

    public void addStatus(string status, int duration = 1)
    {
        int currentDuration;
        if (this.Statuses.TryGetValue(status, out currentDuration))
        {
            this.Statuses[status] = Math.Max(currentDuration, duration);
        }
        else
        {
            this.Statuses.Add(status, duration);
        }

        refreshStatusList();
    }

    private void refreshStatusList()
    {
        string result = "";
        foreach (string status in this.Statuses.Keys)
        {
            result += status + "(" + this.Statuses[status] + ")" + "\n";
        }

        this.StatusList.Text = result;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
