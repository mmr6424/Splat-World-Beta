using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Assertions;

public class TestScript
{
    public void ToggleTest () {
        // Assign
        var toolToggle = new ToolToggle();
        // Act
        toolToggle.ActivateBrush();
        // Assert
        Assert.IsTrue(toolToggle.brush.activeSelf);
        Assert.IsFalse(toolToggle.can.activeSelf);
        Assert.IsFalse(toolToggle.bucket.activeSelf);

        // Act
        toolToggle.ActivateCan();
        // Assert
        Assert.IsTrue(toolToggle.can.activeSelf);
        Assert.IsFalse(toolToggle.brush.activeSelf);
        Assert.IsFalse(toolToggle.bucket.activeSelf);

        // Act
        toolToggle.ActivateBucket();
        // Assert
        Assert.IsTrue(toolToggle.bucket.activeSelf);
        Assert.IsFalse(toolToggle.brush.activeSelf);
        Assert.IsFalse(toolToggle.can.activeSelf);
    }
}