using ScreenTestTask.Data;
using ScreenTestTask.Data.Entities;
using ScreenTestTask.Models;
using ScreenTestTask.Services;

namespace ScreenTestTask;

class Program
{
    private const string ConnectionString = "Data Source=.;Initial Catalog=ScreenTestTask;Integrated Security=True;TrustServerCertificate=True;";
    
    private static readonly XmlFileDeserializerService DeserializerService = new();
    private static readonly DatabaseContext DatabaseContext = new(ConnectionString);

    private const string InsertGoodsQuery = "INSERT INTO [dbo].[Goods] ([dbo].[Goods].[Name], [dbo].[Goods].[Price]) VALUES (@Name, @Price); SELECT SCOPE_IDENTITY();";
    private const string InsertUserQuery = "INSERT INTO [dbo].[Users] ([dbo].[Users].[Name]) VALUES (@Name); SELECT SCOPE_IDENTITY();";
    private const string InsertUserGoodsQuery = "INSERT INTO [dbo].[UserGoods] ([dbo].[UserGoods].[UserId], [dbo].[UserGoods].[GoodsId], [dbo].[UserGoods].[BoughtAt]) VALUES (@UserId, @GoodsId, @BoughtAt); SELECT SCOPE_IDENTITY()";

    private static readonly Dictionary<string, int> GoodsNameToGoodsEntity = new();
    private static readonly Dictionary<string, int> UsersNameToUserEntity = new();
    private static readonly Dictionary<string, (int, DateTime)> UserGoodsMap = new();

    static async Task Main(string[] args)
    {
        string fileName;
        
        while (true)
        {
            Console.WriteLine($"Введите название xml файла в папке {AppDomain.CurrentDomain.BaseDirectory}:");

            fileName = Console.ReadLine();
            
            if(!string.IsNullOrEmpty(fileName)) break;
        }
        
        var orders = DeserializerService.Deserialize<XmlOrders>(AppDomain.CurrentDomain.BaseDirectory + fileName);

        MapUserGoodsRelations(orders);

        await Task.WhenAll(InsertGoods(orders), InsertUsers(orders));

        await InsertUserGoods();
        
        Console.WriteLine("Запись завершена.");
    }

    private static async Task InsertUserGoods()
    {
        var userGoodsList = new List<UserGoods>();
        
        foreach (var kv in UserGoodsMap)
        {
            var (quantity, boughtAt) = kv.Value;
            
            for (var i = 0; i < quantity; i++)
            {
                var partialKey = kv.Key.Split("_");
                if (UsersNameToUserEntity.TryGetValue(partialKey[0], out var userId) && GoodsNameToGoodsEntity.TryGetValue(partialKey[1], out var goodsId))
                {
                    userGoodsList.Add(new UserGoods { BoughtAt = boughtAt.Date, GoodsId = goodsId, UserId = userId });
                }
            }
        }

        foreach (var userGoods in userGoodsList) _ = await DatabaseContext.InsertData(InsertUserGoodsQuery, userGoods);
    }

    private static void MapUserGoodsRelations(XmlOrders xmlOrders)
    {
        foreach (var xmlOrder in xmlOrders.OrdersList)
        {
            foreach (var product in xmlOrder.Products)
            {
                UserGoodsMap.TryAdd($"{xmlOrder.User.Email}_{product.Name}", (product.Quantity, DateTime.Parse(xmlOrder.RegDateString)));
            }
        }
    }

    private static async Task InsertGoods(XmlOrders orders)
    {
        var goods = new List<Goods>();

        foreach (var order in orders.OrdersList)
        {
            goods.AddRange(order.Products.Select(product => new Goods { Name = product.Name, Price = product.Price }));
        }
        
        foreach (var good in goods.DistinctBy(g => g.Name).ToList())
        {
            var id = await DatabaseContext.InsertData(InsertGoodsQuery, good);
            good.Id = id;
            GoodsNameToGoodsEntity.TryAdd(good.Name, good.Id);
        }
    }
    
    private static async Task InsertUsers(XmlOrders orders)
    {
        var users = orders.OrdersList.Select(order => new User { Name = order.User.Email }).DistinctBy(u => u.Name).ToList();
        
        foreach (var user in users)
        {
            var id = await DatabaseContext.InsertData(InsertUserQuery, user);
            user.Id = id;
            UsersNameToUserEntity.TryAdd(user.Name, user.Id);
        }
    }
}