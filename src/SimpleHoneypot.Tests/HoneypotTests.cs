﻿namespace SimpleHoneypot.Tests {
    using System;

    using SimpleHoneypot.Core;

    using Xunit;

    public class HoneypotTests {
        #region Public Methods and Operators

        [Fact]
        public void SetAutomaticallyHandleBots_ShouldSetAutomaticallyHandleBots_ToPassedBool() {
            Honeypot.SetManuallyHandleBots(true);
            Assert.True(Honeypot.ManuallyHandleBots);
        }

        [Fact]
        public void SetCssClassName_ShouldSetTheCssClassNameProperty_WhenPassedAValidString() {
            const string str = "MockCssClass";

            Assert.Equal("input-imp-long", Honeypot.CssClassName);
            Honeypot.SetCssClassName(str);

            Assert.Equal(str, Honeypot.CssClassName);
        }

        [Fact]
        public void SetCssClassName_ShouldThrowArgumentException_WhenPassedNullOrEmptyString() {
            Assert.Throws<ArgumentException>(() => Honeypot.SetCssClassName(null));
            Assert.Throws<ArgumentException>(() => Honeypot.SetCssClassName(String.Empty));
        }

        [Fact]
        public void SetDefaultInputName_ShouldSetTheDefaultInputNameProperty_WhenPassedAValidString() {
            const string str = "MockInputName";

            Assert.Equal("Phone-Data-Home", Honeypot.DefaultInputName);
            Honeypot.SeDefaultInputName(str);

            Assert.Equal(str, Honeypot.DefaultInputName);
        }

        [Fact]
        public void SetDefaultInputName_ShouldThrowArgumentException_WhenPassedNullOrEmptyString() {
            Assert.Throws<ArgumentException>(() => Honeypot.SeDefaultInputName(null));
            Assert.Throws<ArgumentException>(() => Honeypot.SeDefaultInputName(String.Empty));
        }

        [Fact]
        public void SetDefaults_ShouldSetCorrectDefaults() {
            Honeypot.SetDefaults();
            Assert.False(Honeypot.ManuallyHandleBots);
            Assert.Equal(Honeypot.DefaultInputName, HoneypotData.DefaultFieldName);
            Assert.Equal(Honeypot.CssClassName, HoneypotData.DefaultCssClassName);
        }

        #endregion
    }
}