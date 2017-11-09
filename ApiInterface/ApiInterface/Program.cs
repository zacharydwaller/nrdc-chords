using System;

namespace ApiInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiInterface api = new ApiInterface();

            api.GetDataAsync();

            Console.In.Read();
        }
    }
}
