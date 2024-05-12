using Bot.scripts;

namespace Healthbot_tests
{
    [TestClass]
    public class UnitTest1
    {
        public static IEnumerable<object[]> CorrectStringDates { 
            get
            {
                return new[]
                {
                    new object[] {"10.10.2000-12.12.2222", new DateTime(2000, 10,10).ToUniversalTime(), new DateTime(2222, 12, 12).ToUniversalTime() },
                    new object[] {"10.10.2000-12.12.2222", new DateTime(2000, 10, 10).ToUniversalTime(), new DateTime(2222, 12, 12).ToUniversalTime() }
                };
            }
        }

        public static IEnumerable<object[]> IncorrectStringDates { 
            get
            {
                return new[]
                {
                    new object[] {"" },
                    new object[] {"11.5-12345.10" },
                    new object[] {"1+2"},
                    new object[] {@"According to all known laws,
                                of aviation,
                                there is no way a bee
                                should be able to fly.
                                Its wings are too small to get
                                its fat little body off the ground.
                                The bee, of course, flies anyway
                                because bees don't care
                                what humans think is impossible."},
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(CorrectStringDates))]
        public void Command_Help_ParseDates_Correct_Data_Expect_Success(string Input, DateTime Expected1, DateTime Expected2)
        {
            var result = Command.Help.ParseDates(Input);
            Assert.AreEqual(Expected1, result.min);
            Assert.AreEqual(Expected2, result.max);
        }

        [TestMethod]
        [DynamicData(nameof(IncorrectStringDates))]
        public void Command_Help_ParseDates_Incorrect_Data_Expect_Fail(string Input)
        {
            try
            {
                Command.Help.ParseDates(Input);
                Assert.Fail("");
            }
            catch
            {
                Assert.AreEqual(1,1);
            }
        }
    }
}