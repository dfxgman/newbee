using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageble
{
    public static Player Instance { get; private set; }
    //можно впринципе сделать оболчку дл€ листа статов с индексатором
    #region Stats
    public Stat Hp {  get; private set; }
    public Stat Armour { get; private set; }
    public Stat Movespeed { get; private set; }
    public Stat Luck { get; private set; }
    public Stat AttackSpeed { get; private set; }
    public Stat AttackCD { get; private set; }
    public Stat AttackDamage { get; private set; }
    public Stat AttackAmount { get; private set; }
    #endregion
    public Weapon MainWeapon { get; private set; }

    public float CurrHP { get; private set; }
    int currLvl;
    bool alive;
    float currExp;
    float requierExp;
    bool immunable;
    PlayerController playerController;
    //пока что все на пушки скидываю
    event Action<Vector2> Shoot;
    //ну это скорее всего на геймменеджер уйдет или со статики
    public int CurrWeaponCount { get; private set; }
    public int WeaponMaxCount { get; private set; }

    //пока что без инкапсул€ции
    public List<Weapon> NewWeapons { get; private set; } = new List<Weapon>();

    //или Awake
    public void Init(/* вс€кое дл€ иницилизации лучше в скиптэбл запихать*/ PlayerSO _so, int _wpMax)
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
        //потом можно парсить с SO чара
        #region StatsInit
        Hp = new(_so.Hp);
        Armour = new(_so.Armour, true);
        Movespeed = new(_so.Movespeed);
        Luck = new(_so.Luck, true);
        AttackSpeed = new(_so.AttackSpeed);
        AttackCD = new(_so.AttackCD);
        AttackDamage = new(_so.AttackDamage);
        AttackAmount = new(_so.AttackAmount);
        #endregion


        MainWeapon = _so.MainWeapon;
        Shoot += MainWeapon.Shoot;
        CurrWeaponCount = 1;

        alive = true;
        CurrHP = Hp.Value;
        WeaponMaxCount = _wpMax;
        currExp = 0;
        currLvl = 1;
        requierExp = 10;

        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (alive)
        {
            Shoot?.Invoke(playerController.GetMousePoint());
        }
    }

    public void TakeDmg(float _dmg)
    {
        if(!immunable)
        {
            StartCoroutine(GetImmunable());
            if (_dmg < 0)
                _dmg = 0;
            _dmg -= Armour.Value;
            CurrHP -= _dmg;
            if (CurrHP < 0)
                Death();
        }
    }
    public void Heal(float _heal)
    {
        CurrHP += _heal;
        if(CurrHP > Hp.Value)
            CurrHP = Hp.Value;
    }
    //тут будет обращение к геймменеджеру или можно через эвент реализовать
    public void Death()
    {
        alive = false;
    }

    IEnumerator GetImmunable()
    {
        immunable = true;
        yield return new WaitForSeconds(0.5f);
        immunable = false;
    }

    #region DiffCalsses
    //на будующее
    public void GetNewWeapon(Weapon _wp)
    {
        NewWeapons.Add(_wp);
        CurrWeaponCount++;

        Shoot += _wp.Shoot;
    }
    public void GetExp(float _exp)
    {
        if(_exp < 0)
            _exp = 0;
        currExp += _exp;

        if(currExp >= requierExp)
        {
            currExp -= requierExp;
            LvlUp();
        }
    }
    private void LvlUp()
    {
        //‘ункци€ прокачки или ссылка геймменеджер
        currLvl++;
        //ну формулу надо будет подбалансить
        requierExp *= (float)currLvl / 1.5f;
    }
    #endregion
}