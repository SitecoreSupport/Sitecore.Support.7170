namespace Sitecore.Support.XA.Foundation.Search.Providers.Azure
{
  using System;
  public class CloudSearchTypeMapper : Sitecore.XA.Foundation.Search.Providers.Azure.CloudSearchTypeMapper
  {
    public CloudSearchTypeMapper() : base() { }

    public new Type GetNativeType(string edmTypeName)
    {
      Type type = base.GetNativeType(edmTypeName);

      if (type == typeof(Sitecore.XA.Foundation.Search.Providers.Azure.Geospatial.GeoPoint))
      {
        return typeof(Sitecore.Support.XA.Foundation.Search.Providers.Azure.Geospatial.GeoPoint);
      }

      return type;
    }
  }
}