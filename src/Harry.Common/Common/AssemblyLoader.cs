//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace Harry.Common
//{
//    public class AssemblyLoader
//    {
//        public static T TryLoadAndCreateInstance<T>(string assemblyName, Logger logger) where T : class
//        {
//            try
//            {
//                var assembly = Assembly.Load(new AssemblyName(assemblyName));
//                var foundType =
//                    TypeHelper.GetTypes(
//                        assembly,
//                        type =>
//                        typeof(T).IsAssignableFrom(type) && !type.GetTypeInfo().IsInterface
//                        && type.GetTypeInfo().GetConstructor(Type.EmptyTypes) != null, logger).FirstOrDefault();
//                if (foundType == null)
//                {
//                    return null;
//                }

//                return (T)Activator.CreateInstance(foundType, true);
//            }
//            catch (FileNotFoundException exception)
//            {
//                logger.Warn(ErrorCode.Loader_TryLoadAndCreateInstance_Failure, exception.Message, exception);
//                return null;
//            }
//            catch (Exception exc)
//            {
//                logger.Error(ErrorCode.Loader_TryLoadAndCreateInstance_Failure, exc.Message, exc);
//                throw;
//            }
//        }

//        public static T LoadAndCreateInstance<T>(string assemblyName, Logger logger) where T : class
//        {
//            try
//            {
//                var assembly = Assembly.Load(new AssemblyName(assemblyName));
//                var foundType = TypeHelper.GetTypes(assembly, type => typeof(T).IsAssignableFrom(type), logger).First();

//                return (T)Activator.CreateInstance(foundType, true);
//            }
//            catch (Exception exc)
//            {
//                logger.Error(ErrorCode.Loader_LoadAndCreateInstance_Failure, exc.Message, exc);
//                throw;
//            }
//        }
//    }
//}
