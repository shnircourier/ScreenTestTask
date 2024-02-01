namespace ScreenTestTask.Data.Entities;

public class UserGoods
{
    public int Id { get; set; }

    public int GoodsId { get; set; }

    public int UserId { get; set; }

    public DateTime BoughtAt { get; set; }
}