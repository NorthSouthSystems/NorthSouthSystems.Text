namespace FOSStrich.Text;

[TestClass]
public class StringFieldWrapperTests
{
    [TestMethod]
    public void Basic()
    {
        var wrapper = new StringFieldWrapper("A", "1");
        Assert.AreEqual("A", wrapper.ColumnName);
        Assert.AreEqual("1", wrapper.Value);
        Assert.AreEqual("1", wrapper.ToString());

        wrapper = new StringFieldWrapper("A", null);
        Assert.AreEqual(string.Empty, wrapper.ToString());
    }

    [TestMethod]
    public void ConversionBool()
    {
        var wrapper = new StringFieldWrapper("A", "TRUE");
        Assert.AreEqual("TRUE", (string)wrapper);
        Assert.IsTrue((bool)wrapper);
        Assert.IsTrue(((bool?)wrapper).Value);

        wrapper = new StringFieldWrapper("A", "true");
        Assert.AreEqual("true", (string)wrapper);
        Assert.IsTrue((bool)wrapper);
        Assert.IsTrue(((bool?)wrapper).Value);

        wrapper = new StringFieldWrapper("A", "FALSE");
        Assert.AreEqual("FALSE", (string)wrapper);
        Assert.IsFalse((bool)wrapper);
        Assert.IsFalse(((bool?)wrapper).Value);
    }

    [TestMethod]
    public void ConversionNumeric()
    {
        var wrapper = new StringFieldWrapper("A", "7");
        Assert.AreEqual("7", (string)wrapper);
        Assert.AreEqual(7, (int)wrapper);
        Assert.AreEqual(7, ((int?)wrapper).Value);
        Assert.AreEqual(7u, (uint)wrapper);
        Assert.AreEqual(7u, ((uint?)wrapper).Value);
        Assert.AreEqual(7L, (long)wrapper);
        Assert.AreEqual(7L, ((long?)wrapper).Value);
        Assert.AreEqual(7ul, (ulong)wrapper);
        Assert.AreEqual(7ul, ((ulong?)wrapper).Value);
        Assert.AreEqual(7f, (float)wrapper);
        Assert.AreEqual(7f, ((float?)wrapper).Value);
        Assert.AreEqual(7d, (double)wrapper);
        Assert.AreEqual(7d, ((double?)wrapper).Value);
        Assert.AreEqual(7m, (decimal)wrapper);
        Assert.AreEqual(7m, ((decimal?)wrapper).Value);

        wrapper = new StringFieldWrapper("A", "7000000000");
        Assert.AreEqual("7000000000", (string)wrapper);
        Assert.AreEqual(7000000000L, (long)wrapper);
        Assert.AreEqual(7000000000L, ((long?)wrapper).Value);
        Assert.AreEqual(7000000000ul, (ulong)wrapper);
        Assert.AreEqual(7000000000ul, ((ulong?)wrapper).Value);

        wrapper = new StringFieldWrapper("A", "7.7");
        Assert.AreEqual("7.7", (string)wrapper);
        Assert.AreEqual(7.7f, (float)wrapper);
        Assert.AreEqual(7.7f, ((float?)wrapper).Value);
        Assert.AreEqual(7.7d, (double)wrapper);
        Assert.AreEqual(7.7d, ((double?)wrapper).Value);
        Assert.AreEqual(7.7m, (decimal)wrapper);
        Assert.AreEqual(7.7m, ((decimal?)wrapper).Value);
    }

    [TestMethod]
    public void ConversionDateTime()
    {
        DateTime now = DateTime.Now;
        DateTimeOffset nowOffset = DateTimeOffset.Now;

        var wrapper = new StringFieldWrapper("A", now.ToString("o"));
        Assert.AreEqual(now.ToString("o"), (string)wrapper);
        Assert.AreEqual(now, (DateTime)wrapper);
        Assert.AreEqual(now, ((DateTime?)wrapper).Value);

        wrapper = new StringFieldWrapper("A", nowOffset.ToString("o"));
        Assert.AreEqual(nowOffset.ToString("o"), (string)wrapper);
        Assert.AreEqual(nowOffset, (DateTimeOffset)wrapper);
        Assert.AreEqual(nowOffset, ((DateTimeOffset?)wrapper).Value);

        TimeSpan ts = now - now.Date;
        ts = new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds);
        string tsXsd = string.Format("PT{0}H{1}M{2}S", ts.Hours, ts.Minutes, ts.Seconds);

