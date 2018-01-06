﻿using NUnit.Framework;

[TestFixture]
public class Test
{
    private void Start()
    {
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
    }
}
