using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoMapperCodeGenerator.ModelUtils
{
    public static class ModelUtilities
    {
        public static string CreateStringWithModelCode(string modelName, string entityName, IEnumerable<(string PropertyName, string PropertyCode)> properties)
        {
            var beggining = String.Format(begginingTemplate, entityName, modelName);

            var body = "";
            foreach (var property in properties)
            {
                body += String.Format(propertyTemplate, property.PropertyName) + property.PropertyCode;
            }

            return beggining + body + end;
        }

        private const string begginingTemplate = @"
using Ap.Plugins.InstiPro.Data.Entities;
using System;
using System.Collections.Generic;

namespace Ap.Plugins.InstiPro.Data.Models
{{
    /// <summary
    /// Model of {0}
    /// </summary>
    public class {1}
    {{";

        private const string propertyTemplate = @"
        /// <summary>
        /// Mapped from {0}
        /// </summary>
        ";

        private const string end = @"
    }
}";
    }
}
