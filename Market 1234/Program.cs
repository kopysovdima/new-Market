using System;
using System.Collections.Generic;

namespace Market_1234
{
    class Program
    {
        static void Main(string[] args)
        {
            Market market = new Market();
            market.Work();
        }
    }

    class Market
    {
        private static Random _random = new Random();

        private int _money = 0;
        private Queue<Client> _clientsQueue = new Queue<Client>();

        public Market()
        {
            CreateQueueClients();
        }

        public void CreateQueueClients()
        {
            int minValueClients = 5;
            int maxValueClients = 10;

            for (int i = 0; i < _random.Next(minValueClients, maxValueClients); i++)
            {
                _clientsQueue.Enqueue(new Client());
            }
        }

        public void Work()
        {
            while (_clientsQueue.Count > 0)
            {
                Client client = _clientsQueue.Dequeue();
                client.ShowInfo();
                bool isActiv = true;

                while (isActiv) 
                {
                    if(client.TryPay())
                    {
                        _money += client.Basket.GetBusketSum();
                        Console.WriteLine($"Баланс магазина {_money} рублей");
                        isActiv = false;
                    }
                    else
                    {
                        Product product = client.GetBusket().GetProducts()[_random.Next(0, client.GetBusket().GetBusketCount())];

                        Console.WriteLine($"У клиента {client.Money} денег. Не достаточно для совершения покупки, он убрал из корзины {product.Name} стоимостью {product.Cost}");

                        client.GetBusket().RemoveProduct(product);

                        if (client.TryPay())
                        {
                            _money += client.Basket.GetBusketSum();
                            Console.WriteLine($"Баланс магазина {_money} рублей");
                            isActiv = false;
                        }
                    }
                }
            }
        }
    }

    class Client
    {
        public Busket Basket;

        private static Random _random = new Random();
        private int _money;

        public Client()
        {
            int minValueMoney = 100;
            int maxValueMoney = 300;

            _money = _random.Next(minValueMoney, maxValueMoney);
            Basket = new Busket();
        }

        public int Money => _money;

        public Busket GetBusket()
        {
            return Basket;
        }

        public bool TryPay()
        {
            if (Basket.GetBusketCount() == 0)
            {
                Console.WriteLine("У улиента в корзине нет продуктов");
                return false;
            }

            if (Basket.GetBusketSum() <= _money)
            {
                Console.WriteLine($"Довольный клиент совершил покупку на {Basket.GetBusketSum()}! Это успех");
                _money -= Basket.GetBusketSum();
                return true;
            }

            return false;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"У клиента {_money} денег, стоимость корзины {Basket.GetBusketSum()}");

            Basket.ShowBusket();
        }
    }

    class Busket
    {
        private static Random _random = new Random();

        private List<Product> _products = new List<Product>();

        public Busket()
        {
            int maxValueProducts = 15;
            int minValueProducts = 7;

            for (int i = 0; i < _random.Next(minValueProducts, maxValueProducts); i++)
            {
                List<Product> products = new ProductCatalog().GetProducts();

                _products.Add(products[_random.Next(products.Count)]);
            }
        }

        public void RemoveProduct(Product product)
        {
            if (_products.Count != 0)
            {
                _products.Remove(product);
            }
        }

        public void ShowBusket()
        {
            foreach (var product in _products)
            {
                Console.WriteLine($"Продукт {product.Name} стоимость {product.Cost} рублей.");
            }
        }

        public List<Product> GetProducts()
        {
            return new List<Product>(_products);
        }

        public int GetBusketSum()
        {
            int sumBusket = 0;

            foreach (var item in _products)
            {
                sumBusket += item.Cost;
            }

            return sumBusket;
        }

        public int GetBusketCount()
        {
            return _products.Count;
        }
    }

    class Product
    {
        public Product(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public int Cost { get; }
        public string Name { get; }
    }

    class ProductCatalog
    {
        private List<Product> _products = new List<Product>();

        public ProductCatalog()
        {
            _products.Add(new Product("Презервативы", 50));
            _products.Add(new Product("Молоко", 10));
            _products.Add(new Product("Фанарик", 45));
            _products.Add(new Product("Батончик", 12));
            _products.Add(new Product("Хлеб", 5));
            _products.Add(new Product("Сливки", 30));
            _products.Add(new Product("Шоколад", 22));
        }

        public List<Product> GetProducts()
        {
            return new List<Product>(_products);
        }
    }
}
