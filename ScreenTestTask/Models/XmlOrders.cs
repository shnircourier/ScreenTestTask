using System.Xml.Serialization;

namespace ScreenTestTask.Models;

[XmlRoot("orders")]
public class XmlOrders
{
    [XmlElement("order")]
    public List<XmlOrder> OrdersList { get; set; } = new();
}