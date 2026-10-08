namespace Bootcamp.Support.Utils
{
    public static class Utils
    {
        public static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();

            string randomString = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }

        public static int GeneratePositiveRandomInt()
        {
            byte[] buffer = new byte[8];
            Random random = new Random();
            random.NextBytes(buffer);
            int randomNumber = BitConverter.ToInt32(buffer, 0);
            return Math.Abs(randomNumber);
        }

        public static string GenerateRandomEmail()
        {
            string email = GenerateRandomString(5) + "." + GenerateRandomString(8) + "@sandfieldautomation.co.nz";
            return email;
        }
    }
}
