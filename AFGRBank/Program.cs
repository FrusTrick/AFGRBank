using AFGRBank.Utility;
using AFGRBank.Main;

namespace AFGRBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankingMain menus = new BankingMain();

            menus.MainMenu();
            Console.WriteLine("Hello, World!");
        }
    }
}
