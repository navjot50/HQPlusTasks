using System;
using System.ComponentModel;
using System.Data;

namespace HQPlus.Data.Extensions {
    public static class DataTableExtensions {

        public static void InsertPropertiesAsDataTableRow<T>(this T item, DataTable dataTable) where T : class {
            var propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            var values = new object[propertyDescriptorCollection.Count];
            
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = propertyDescriptorCollection[i].GetValue(item);
            }
            
            dataTable.Rows.Add(values);
        }
        
    }
}