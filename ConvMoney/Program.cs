using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp1
{

    class valet
    {
        public string r030 { get; set; }
        public string txt { get; set; }
        public string rate { get; set; }
        public string cc { get; set; }
    }

    class Program
    {
        [STAThread]

        static void Main(string[] args)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load("http://bank.gov.ua/NBUStatService/v1/statdirectory/exchange");
            XmlElement xRoot = doc.DocumentElement;
            List<valet> List = new List<valet>();
            foreach (XmlNode node in doc.GetElementsByTagName("currency"))
            {
                valet valet = new valet();
                foreach (XmlNode childnode in node.ChildNodes)
                {
                    if (childnode.Name == "rate")
                        valet.rate = childnode.InnerText;
                    if (childnode.Name == "cc")
                        valet.cc = childnode.InnerText;
                    if (childnode.Name == "txt")
                        valet.txt = childnode.InnerText;
                    if (childnode.Name == "r030")
                        valet.r030 = childnode.InnerText;
                }
                List.Add(valet);
            }

            double tempEUR, tempRUB, tempUSD;
            foreach (var el in List)
            {

                if (el.cc == "EUR")
                {
                    Console.WriteLine("1 EUR = " + el.rate + " UAH ");
                    double.TryParse(el.rate, out tempEUR);
                }

                if (el.cc == "USD")
                {
                    Console.WriteLine("1 USD = " + el.rate + " UAH ");
                    double.TryParse(el.rate, out tempUSD);
                }

                if (el.cc == "RUB")
                {
                    Console.WriteLine("1 RUB = " + el.rate + " UAH ");
                    double.TryParse(el.rate, out tempRUB);
                }

            }


            while (true)
            {
                double UAH;

                try
                {
                    Console.Write("Введите сумму, необходимую перевести: ");
                    UAH = double.Parse(Console.ReadLine());
                    if (UAH < 0)
                    {
                        Console.WriteLine("Данное число нельзя перевести!");
                        Console.ReadLine();
                        continue;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Не удалось преобразовать строку в число! Нажмите Enter для продолжения...");
                    Console.ReadLine();
                    continue;
                }

                Console.Write("Выберите операцию перевода: 'RUB' 'USD' 'EUR' -> 'UAH' : ");
                string action = Console.ReadLine();
                valet v = GetValetByCurrency(action, List);

                switch (action)
                {
                    case "RUB":
                        Console.WriteLine(UAH + " RUB = " + RoundUp(UAH * double.Parse(v.rate, CultureInfo.InvariantCulture), 2) + " UAH");
                        break;

                    case "USD":
                        Console.WriteLine(UAH + " USD = " + RoundUp(UAH * double.Parse(v.rate, CultureInfo.InvariantCulture), 2) + " UAH");
                        break;

                    case "EUR":
                        Console.WriteLine(UAH + " EUR = " + RoundUp(UAH * double.Parse(v.rate, CultureInfo.InvariantCulture), 2) + " UAH");
                        break;
                    default:
                        Console.WriteLine("Не удалось распознать операцию! Нажмите Enter для продолжения...");
                        break;
                }
            }



        }

        static double RoundUp(double number, int digits)
        {
            var factor = Convert.ToDouble(Math.Pow(10, digits));
            var ret = Math.Ceiling(number * factor) / factor;
            return ret;
        }

        static valet GetValetByCurrency(string currency, List<valet> valets)
        {
            foreach (valet v in valets)
            {
                if (v.cc == currency)
                    return v;
            }
            return null;
        }

    }
}