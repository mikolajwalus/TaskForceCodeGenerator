using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapperCodeGenerator.RepositoryUtils
{
    public static class RepositoryUtilities
    {
        public static string CreateStringWithRepositoryCode(string entityName, string modelName)
        {
            return String.Format(repositoryTemplate, GetModelNameCoreOrDefault(modelName), entityName);
        }

        public static string CreateStringWithRepositoryInterfaceCode(string entityName, string modelName)
        {
            return String.Format(repositoryInterfaceTemplate, GetModelNameCoreOrDefault(modelName), entityName);
        }

        public static string GetModelNameCoreOrDefault(string modelName)
        {
            if (modelName.Contains("Model"))
                return modelName.Replace("Model", "");

            return modelName;
        }

        private const string repositoryTemplate = @"
using Ap.Plugins.InstiPro.Data.Entities;
using Ap.Plugins.InstiPro.Data.Models;
using Ap.Plugins.InstiPro.Data.Repositories.Base;
using Ap.Plugins.InstiPro.Data.Repositories.Interfaces;
using AutoMapper;

namespace Ap.Plugins.InstiPro.Data.Repositories
{{
    public class {0}Repository : EfRepository<{1}, {0}Model>, I{0}Repository
    {{
        public {0}Repository(InstiProDbContext db, IMapper mapper) : base(db, mapper)
        {{
        }}
    }}
}}";
        private const string repositoryInterfaceTemplate = @"
using Ap.Plugins.InstiPro.Data.Entities;
using Ap.Plugins.InstiPro.Data.Models;
using Ap.Plugins.InstiPro.Data.Repositories.Base;

namespace Ap.Plugins.InstiPro.Data.Repositories.Interfaces
{{
    public interface I{0}Repository : IRepository<{1}, {0}Model>
    {{
    }}
}}";
    }
}
