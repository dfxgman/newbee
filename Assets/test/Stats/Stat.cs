using System.Collections.Generic;

public class Stat
{
    public float Value
    {
        get
        {
            if (isDirty)
            {
                lastValue = CalcStat();
                isDirty = false;
            }
            return lastValue;
        }
    }
    readonly float BaseValue;
    readonly List<StatModifier> Modifiers;
    bool isDirty = true;
    readonly bool isNegativeable;
    float lastValue;
    public Stat(float _baseValue = 0, bool isNegativeable = false)
    {
        Modifiers = new List<StatModifier>();
        BaseValue = _baseValue;
        this.isNegativeable = isNegativeable;
    }

    #region ModifierSystem
    private int CompareModFunc(StatModifier _a, StatModifier _b)
    {
        if (_a.order < _b.order)
            return -1;
        else if (_a.order > _b.order)
            return 1;
        return 0;
    }
    public void AddModifier(StatModifier _mod)
    {
        Modifiers.Add(_mod);
        isDirty = true;
        Modifiers.Sort(CompareModFunc);
    }
    //может и не пригодиться
    public bool RemModifier(StatModifier _mod)
    {
        if (Modifiers.Remove(_mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }
    float CalcStat()
    {
        float finalValue = BaseValue;
        for (int i = 0; i < Modifiers.Count; i++)
        {
            StatModifier mod = Modifiers[i];
            if (mod.type == TypeModifier.flat)
            {
                finalValue += mod.value;
            }
            else
            {
                //если проценты будм считать в сотых, то соточку убираем

                finalValue *= 1 + mod.value / 100;
            }
        }
        if (!isNegativeable && finalValue < 0)
            return 0;
        return finalValue;
    }
    #endregion
}