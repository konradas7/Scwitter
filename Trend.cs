using RestSharp;
using System;

namespace Scwitter
{
    public class Trend
    {
        public string Name { get; private set; }
        public string MetaDescription { get; private set; }
        public string DomainContext { get; private set; }
        internal Trend(JsonObject item)
        {
            if (!(item).TryGetValue("content", out object itemContent))
            {
                throw new Exception("Incorrect trend response body");
            }
            if (!((JsonObject)itemContent).TryGetValue("trend", out object trend))
            {
                throw new Exception("Incorrect trend response body");
            }
            if (((JsonObject)trend).TryGetValue("name", out object name))
            {
                Name = (string)name;
            }
            if (((JsonObject)trend).TryGetValue("trendMetadata", out object trendMetadata))
            {
                if (((JsonObject)trendMetadata).TryGetValue("metaDescription", out object metadescribtion))
                {
                    MetaDescription = (string)metadescribtion;
                }
                if (((JsonObject)trendMetadata).TryGetValue("domainContext", out object domainContext))
                {
                    DomainContext = (string)domainContext;
                }
            }
        }
    }
}