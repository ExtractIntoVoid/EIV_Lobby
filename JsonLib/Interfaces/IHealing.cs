namespace JsonLib.Interfaces;

public interface IHealing : IUsable
{
    public float HealAmount { get; set; }

    // IEffect's BaseID I guess
    public List<string> SideEffect { get; set; }
}
