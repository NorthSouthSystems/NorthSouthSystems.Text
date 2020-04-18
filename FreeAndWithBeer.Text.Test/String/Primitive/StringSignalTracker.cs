namespace FreeAndWithBeer.Text
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class StringSignalTrackerTests
    {
        [TestMethod]
        public void Empty()
        {
            StringSignalTracker tracker = new StringSignalTracker(string.Empty);
            EmptyBase(tracker);
            tracker.Reset();
            EmptyBase(tracker);

            tracker = new StringSignalTracker(null);
            EmptyBase(tracker);
            tracker.Reset();
            EmptyBase(tracker);
        }

        private void EmptyBase(StringSignalTracker tracker)
        {
            Assert.AreEqual(string.Empty, tracker.Signal);

            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);
        }

        [TestMethod]
        public void SingleChar()
        {
            StringSignalTracker tracker = new StringSignalTracker("a");
            SingleCharBase(tracker);
            tracker.Reset();
            SingleCharBase(tracker);
        }

        private void SingleCharBase(StringSignalTracker tracker)
        {
            Assert.AreEqual("a", tracker.Signal);

            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('b');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsTrue(tracker.IsTriggered);
        }

        [TestMethod]
        public void MultiCharSimple()
        {
            StringSignalTracker tracker = new StringSignalTracker("ab");
            MultiCharSimpleBase(tracker);
            tracker.Reset();
            MultiCharSimpleBase(tracker);
        }

        private void MultiCharSimpleBase(StringSignalTracker tracker)
        {
            Assert.AreEqual("ab", tracker.Signal);

            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('b');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('c');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('b');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsTrue(tracker.IsTriggered);
        }

        [TestMethod]
        public void MultiCharComplex()
        {
            StringSignalTracker tracker = new StringSignalTracker("abac");
            MultiCharComplexBase(tracker);
            tracker.Reset();
            MultiCharComplexBase(tracker);
        }

        private void MultiCharComplexBase(StringSignalTracker tracker)
        {
            Assert.AreEqual("abac", tracker.Signal);

            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('b');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('c');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('b');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('b');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('a');
            Assert.IsTrue(tracker.IsCounting);
            Assert.IsFalse(tracker.IsTriggered);

            tracker.ProcessChar('c');
            Assert.IsFalse(tracker.IsCounting);
            Assert.IsTrue(tracker.IsTriggered);
        }

        #region Exceptions

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ProcessCharAlreadyTriggered()
        {
            StringSignalTracker tracker = new StringSignalTracker("a");
            tracker.ProcessChar('a');
            tracker.ProcessChar(' ');
        }

        #endregion
    }
}