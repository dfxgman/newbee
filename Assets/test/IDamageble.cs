internal interface IDamageble
{
    Stat Hp { get; }
    void TakeDmg(float _dmg);
    float CurrHP {  get; }
}