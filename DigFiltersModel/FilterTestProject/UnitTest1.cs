using NUnit.Framework;
using DigFiltersModel;

namespace FilterTestProject
{
    public class Tests
    {
        DFMSignal impulseInput=new DFMSignal("impulse", 1);
        DFMSignal variableSignal=new DFMSignal("variable", 1,0,2,-1,2,1,-3,0,1);
        DFMCoefList upper1 = new DFMCoefList(1, 0);
        DFMCoefList upper2 = new DFMCoefList(2);
        DFMCoefList upper3 = new DFMCoefList(0,1);
        DFMCoefList lower1 = new DFMCoefList(1, 0);
        DFMCoefList lower2 = new DFMCoefList(1, -0.5);
        DFMFilter identityFilter;
        DFMFilter doubleFilter;
        DFMFilter delayFilter;
        DFMFilter fadeFilter;
        [SetUp]
        public void Setup()
        {
            identityFilter = new DFMFilter(upper1, lower1);
            doubleFilter = new DFMFilter(upper2, lower1);
            delayFilter = new DFMFilter(upper3, lower1);
            fadeFilter = new DFMFilter(upper1, lower2);
        }

        [Test]
        public void LengthTest()
        {
            var output=identityFilter.GenerateOutput(impulseInput, 5);
            Assert.AreEqual(5, output.Length());
        }
        [Test]
        public void IdentityFilterTest()
        {
            var output = identityFilter.GenerateOutput(impulseInput, 5);
            Assert.AreEqual(1, output[0]);
            Assert.AreEqual(0, output[1]);
            Assert.AreEqual(0, output[2]);
            Assert.AreEqual(0, output[3]);
            Assert.AreEqual(0, output[4]);
            Assert.IsTrue(output.IsEqual(impulseInput));
        }
        [Test]
        public void DoubleFilterTest()
        {
            var output = doubleFilter.GenerateOutput(variableSignal, 10);
            var output2 = doubleFilter.GenerateOutput(variableSignal, 5);
            Assert.AreEqual(2, output[0]);
            Assert.AreEqual(0, output[1]);
            Assert.AreEqual(4, output[2]);
            Assert.AreEqual(-2, output[3]);
            Assert.AreEqual(4, output[4]);
            Assert.IsTrue(output.IsEqual(2*variableSignal));
            Assert.IsTrue(output2.IsEqual(2 * variableSignal, 5));
        }
        [Test]
        public void DelayFilterTest()
        {
            var output = delayFilter.GenerateOutput(variableSignal, 10);
            var output2 = delayFilter.GenerateOutput(output, 10);
            Assert.AreEqual(0, output[0]);
            Assert.AreEqual(1, output[1]);
            Assert.AreEqual(0, output[2]);
            Assert.AreEqual(2, output[3]);
            Assert.AreEqual(-1, output[4]);
            Assert.IsTrue(output.IsEqual(variableSignal.RightShift(1)));
            Assert.IsTrue(output2.IsEqual(variableSignal.RightShift(2), 5));
        }
        [Test]
        public void FadeFilterTest()
        {
            var output = fadeFilter.GenerateOutput(4*impulseInput, 10);
            Assert.AreEqual(4, output[0]);
            Assert.AreEqual(2, output[1]);
            Assert.AreEqual(1, output[2]);
            Assert.AreEqual(0, output[3]);
            Assert.AreEqual(0, output[4]);
        }
    }
}