﻿using NUnit.Framework;

using System.Collections.Generic;
using UnityEngine;

using AI.Player;
using AI.Enemy;

[TestFixture]
public class Test
{
    private const int PLAYER_POWER = 100;
    private const int PLAYER_SPEED = 100;
    private const int PLAYER_HP = 100;
    private readonly Vector3 PLAYER_POS = Vector3.zero;

    private EnemyBehaviorController behavior = new EnemyBehaviorController();
    private PlayerModel player = new PlayerModel(PLAYER_POWER, PLAYER_SPEED, PLAYER_HP);

    private void Start()
    {
        player.Pos = PLAYER_POS;
    }

    [SetUp]
    public void SetUp()
    {

    }

    [TearDown]
    public void TearDown()
    {

    }

    [Test]
    [Category("Model")]
    // NOTE: モデルが存在するのかをテスト
    public void ModelExistenceTest()
    {
        Assert.IsNotNull(player);
        Assert.IsNotNull(new EnemyModel(1, 1, 1, 10, EnemyModel.TENDENCY.BLAVE));
    }

    [Test]
    [Category("SearchLength")]
    public void SearchLengthTest()
    {
        const float SEARCH_LENGTH = 1f;

        EnemyModel blaveEnemy = new EnemyModel(0, 0, 0, SEARCH_LENGTH, EnemyModel.TENDENCY.BLAVE);
        EnemyModel timidEnemy = new EnemyModel(0, 0, 0, SEARCH_LENGTH, EnemyModel.TENDENCY.TIMID);
        EnemyModel vulgarEnemy = new EnemyModel(0, 0, 0, SEARCH_LENGTH, EnemyModel.TENDENCY.VULGAR);

        // NOTE: サーチ距離より近いと行動をすること確認
        blaveEnemy.Pos = new Vector3(0, 0, 0);
        timidEnemy.Pos = new Vector3(0, 0, 0);
        vulgarEnemy.Pos = new Vector3(0, 0, 0);
        Assert.IsFalse(behavior.GetBehavior(player, blaveEnemy) == EnemyBehaviorModel.Behavior.StandBy);
        Assert.IsFalse(behavior.GetBehavior(player, timidEnemy) == EnemyBehaviorModel.Behavior.StandBy);
        Assert.IsFalse(behavior.GetBehavior(player, vulgarEnemy) == EnemyBehaviorModel.Behavior.StandBy);

        // NOTE: サーチ距離より遠いとも行動をしないこと確認
        blaveEnemy.Pos = new Vector3(2, 0, 0);
        timidEnemy.Pos = new Vector3(2, 0, 0);
        vulgarEnemy.Pos = new Vector3(2, 0, 0);
        Assert.IsTrue(behavior.GetBehavior(player, blaveEnemy) == EnemyBehaviorModel.Behavior.StandBy);
        Assert.IsTrue(behavior.GetBehavior(player, timidEnemy) == EnemyBehaviorModel.Behavior.StandBy);
        Assert.IsTrue(behavior.GetBehavior(player, vulgarEnemy) == EnemyBehaviorModel.Behavior.StandBy);
    }

    [Test]
    [Category("BlaveEnemy")]
    public void BlaveEnemyTest()
    {
        List<EnemyModel> enemys = new List<EnemyModel>();

        for (int power = 50; power < 150; power++)
        {
            for (int speed = 50; speed < 150; speed++)
            {
                for (int hp = 50; hp < 150; hp++)
                {
                    enemys.Add(new EnemyModel(power, speed, hp, 10, EnemyModel.TENDENCY.BLAVE));
                }
            }
        }

        // NOTE: 距離が近いとどんな状態でも攻撃するのかを確認
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].Pos = new Vector3(1, 0, 0);
            Assert.IsTrue(behavior.GetBehavior(player, enemys[i]) == EnemyBehaviorModel.Behavior.Attack);
        }
    }

    [Test]
    [Category("TimidEnemy")]
    public void TimidEnemyTest()
    {
        List<EnemyModel> enemys = new List<EnemyModel>();

        for (int power = 50; power < 150; power++)
        {
            for (int speed = 50; speed < 150; speed++)
            {
                for (int hp = 50; hp < 150; hp++)
                {
                    enemys.Add(new EnemyModel(power, speed, hp, 10, EnemyModel.TENDENCY.TIMID));
                }
            }
        }

        // NOTE: 距離が近いとどんな状態でも逃げるのかを確認
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].Pos = new Vector3(1, 0, 0);
            Assert.IsTrue(behavior.GetBehavior(player, enemys[i]) == EnemyBehaviorModel.Behavior.Escape);
        }
    }

    [Test]
    [Category("VulgarEnemy")]
    public void VulgarEnemyTest()
    {
        int enemyPowerCriterion = (int)(PLAYER_POWER * EnemyBehaviorModel.POWER_CRITERION);
        int enemyHpCriterion = (int)(PLAYER_HP * EnemyBehaviorModel.HP_CRITERION);

        EnemyModel lowPowerHighHpEnemy = new EnemyModel(enemyPowerCriterion - 10, 0, enemyHpCriterion + 10, 10, EnemyModel.TENDENCY.VULGAR);
        EnemyModel highPowerLowHpEnemy = new EnemyModel(enemyPowerCriterion + 10, 0, enemyHpCriterion - 10, 10, EnemyModel.TENDENCY.VULGAR);
        EnemyModel lowPowerLowHpEnemy = new EnemyModel(enemyPowerCriterion - 10, 0, enemyHpCriterion - 10, 10, EnemyModel.TENDENCY.VULGAR);
        EnemyModel highPowerHighHpEnemy = new EnemyModel(enemyPowerCriterion + 10, 0, enemyHpCriterion + 10, 10, EnemyModel.TENDENCY.VULGAR);

        // NOTE: 基準値以上のPower、Hpじゃないと逃げる。両方が以上だと攻撃する。
        Assert.IsTrue(behavior.GetBehavior(player, lowPowerHighHpEnemy) == EnemyBehaviorModel.Behavior.Escape);
        Assert.IsTrue(behavior.GetBehavior(player, highPowerLowHpEnemy) == EnemyBehaviorModel.Behavior.Escape);
        Assert.IsTrue(behavior.GetBehavior(player, lowPowerLowHpEnemy) == EnemyBehaviorModel.Behavior.Escape);
        Assert.IsTrue(behavior.GetBehavior(player, highPowerHighHpEnemy) == EnemyBehaviorModel.Behavior.Attack);
    }
}
