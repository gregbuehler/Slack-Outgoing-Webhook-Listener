using System;
using System.Collections.Generic;

namespace SuperMarioPivotalEdition.Clients
{
    class FractalClient
    {
        private Random _random;
        private List<string> _types;
         
        public FractalClient()
        {
            _random = new Random();
            _types = new List<string>()
            {
                "schroder",
                "newton",
                "halley",
                "householder",
                "secant"
            };
        }

        public string RandomFractal()
        {
            var randomType = _types[_random.Next(_types.Count)];
            var coefs = "";
            for (int i = 0; i < _random.Next(4, 9); i++)
            {
                coefs += (0.5 - _random.NextDouble()).ToString("F2");
                coefs += "/";
            }
            return $"http://pareidoliaiscreated.org:8001/fractal/{randomType}/0/1/1/{coefs}medium.png";
        }
    }
}
