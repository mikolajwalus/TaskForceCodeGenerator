using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapperCodeGenerator.FileUtils;
using AutoMapperCodeGenerator.AutoMapperUtils;
using System.IO;
using AutoMapperCodeGenerator.ModelUtils;

namespace AutoMapperCodeGenerator
{
    class Program
    {
        private const string defaultEntityPath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\Data\Entities";
        private const string defaultModelPath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\Data\Models";
        private const string defaultOutputPath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\AutoMapper\TypeProfiles";
        static void Main(string[] args)
        {
            Console.WriteLine("############################");
            Console.WriteLine("Enter entity name:");
            Console.WriteLine("############################");
            var entityName = Console.ReadLine();
            var entityProps = FileUtilities.GetAllClassPropertyNames(entityName, defaultEntityPath);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("############################");
            Console.WriteLine("Enter model name:");
            Console.WriteLine("############################");
            var modelName = Console.ReadLine();

            var modelProps = new List<string>();

            if(!File.Exists(defaultModelPath + modelName + ".cs"))
            {
                var propertiesCode = FileUtilities.GetAllProperties(entityName, defaultEntityPath);
                modelProps = entityProps;
                var modelPairs = CreatePropPairs(modelProps, propertiesCode);

                var modelCode = ModelUtilities.CreateStringWithModelCode(modelName, entityName, modelPairs);

                FileUtilities.SaveToFile(modelName, modelCode, defaultModelPath);
            }
            else
            {
                modelProps = FileUtilities.GetAllClassPropertyNames(modelName, defaultModelPath);
            }

            Console.WriteLine();
            Console.WriteLine();

            if (entityProps.Count != modelProps.Count)
                throw new Exception("Number of props in model and entity has to be the same");

            var pairs = CreatePropPairs(entityProps, modelProps);

            foreach (var pair in pairs)
            {
                Console.WriteLine(pair.EntityProp + "------> " + pair.ModelProp);
            }

            var code = AutoMapperUtilities.CreateStringWithMapCode(entityName, modelName, pairs);

            var fileName = modelName.Replace("Model", "")+ "TypeProfile";

            FileUtilities.SaveToFile(fileName, code, defaultOutputPath);
        }

        public static List<(string EntityProp, string ModelProp)> CreatePropPairs(IEnumerable<string> entityProps, IEnumerable<string> modelProps)
        {
            var result = new List<(string EntityProp, string ModelProp)>();

            var entityPropsArray = entityProps.ToArray();
            var modelPropsArray = modelProps.ToArray();

            for (int i = 0; i < entityPropsArray.Length; i++)
            {
                result.Add((EntityProp: entityPropsArray[i], ModelProp: modelPropsArray[i]));
            }

            return result;
        }
    }

}
