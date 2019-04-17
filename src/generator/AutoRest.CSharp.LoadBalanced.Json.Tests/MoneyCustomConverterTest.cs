using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AutoRest.CSharp.LoadBalanced.Json.Tests
{
    [TestFixture]
    public class MoneyCustomConverterTest
    {
        [Test, TestCaseSource(nameof(GetMoneySerializationTestCases))]
        public void MoneySerializationTest(string description, TestObject testObject, string expectedJson)
        {
            Assert.AreEqual(JsonConvert.SerializeObject(testObject), expectedJson);
        }

        [Test, TestCaseSource(nameof(GetDeserializationTestCases))]
        public void MoneyDeserializationTest(string description, TestObject testObject, string jsonText)
        {
            var deserializedObject = JsonConvert.DeserializeObject<TestObject>(jsonText);
            Assert.AreEqual(testObject.NotNullableMoney, deserializedObject.NotNullableMoney);
            Assert.AreEqual(testObject.NullableTextMyMoney, deserializedObject.NullableTextMyMoney);
            Assert.AreEqual(testObject.NullableMoney, deserializedObject.NullableMoney);
            Assert.AreEqual(testObject.NotNullableMoney, deserializedObject.NotNullableMoney);;
        }

        public static IEnumerable<object[]> GetMoneySerializationTestCases()
        {
            yield return new object[] { "test #1", Create(1, 2, 22, 122), 
                "{\"notNullableTextMoney\":\"1\",\"nullableTextMyMoney\":\"2\",\"nullableMoney\":22.0,\"notNullableMoney\":122.0}" };

            yield return new object[] { "test #2", Create(123.4567M, 123.4M, 123.0M, 122.53M), 
                "{\"notNullableTextMoney\":\"123.46\",\"nullableTextMyMoney\":\"123.40\",\"nullableMoney\":123.0,\"notNullableMoney\":122.53}" };

            yield return new object[] { "test #3", Create(0, null, 0, 0), 
                "{\"notNullableTextMoney\":\"0\",\"nullableTextMyMoney\":null,\"nullableMoney\":0.0,\"notNullableMoney\":0.0}" };

            yield return new object[] { "scientific", Create(1.25E7m, 2.25E7m, 3.25E7m, 4.25E7m), 
                "{\"notNullableTextMoney\":\"12500000\",\"nullableTextMyMoney\":\"22500000\",\"nullableMoney\":32500000.0,\"notNullableMoney\":42500000.0}" };
        }

        public static IEnumerable<object[]> GetDeserializationTestCases()
        {
            yield return new object[] { "numeric #1", Create(1, 2, 22, 122), 
                "{\"notNullableTextMoney\":1,\"nullableTextMyMoney\":2,\"nullableMoney\":22.0,\"notNullableMoney\":122.0}" };

            yield return new object[] { "text #1", Create(1, 2, 22, 122), 
                "{\"notNullableTextMoney\":\"1\",\"nullableTextMyMoney\":\"2\",\"nullableMoney\":\"22.0\",\"notNullableMoney\":\"122.0\"}" };

            yield return new object[] { "numeric #2", Create(1.12M, 2.34M, 22.22M, 122.53M), 
                "{\"notNullableTextMoney\":\"1.12\",\"nullableTextMyMoney\":\"2.34\",\"nullableMoney\":\"22.22\",\"notNullableMoney\":122.53}" };

            yield return new object[] { "text #2", Create(1.12M, 2.34M, 22.22M, 122.53M), 
                "{\"notNullableTextMoney\":1.12,\"nullableTextMyMoney\":2.34,\"nullableMoney\":22.22,\"notNullableMoney\":122.53}" };

            yield return new object[] { "numeric #3", Create(0, null, 0, 0), 
                "{\"notNullableTextMoney\":0,\"nullableTextMyMoney\":null,\"nullableMoney\":0.0,\"notNullableMoney\":0.0}" };

            yield return new object[] { "text #3", Create(0, null, 0, 0), 
                "{\"notNullableTextMoney\":\"0\",\"nullableTextMyMoney\":null,\"nullableMoney\":\"0.0\",\"notNullableMoney\":\"0.0\"}" };            

            yield return new object[] { "scientific", Create(1.25E7m, 2.25E7m, 3.25E7m, 4.25E7m), 
                "{\"notNullableTextMoney\":\"1.25E7\",\"nullableTextMyMoney\":\"2.25E7\",\"nullableMoney\":\"3.25E7\",\"notNullableMoney\":\"4.25E7\"}" };
        }

        protected static TestObject Create(decimal notNullableTextMoney, decimal? nullableTextMyMoney, decimal nullableMoney, decimal notNullableMoney)
        {
            return new TestObject
            {
                NotNullableTextMoney = notNullableTextMoney,
                NullableTextMyMoney = nullableTextMyMoney,
                NullableMoney = nullableMoney,
                NotNullableMoney = notNullableMoney
            };
        }

        public class TestObject
        {
            [JsonProperty("notNullableTextMoney"), JsonConverter(typeof(MoneyConverter), MoneyConverterOptions.SendAsText)]
            public decimal NotNullableTextMoney { get; set; }

            [JsonProperty("nullableTextMyMoney"), JsonConverter(typeof(MoneyConverter), MoneyConverterOptions.SendAsText | MoneyConverterOptions.IsNullable)]
            public decimal? NullableTextMyMoney { get; set; }

            [JsonProperty("nullableMoney"), JsonConverter(typeof(MoneyConverter), MoneyConverterOptions.IsNullable)]
            public decimal NullableMoney { get; set; }

            [JsonProperty("notNullableMoney"), JsonConverter(typeof(MoneyConverter))]
            public decimal NotNullableMoney { get; set; }
        }
    }
}
