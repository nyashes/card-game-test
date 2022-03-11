using System.Collections.Generic;
using Godot;

static class card_parser
{
    public static IEnumerable<card_base> parseCard(string filename)
    {
        var f = new File();
        if (f.Open(filename, File.ModeFlags.Read) == Error.Ok)
        {
            Godot.Collections.Array json = JSON.Parse(f.GetAsText()).Result as Godot.Collections.Array;
            foreach(Godot.Collections.Dictionary item in json)
            {
                card_base parsed_card = null;

                var stats = new 
                {
                    hp = item.Contains("hp") ? (int)(float)item["hp"] : 0,
                    lust = item.Contains("lust") ? (int)(float)item["lust"] : 0,
                    energy = item.Contains("energy") ? (int)(float)item["energy"] : 0,
                    action = item.Contains("action") ? (int)(float)item["action"] : 1
                };

                if (item.Contains("status"))
                {
                    parsed_card = status_card.MakeStatusCard(item["name"] as string, item["status"] as string, stats.hp, stats.lust, stats.energy, stats.action);
                }
                else
                {
                    parsed_card = card_base.MakeDamageCard(item["name"] as string, stats.hp, stats.lust, stats.energy, stats.action);
                } 

                if (parsed_card != null) yield return parsed_card;
            }

            f.Close();
        }
    }
}
