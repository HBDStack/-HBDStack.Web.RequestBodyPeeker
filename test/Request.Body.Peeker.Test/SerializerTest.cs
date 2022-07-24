using HBDStack.Web.BodyPeeker;
using NUnit.Framework;

namespace Request.Body.Peeker.Test
{
    public class SerializerTest
    {
        [Test]
        public void Deserialize()
        {
            var serializer = new DefaultSerializer();

            var dummy = new DummyClass()
            {
                Name = "ali",
                SurName = "alp"
            };

            var serialized = serializer.Serialize(dummy);
            var deserializeObject = serializer.Deserialize<DummyClass>(serialized);
            
            Assert.AreEqual(0,deserializeObject.CompareTo(dummy));
        }
    }
}