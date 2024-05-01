using Newtonsoft.Json;

namespace JsonLib.Interfaces;

public interface IGun : IItem
{
    //  IMagazine's BaseID
    public List<string> MagazineSupport { get; set; }

    public IMagazine? Magazine { get; set; }
}
