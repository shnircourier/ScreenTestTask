using System.Xml.Serialization;

namespace ScreenTestTask.Models;

public class XmlUser
{
    [XmlElement("fio")]
    public string Fio { get; set; }

    [XmlElement("email")]
    public string Email { get; set; }
}