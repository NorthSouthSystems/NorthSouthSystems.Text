namespace FOSStrich.Text;

public class StringFieldWrapperTests
{
    [Fact]
    public void Basic()
    {
        StringFieldWrapper wrapper;

        wrapper = new("A", "1");
        wrapper.ColumnName.Should().Be("A");
        wrapper.Value.Should().Be("1");
        wrapper.ToString().Should().Be("1");

        wrapper = new("A", null);
        wrapper.ToString().Should().BeEmpty();
    }

    [Fact]
    public void ConversionBool()
    {
        StringFieldWrapper wrapper;

        wrapper = new("A", "TRUE");
        ((string)wrapper).Should().Be("TRUE");
        ((bool)wrapper).Should().BeTrue();
        ((bool?)wrapper).Value.Should().BeTrue();

        wrapper = new("A", "true");
        ((string)wrapper).Should().Be("true");
        ((bool)wrapper).Should().BeTrue();
        ((bool?)wrapper).Value.Should().BeTrue();

        wrapper = new("A", "FALSE");
        ((string)wrapper).Should().Be("FALSE");
        ((bool)wrapper).Should().BeFalse();
        ((bool?)wrapper).Value.Should().BeFalse();
    }

    [Fact]
    public void ConversionNumeric()
    {
        StringFieldWrapper wrapper;

        wrapper = new("A", "7");
        ((string)wrapper).Should().Be("7");
        ((int)wrapper).Should().Be(7);
        ((int?)wrapper).Value.Should().Be(7);
        ((uint)wrapper).Should().Be(7u);
        ((uint?)wrapper).Value.Should().Be(7u);
        ((long)wrapper).Should().Be(7L);
        ((long?)wrapper).Value.Should().Be(7L);
        ((ulong)wrapper).Should().Be(7ul);
        ((ulong?)wrapper).Value.Should().Be(7ul);
        ((float)wrapper).Should().Be(7f);
        ((float?)wrapper).Value.Should().Be(7f);
        ((double)wrapper).Should().Be(7d);
        ((double?)wrapper).Value.Should().Be(7d);
        ((decimal)wrapper).Should().Be(7m);
        ((decimal?)wrapper).Value.Should().Be(7m);

        wrapper = new("A", "7000000000");
        ((string)wrapper).Should().Be("7000000000");
        ((long)wrapper).Should().Be(7000000000L);
        ((long?)wrapper).Value.Should().Be(7000000000L);
        ((ulong)wrapper).Should().Be(7000000000ul);
        ((ulong?)wrapper).Value.Should().Be(7000000000ul);

        wrapper = new("A", "7.7");
        ((string)wrapper).Should().Be("7.7");
        ((float)wrapper).Should().Be(7.7f);
        ((float?)wrapper).Value.Should().Be(7.7f);
        ((double)wrapper).Should().Be(7.7d);
        ((double?)wrapper).Value.Should().Be(7.7d);
        ((decimal)wrapper).Should().Be(7.7m);
        ((decimal?)wrapper).Value.Should().Be(7.7m);
    }

    [Fact]
    public void ConversionDateTime()
    {
        StringFieldWrapper wrapper;

        var now = DateTime.Now;
        var nowOffset = DateTimeOffset.Now;

        wrapper = new("A", now.ToString("o"));
        ((string)wrapper).Should().Be(now.ToString("o"));
        ((DateTime)wrapper).Should().Be(now);
        ((DateTime?)wrapper).Value.Should().Be(now);

        wrapper = new("A", nowOffset.ToString("o"));
        ((string)wrapper).Should().Be(nowOffset.ToString("o"));
        ((DateTimeOffset)wrapper).Should().Be(nowOffset);
        ((DateTimeOffset?)wrapper).Value.Should().Be(nowOffset);

        var ts = now - now.Date;
        ts = new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds);
        string tsXsd = string.Format("PT{0}H{1}M{2}S", ts.Hours, ts.Minutes, ts.Seconds);

        wrapper = new("A", tsXsd);
        ((string)wrapper).Should().Be(tsXsd);
        ((TimeSpan)wrapper).Should().Be(ts);
        ((TimeSpan?)wrapper).Value.Should().Be(ts);
    }

    [Fact]
    public void ConversionGuid()
    {
        StringFieldWrapper wrapper;

        Guid guid = Guid.NewGuid();

        wrapper = new("A", guid.ToString());
        ((string)wrapper).Should().Be(guid.ToString());
        ((Guid)wrapper).Should().Be(guid);
        ((Guid?)wrapper).Value.Should().Be(guid);
    }

    [Fact]
    public void ConversionIsNull()
    {
        StringFieldWrapper wrapper;

        wrapper = null;
        ((string)wrapper).Should().BeNull();
        ShouldBeNull(null);

        wrapper = new("A", null);
        ((string)wrapper).Should().BeNull();
        ShouldBeNull(wrapper);

        wrapper = new("A", string.Empty);
        ShouldBeNull(wrapper);

        wrapper = new("A", " ");
        ShouldBeNull(wrapper);

        wrapper = new("A", "  ");
        ShouldBeNull(wrapper);

        static void ShouldBeNull(StringFieldWrapper w)
        {
            ((bool?)w).Should().BeNull();
            ((int?)w).Should().BeNull();
            ((uint?)w).Should().BeNull();
            ((long?)w).Should().BeNull();
            ((ulong?)w).Should().BeNull();
            ((float?)w).Should().BeNull();
            ((double?)w).Should().BeNull();
            ((decimal?)w).Should().BeNull();
            ((DateTime?)w).Should().BeNull();
            ((DateTimeOffset?)w).Should().BeNull();
            ((TimeSpan?)w).Should().BeNull();
            ((Guid?)w).Should().BeNull();
        }
    }

    [Fact]
    public void Exceptions()
    {
        Action act;
        object x;

        // wrapper = null

        act = () => x = (bool)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (int)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (uint)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (long)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (ulong)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (float)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (double)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (decimal)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (DateTime)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (DateTimeOffset)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (TimeSpan)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (Guid)((StringFieldWrapper)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        // field = null

        act = () => x = (bool)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (int)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (uint)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (long)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (ulong)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (float)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (double)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (decimal)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (DateTime)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (DateTimeOffset)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (TimeSpan)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => x = (Guid)(new StringFieldWrapper("A", null));
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}