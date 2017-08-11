using System;
using Xunit;

namespace GB2260.Test
{
    public class Gb2260Test
    {
        public readonly Gb2260 Gb;
        public Gb2260Test()
        {
            Gb = Gb2260Factory.Create();
        }
        [Fact]
        public void TestConstructor()
        {
            var gb = new Gb2260();
            Assert.Equal(Revision.V201607, gb.Revision);
            Assert.NotNull(gb.Provinces);
        }
        [Fact]
        public void TestFlyweight()
        {
            var gb1 = Gb2260Factory.Create(Revision.V201010);
            var gb2 = Gb2260Factory.Create(Revision.V201010);
            Assert.NotNull(gb1);
            Assert.Same(gb1, gb2);
            var gb3 = Gb2260Factory.Create(Revision.V200212);
            Assert.NotSame(gb1, gb3);
        }
        [Fact]
        public void TestGetDivision()
        {
            var code = "110105";
            var result = Gb.GetDivision(code);
            Assert.Equal(code, result.Code);
        }
        [Fact]
        public void TestGetPrefectures()
        {
            var code = "310000";
            var result = Gb.GetPrefectures(code);
            Assert.True(result.Count>0);
        }
        [Fact]
        public void TestGetCounties()
        {
            var code = "310100";
            var result = Gb.GetCounties(code);
            Assert.True(result.Count > 0);
        }
    }
}
