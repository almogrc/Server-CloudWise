﻿using System;
using System.Collections.Generic;
using BuisnessLogic.Collector.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Server_cloudata.DTO;
using Server_cloudata.Enums;

namespace Server_cloudata.Models
{
    public class VirtualMachine
    {
        public string Name { get; set; }
        public string Supplier { get; set; }
        public string Address { get; set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<eNodeExporterData, Threshold> Thresholds { get; set; } // should fix database and return it and in the alert to threshold class
        public List<Alert> Alerts { get; set; }
    }
}
