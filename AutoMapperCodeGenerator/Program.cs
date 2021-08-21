using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapperCodeGenerator.FileUtils;
using AutoMapperCodeGenerator.AutoMapperUtils;
using System.IO;
using AutoMapperCodeGenerator.ModelUtils;
using AutoMapperCodeGenerator.RepositoryUtils;

namespace AutoMapperCodeGenerator
{
    class Program
    {
        private const string defaultEntityPath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\Data\Entities";
        private const string defaultModelPath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\Data\Models";
        private const string defaultRepositoryPath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\Data\Repositories";
        private const string defaultRepositoryInterfacePath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\Data\Repositories\Interfaces";
        private const string defaultProfilePath = @"C:\Projects\ArribatecInnovation\A1AR.Prod.ArribaPro\src\Plugins\Ap.Plugins.InstiPro\AutoMapper\TypeProfiles";
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
            Console.WriteLine("Enter model name (must contain 'Model'):");
            Console.WriteLine("############################");
            var modelName = Console.ReadLine();

            var modelProps = GetModelProps(modelName, entityName, entityProps);

            Console.WriteLine();
            Console.WriteLine();

            CreateProfileFile(entityName, modelName, entityProps, modelProps);
            CreateRepositoryFile(entityName, modelName);
            CreateRepositoryInterfaceFile(entityName, modelName);
        }

        public static List<string>  GetModelProps(string modelName, string entityName, List<string> entityProps)
        {
            if (!File.Exists(Path.Combine(defaultModelPath, modelName + ".cs")))
            {
                CreateModelFile(modelName, entityName, entityProps);

                return entityProps;
            }
            else
            {
                return FileUtilities.GetAllClassPropertyNames(modelName, defaultModelPath);
            }
        }

        public static void CreateModelFile(string modelName, string entityName, List<string> entityProps)
        {
            var propertiesCode = FileUtilities.GetAllProperties(entityName, defaultEntityPath);

            var modelPairs = CreatePropPairs(entityProps, propertiesCode);

            var modelCode = ModelUtilities.CreateStringWithModelCode(modelName, entityName, modelPairs);

            FileUtilities.SaveToFile(modelName, modelCode, defaultModelPath);
        }

        public static void CreateProfileFile(string entityName, string modelName, List<string> entityProps, List<string> modelProps)
        {
            if (entityProps.Count != modelProps.Count)
                throw new Exception("Number of props in model and entity has to be the same");

            var pairs = CreatePropPairs(entityProps, modelProps);

            foreach (var pair in pairs)
            {
                Console.WriteLine(pair.EntityProp + "------> " + pair.ModelProp);
            }

            var code = AutoMapperUtilities.CreateStringWithMapCode(entityName, modelName, pairs);

            var fileName = modelName.Replace("Model", "TypeProfile");

            FileUtilities.SaveToFile(fileName, code, defaultProfilePath);
        }

        public static void CreateRepositoryFile(string entityName, string modelName)
        {
            var code = RepositoryUtilities.CreateStringWithRepositoryCode(entityName, modelName);

            var fileName = modelName.Replace("Model", "Repository");

            FileUtilities.SaveToFile(fileName, code, defaultRepositoryPath);
        }

        public static void CreateRepositoryInterfaceFile(string entityName, string modelName)
        {
            var code = RepositoryUtilities.CreateStringWithRepositoryInterfaceCode(entityName, modelName);

            var fileName = "I" + modelName.Replace("Model", "Repository");

            FileUtilities.SaveToFile(fileName, code, defaultRepositoryInterfacePath);
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
