using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new SignatureModel()
            {
                AuthType = 1,
                NeedTag = 1,
                SceneId = "sceneId",
                Source = 2,
                UserAppId = 123123
            };
            string tpToken = "token";

            string signature = SignatureGeneratorV2.GenerateSignature(model, tpToken);
            string expected = SignatureGenerator.GenerateSignature(model, tpToken);

            Console.WriteLine($"expected : {expected}");
            Console.WriteLine($"signature: {signature}, match: {signature == expected}");
        }
    }
}
