// See https://aka.ms/new-console-template for more information
using Kogel.Elasticsearch;
using Kogel.Elasticsearch.Test.Models;

Console.WriteLine("Hello, World!");


var client = new EsClient("http://192.168.3.7:9200/");

var list = client.QuerySet<EsProduct>()
    .Where(x => x.materialCode == "M00031734" && x.accuracyInt >= 1)
    .ToList();


