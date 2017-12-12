using System;
using FsCheck.Xunit;
using Xunit;
using FsCheck;
using System.Text;

namespace fscheckdemo
{
    public class UnitTest1
    {
        [MD5PropertyAttribute]
        public void MD5ReturnsStringOfCorrectLength(string input)
        {
            var hash = MD5(input);
            Assert.Equal(32, hash.Length);
        }

        [MD5PropertyAttribute]
        public void MD5ReturnsStringDifferentFromInput(string input)
        {
            var hash = MD5(input);
            Assert.NotEqual(input, hash);
        }

        [MD5PropertyAttribute]
        public void MD5ReturnsStringWithOnlyAlphaNumericCharacters(string input)
        {
            var hash = MD5(input);
            var allowed = "0123456789abcdef".ToCharArray();
            Assert.All(hash, c => Assert.Contains(c, allowed));
        }

        [MD5PropertyAttribute]
        public void MD5ReturnsSameHashForSameInput(string input)
        {
            var hash1 = MD5(input);
            var hash2 = MD5(input);
            Assert.Equal(hash1, hash2);
        }

        public static string MD5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create()) 
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                var hash = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();

                for (var i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                return sb.ToString();    
            }
        }
    }
    public class MD5PropertyAttribute : PropertyAttribute
    {
        public MD5PropertyAttribute() => Arbitrary = new[] { typeof(NonNullStringArbitrary) };
    }
    public static class NonNullStringArbitrary
    {
        public static Arbitrary<string> Strings()
        {
            return Arb.Default.String().Filter(x => x != null);
        }
    } 
}
