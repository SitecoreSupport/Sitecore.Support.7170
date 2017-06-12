namespace Sitecore.Support.XA.Foundation.Search.Providers.Azure.Geospatial
{
  public class GeoPoint
  {
    public double[] coordinates;

    public string type = "Point";

    public double Latitude
    {
      get
      {
        return this.coordinates[1]; // Sitecore.Support.7170
      }
    }

    public double Longitude
    {
      get
      {
        return this.coordinates[0]; // Sitecore.Support.7170
      }
    }

    public GeoPoint(double latitude, double longitude)
    {
      this.coordinates = new double[]
      {
        longitude, // Sitecore.Support.7170
        latitude   // Sitecore.Support.7170
      };
    }
  }
}