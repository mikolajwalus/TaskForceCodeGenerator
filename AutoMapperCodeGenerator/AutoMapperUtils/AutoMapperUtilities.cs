using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMapperCodeGenerator.AutoMapperUtils
{
    public static class AutoMapperUtilities
    {
        public static string CreateStringWithMapCode(string entityName, string modelName, IEnumerable<(string EntityProp, string ModelProp)> entityModelPropsPairs)
        {
            var beggining = String.Format(begginingTemplate, modelName);

            var entityToModelMap = CreateSingleMapString(entityName, modelName, entityModelPropsPairs);

            var reversedPropPairs = entityModelPropsPairs.Select(x => (x.ModelProp, x.EntityProp));

            var modelToEntityMap = CreateSingleMapString(modelName, entityName, reversedPropPairs);

            return beggining + "\n" + entityToModelMap + "\n\n" + modelToEntityMap + endTemplate;
        }

        private static string CreateSingleMapString(string from, string to, IEnumerable<(string FromProp, string ToProp)> fromToPropsPairs)
        {
            var result = String.Format(createMapTemplate, from, to);

            foreach (var pair in fromToPropsPairs)
            {
                result += String.Format(memberMapTemplate, pair.FromProp, pair.ToProp);
            }

            result += ";";

            return result;
        }

        private const string begginingTemplate = @"
using Ap.Plugins.InstiPro.Data.Entities;
using Ap.Plugins.InstiPro.Data.Models;
using AutoMapper;

namespace Ap.Plugins.InstiPro.AutoMapper.TypeProfiles
{{
    public class {0}Profile : Profile
    {{
            public {0}Profile()
            {{";

        private const string createMapTemplate = @"
                CreateMap<{0}, {1}>()";

        private const string memberMapTemplate = @"
                    .ForMember(
                        d => d.{1},
                        op => op.MapFrom(s => s.{0})
                    )";

        private const string endTemplate = @"
            }
    }
}";
    }
}
