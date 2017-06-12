namespace Sitecore.Support.XA.Foundation.Search.Providers.Azure.ComputedFields
{
  using Sitecore.ContentSearch;
  //using Sitecore.ContentSearch.ComputedFields; // Sitecore.Support.7170
  using Sitecore.Data.Items;
  using Sitecore.XA.Foundation.Geospatial;
  //using Sitecore.XA.Foundation.Search.Providers.Azure.Geospatial; // Sitecore.Support.7170
  using Sitecore.Support.XA.Foundation.Search.Providers.Azure.Geospatial; // Sitecore.Support.7170
  using System;
  using System.Globalization;

  public class AzureCoordinate : Sitecore.XA.Foundation.Search.Providers.Azure.ComputedFields.AzureCoordinate
  {
    public override object ComputeFieldValue(IIndexable indexable)
    {
      Item item = indexable as SitecoreIndexableItem;
      if (item == null || !item.Fields.Contains(Templates.IPoi.Fields.Latitude) || !item.Fields.Contains(Templates.IPoi.Fields.Longitude))
      {
        return null;
      }
      double latitude;
      double longitude;
      if (!double.TryParse(item[Templates.IPoi.Fields.Latitude], NumberStyles.Any, CultureInfo.InvariantCulture, out latitude) || !double.TryParse(item[Templates.IPoi.Fields.Longitude], NumberStyles.Any, CultureInfo.InvariantCulture, out longitude))
      {
        return null;
      }
      return new GeoPoint(latitude, longitude);
    }
  }
}