        wrapper = new StringFieldWrapper("A", tsXsd);
        Assert.AreEqual(tsXsd, (string)wrapper);
        Assert.AreEqual(ts, (TimeSpan)wrapper);
        Assert.AreEqual(ts, ((TimeSpan?)wrapper).Value);
    }

    [TestMethod]
    public void ConversionGuid()
    {
        Guid guid = Guid.NewGuid();

        var wrapper = new StringFieldWrapper("A", guid.ToString());
        Assert.AreEqual(guid.ToString(), (string)wrapper);
        Assert.AreEqual(guid, (Guid)wrapper);
        Assert.AreEqual(guid, ((Guid?)wrapper).Value);
    }

    [TestMethod]
    public void ConversionIsNull()
    {
        AssertIsNull(null);
        Assert.IsNull((string)((StringFieldWrapper)null));

        var wrapper = new StringFieldWrapper("A", null);
        Assert.IsNull((string)wrapper);
        AssertIsNull(wrapper);

        wrapper = new StringFieldWrapper("A", string.Empty);
        AssertIsNull(wrapper);

        wrapper = new StringFieldWrapper("A", " ");
        AssertIsNull(wrapper);

        wrapper = new StringFieldWrapper("A", "  ");
        AssertIsNull(wrapper);
    }

    private void AssertIsNull(StringFieldWrapper wrapper)
    {
        Assert.IsNull((bool?)wrapper);
        Assert.IsNull((int?)wrapper);
        Assert.IsNull((uint?)wrapper);
        Assert.IsNull((long?)wrapper);
        Assert.IsNull((ulong?)wrapper);
        Assert.IsNull((float?)wrapper);
        Assert.IsNull((double?)wrapper);
        Assert.IsNull((decimal?)wrapper);
        Assert.IsNull((DateTime?)wrapper);
        Assert.IsNull((DateTimeOffset?)wrapper);
        Assert.IsNull((TimeSpan?)wrapper);
        Assert.IsNull((Guid?)wrapper);
    }

    #region Exceptions

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionBoolArgumentNull1()
    {
        var x = (bool)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionBoolArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (bool)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionIntArgumentNull1()
    {
        var x = (int)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionIntArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (int)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionUIntArgumentNull1()
    {
        var x = (uint)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionUIntArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (uint)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionLongArgumentNull1()
    {
        var x = (long)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionLongArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (long)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionULongArgumentNull1()
    {
        var x = (ulong)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionULongArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (ulong)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionFloatArgumentNull1()
    {
        var x = (float)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionFloatArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (float)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDoubleArgumentNull1()
    {
        var x = (double)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDoubleArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (double)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDecimalArgumentNull1()
    {
        var x = (decimal)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDecimalArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (decimal)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDateTimeArgumentNull1()
    {
        var x = (DateTime)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDateTimeArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (DateTime)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDateTimeOffsetArgumentNull1()
    {
        var x = (DateTimeOffset)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionDateTimeOffsetArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (DateTimeOffset)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionTimeSpanArgumentNull1()
    {
        var x = (TimeSpan)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionTimeSpanArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (TimeSpan)wrapper;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionGuidArgumentNull1()
    {
        var x = (Guid)((StringFieldWrapper)null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConversionGuidArgumentNull2()
    {
        var wrapper = new StringFieldWrapper("A", null);
        var x = (Guid)wrapper;
    }

    #endregion
}