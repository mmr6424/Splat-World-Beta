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
        UnityEngine.Assertions.Assert.IsTrue(toolToggle.brush.activeSelf);
        UnityEngine.Assertions.Assert.IsFalse(toolToggle.can.activeSelf);
        UnityEngine.Assertions.Assert.IsFalse(toolToggle.bucket.activeSelf);

        // Act
        toolToggle.ActivateCan();
        // Assert
        UnityEngine.Assertions.Assert.IsTrue(toolToggle.can.activeSelf);
        UnityEngine.Assertions.Assert.IsFalse(toolToggle.brush.activeSelf);
        UnityEngine.Assertions.Assert.IsFalse(toolToggle.bucket.activeSelf);

        // Act
        toolToggle.ActivateBucket();
        // Assert
        UnityEngine.Assertions.Assert.IsTrue(toolToggle.bucket.activeSelf);
        UnityEngine.Assertions.Assert.IsFalse(toolToggle.brush.activeSelf);
        UnityEngine.Assertions.Assert.IsFalse(toolToggle.can.activeSelf);
    }
}