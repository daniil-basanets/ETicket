﻿using System.Xml.Serialization;

namespace ETicket.PrivatBankApi
{
    public class Merchant
    {
        [XmlElement(ElementName = "id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "signature")]
        public string Signature { get; set; }
    }
}