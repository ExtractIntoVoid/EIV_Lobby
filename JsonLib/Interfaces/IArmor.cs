namespace JsonLib.Interfaces;

public interface IArmor : IItem
{
    //How many | hatékonyság | for blocking this bullet
    public decimal BlockEfficacy { get; set; }
    // Effects Applied if hit bullet. - TEMP not available
    //public List<IEffect> EffectsWhenHIT { get; set; }

    //This has a valid armor Weight (for sprinting, etc.)
    public decimal ArmorWeight { get; set; }
}
