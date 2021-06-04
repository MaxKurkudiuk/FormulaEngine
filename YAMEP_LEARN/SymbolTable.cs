using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YAMEP_LEARN {
    /// <summary>
    /// var expression = "2 * sin(x) * y";
    /// var variables = new { x = 5, y = 10 };
    ///
    /// var evalEngine = new ExpressionEngine();
    /// var result = evalEngine.Evaluate(expression, variables);
    /// </summary>
    public abstract class SymbolTableEntry {
        public string IdentiferName { get; }
        public EntryType Type { get; }

        protected SymbolTableEntry(string name, EntryType entryType) {
            this.IdentiferName = name;
            this.Type = entryType;
        }

        public enum EntryType {
            Variable,
            Fucntion
        }
    }

    public class VariableSymbolTableEntry : SymbolTableEntry {
        public double Value { get; set; }
        public VariableSymbolTableEntry(string name, double value)
            : base(name, EntryType.Variable) => this.Value = value;
    }

    public class FunctionSymbolTableEntry : SymbolTableEntry {
        public MethodInfo MethodInfo { get; }
        public FunctionSymbolTableEntry(MethodInfo methodInfo)
            : base(methodInfo.Name, EntryType.Fucntion) => MethodInfo = methodInfo;
    }

    public class SymbolTable {
        readonly Dictionary<string, List<SymbolTableEntry>> Entries = new Dictionary<string, List<SymbolTableEntry>>();

        public void AddOrUpdate(object variables) {
            var kvp = variables.GetType()
                .GetProperties()
                .Select<PropertyInfo, (string, double)>(x => (x.Name, Convert.ToDouble(x.GetValue(variables))));

            foreach (var (id, val) in kvp)
                AddOrUpdate(id, val);
        }

        public void AddOrUpdate(string identifier, double value) {
            var key = identifier.ToLower();
            if (!Entries.ContainsKey(key)) {
                // create one
                Entries.Add(key, new List<SymbolTableEntry>() { new VariableSymbolTableEntry(identifier, value) });
            } else {
                var entry = Entries[key];
                if (entry.First().Type != SymbolTableEntry.EntryType.Variable)
                    throw new Exception($"Identifier {identifier} type mismatch");
                (entry.First() as VariableSymbolTableEntry).Value = value;
            }
        }

        public void AddFunctions<T>() {
            // Get all Public Static Functions that return double and take double parameter types
            // 1 - must have double return type
            // 2 - parameters must be of type double
            var methods = typeof(T)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => typeof(double).IsAssignableFrom(mi.ReturnType))
                .Where(mi => !mi.GetParameters().Any(param => !param.ParameterType.IsAssignableFrom(typeof(double))));

            foreach (var mi in methods) {
                if (Entries.ContainsKey(mi.Name.ToLower()))
                    Entries[mi.Name.ToLower()].Add(new FunctionSymbolTableEntry(mi));
                else
                    Entries.Add(mi.Name.ToLower(), new List<SymbolTableEntry>() { new FunctionSymbolTableEntry(mi) });
            }
        }

        public List<SymbolTableEntry> Get(string identifier)
            => Entries.ContainsKey(identifier.ToLower())
            ? Entries[identifier.ToLower()] : null;

        public Dictionary<string, List<SymbolTableEntry>> GetAll() => Entries;
    }
}
