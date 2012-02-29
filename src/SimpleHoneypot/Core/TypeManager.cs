using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleHoneypot.Core {
    public static class TypeManager {
        public static IEnumerable<Type> TypesImplementingInterface(Type desiredType) {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => desiredType.IsAssignableFrom(type));
        }

        //public static IEnumerable<T> CustomRules<T>() {
        //    var items = TypesImplementingInterface(typeof (IHoneypotCustomRule<>));
        //}

    }
}