﻿using NUnit.Framework;

[TestFixture]
public class Test {
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
    public void ModelTest()
    {
        Assert.AreEqual(0, new PlayerModel(1).Power);
    }
}
