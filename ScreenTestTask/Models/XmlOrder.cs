using System.Xml.Serialization;

namespace ScreenTestTask.Models;

public class XmlOrder
{
    [XmlElement("no")]
    public int OrderNumber { get; set; }
    
    [XmlElement("reg_date")]
    public string RegDateString { get; set; }

    [XmlElement("sum")]
    public decimal Sum { get; set; }

    [XmlElement("product")]
    public List<XmlProduct> Products { get; set; }

    [XmlElement("user")]
    public XmlUser User { get; set; }
}