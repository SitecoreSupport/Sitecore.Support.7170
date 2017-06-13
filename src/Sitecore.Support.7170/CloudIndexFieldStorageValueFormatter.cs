namespace Sitecore.Support.ContentSearch.Azure.Converters
{
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.Azure.Models;
  using Sitecore.ContentSearch.Converters;
  using Sitecore.Diagnostics;
  using System;
  using System.Collections.Generic;
  using System.Reflection;

  public class CloudIndexFieldStorageValueFormatter : Sitecore.ContentSearch.Azure.Converters.CloudIndexFieldStorageValueFormatter
  {

    private Sitecore.XA.Foundation.Search.Providers.Azure.CloudSearchProviderIndex index;

    public CloudIndexFieldStorageValueFormatter() : base() { }


    public override object FormatValueForIndexStorage(object value, string fieldName)
    {
      Assert.IsNotNullOrEmpty(fieldName, "fieldName");
      object obj = value;
      if (obj == null)
      {
        return null;
      }

      Type baseType = this.GetType().BaseType;
      var fieldInfo = baseType.GetField("searchIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      var indexObj = fieldInfo.GetValue(this);
      index = indexObj as Sitecore.XA.Foundation.Search.Providers.Azure.CloudSearchProviderIndex;

      if (index == null)
      {
        return base.FormatValueForIndexStorage(value, fieldName);
      }

      IndexedField fieldByCloudName = this.index.SchemaBuilder.GetSchema().GetFieldByCloudName(fieldName);

      if (fieldByCloudName == null)
      {
        return value;
      }

      Type nativeType = this.index.CloudConfiguration.CloudTypeMapper.GetNativeType(fieldByCloudName.Type);

      if (nativeType == typeof(Sitecore.XA.Foundation.Search.Providers.Azure.Geospatial.GeoPoint))
      {
        nativeType = typeof(Sitecore.Support.XA.Foundation.Search.Providers.Azure.Geospatial.GeoPoint);
      }

      IndexFieldConverterContext context = new IndexFieldConverterContext(fieldName);
      try
      {
        if (obj is IIndexableId)
        {
          obj = this.FormatValueForIndexStorage(((IIndexableId)obj).Value, fieldName);
        }
        else if (obj is IIndexableUniqueId)
        {
          obj = this.FormatValueForIndexStorage(((IIndexableUniqueId)obj).Value, fieldName);
        }
        else
        {
          MethodInfo dynMethod = this.GetType().BaseType.GetMethod("ConvertToType", BindingFlags.NonPublic | BindingFlags.Instance);
          obj = dynMethod.Invoke(this, new object[] { obj, nativeType, context });
        }
        if (obj != null && !(obj is string) && !nativeType.IsInstanceOfType(obj) && (!(obj is IEnumerable<string>) || !typeof(IEnumerable<string>).IsAssignableFrom(nativeType)))
        {
          throw new InvalidCastException(string.Format("Converted value has type '{0}', but '{1}' is expected.", obj.GetType(), nativeType));
        }
      }
      catch (Exception innerException)
      {
        throw new NotSupportedException(string.Format("Field '{0}' with value '{1}' of type '{2}' cannot be converted to type '{3}' declared for the field in the schema.", new object[]
        {
      fieldName,
      value,
      value.GetType(),
      nativeType
        }), innerException);
      }
      return obj;
    }
  }
}