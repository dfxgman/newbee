public class StatModifier
{
    public readonly float value;
    public readonly TypeModifier type;
    //для сортировки в Stat
    public int order { get { return (int)type; } }
    public StatModifier(float _value, TypeModifier _type)
    {
        type = _type;
        value = _value;
    }
}
public enum TypeModifier
{
    flat = 100,
    persentAdd = 200,
}