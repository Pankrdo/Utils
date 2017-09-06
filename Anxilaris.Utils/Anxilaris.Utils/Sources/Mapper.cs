//-----------------------------------------------------------------------
// <copyright file="Mapper.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Adrian Castan Melchor</author>
//-----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The mapper class tool
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// Map an object to another with the same structure
        /// </summary>
        /// <typeparam name="T"> the source type</typeparam>
        /// <typeparam name="U"> the destination type</typeparam>
        /// <param name="toMap">an object of source type</param>
        /// <returns>an object of destination type</returns>
        public static U Map<T, U>(T toMap)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, U>());
            var mapper = config.CreateMapper();
            return mapper.Map<T, U>(toMap);
        }

        /// <summary>
        /// Map an object to another with the same structure
        /// </summary>
        /// <typeparam name="T"> the source type</typeparam>
        /// <param name="toMap">an object of source type</param>
        /// <returns>an object of destination type</returns>
        public static T Map<T>(object toMap)
        {
            return AutoMapper.Mapper.Map<T>(toMap);
        }

        /// <summary>
        /// Map a list to another with the same structure
        /// </summary>
        /// <typeparam name="T"> the source type</typeparam>
        /// <typeparam name="U"> the destination type</typeparam>
        /// <param name="listToMap">a generic source type list</param>
        /// <returns>A generic list of destination type</returns>
        public static List<U> MapCollection<T, U>(IEnumerable<T> listToMap)
        {
            List<U> mappedList = new List<U>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, U>());
            var mapper = config.CreateMapper();

            for (int i = 0; i < listToMap.Count(); i++)
            {
                mappedList.Add(mapper.Map<T, U>(listToMap.ElementAt(i)));
            }

            return mappedList;
        }

        // <summary>
        /// Map a list to another with the same structure
        /// </summary>
        /// <typeparam name="T"> the source type</typeparam>
        /// <param name="listToMap">a generic source type list</param>
        /// <returns>A generic list of destination type</returns>
        public static List<T> MapCollection<T>(IEnumerable<object> listToMap)
        {
            List<T> mappedList = new List<T>();

            for (int i = 0; i < listToMap.Count(); i++)
            {
                mappedList.Add(AutoMapper.Mapper.Map<T>(listToMap.ElementAt(i)));
            }

            return mappedList;
        }

        /// <summary>
        /// Get a maper configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public static IMapper GetMapper<T, U>()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, U>());
            var mapper = config.CreateMapper();

            return mapper;
        }

        /// <summary>
        /// Init a configuration mapper
        /// </summary>
        /// <param name="mappers"></param>
        public static void SetAutoMapper(Action<IMapperConfiguration> mappers)
        {
            AutoMapper.Mapper.Initialize(mappers);
        }
    }
}