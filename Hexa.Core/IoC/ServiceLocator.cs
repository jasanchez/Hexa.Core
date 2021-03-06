﻿#region License

//===================================================================================
//Copyright 2010 HexaSystems Corporation
//===================================================================================
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//http://www.apache.org/licenses/LICENSE-2.0
//===================================================================================
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
//===================================================================================

#endregion

using System;
using System.Linq;

using SL = Microsoft.Practices.ServiceLocation;

namespace Hexa.Core
{
    /// <summary>
    /// This is a helper for accessing dependencies via the Common Service Locator (CSL).  But while
    /// the CSL will throw object reference errors if used before initialization, this will inform
    /// you of what the problem is.  Perhaps it would be more aptly named "InformativeServiceLocator."
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns></returns>
        public static TDependency GetInstance<TDependency>()
        {
            return (TDependency)GetInstance(typeof(TDependency));
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ServiceLocator")]
        public static object GetInstance(Type dependencyType)
        {
            object service;

            try
            {
                service = SL.ServiceLocator.Current.GetService(dependencyType);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("ServiceLocator has not been initialized; " +
                    "I was trying to retrieve " + dependencyType.ToString());
            }
            catch (SL.ActivationException)
            {
                throw new SL.ActivationException("The needed dependency of type " + dependencyType.Name +
                    " could not be located with the ServiceLocator. You'll need to register it with " +
                    "the Common Service Locator (CSL) via your IoC's CSL adapter.");
            }

            return service;
        }

        /// <summary>
        /// Tries to get service.
        /// </summary>
        /// <returns></returns>
        public static TDependency TryGetInstance<TDependency>()
        {
            try
            {
                var services = SL.ServiceLocator.Current.GetAllInstances(typeof(TDependency));

                if (services != null)
                    return (TDependency)services.FirstOrDefault();
            }
            catch (NullReferenceException)
            {
                throw new InternalException("ServiceLocator has not been initialized; " +
                    "I was trying to retrieve " + typeof(TDependency).ToString());
            }

            return default(TDependency);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ServiceLocator")]
        public static TDependency[] GetAllInstances<TDependency>()
        {
            try
            {
                var services = SL.ServiceLocator.Current.GetAllInstances(typeof(TDependency));

                if (services != null)
                    return services.Cast<TDependency>().ToArray();
            }
            catch (NullReferenceException)
            {
                throw new InternalException("ServiceLocator has not been initialized; " +
                    "I was trying to retrieve " + typeof(TDependency).ToString());
            }

            return default(TDependency[]);
        }

    }
}
