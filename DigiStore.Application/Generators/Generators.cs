using System;


namespace DigiStore.Application.Generators
{
    public class Generators
    {
        public static string GeneratorsUniqueCode()
        {
            return Guid.NewGuid().ToString("N").Replace("-", "");
        }
    }
}
