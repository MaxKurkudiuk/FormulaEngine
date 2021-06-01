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

    public class SymbolTable {
        readonly Dictionary<string, SymbolTableEntry> Entries = new Dictionary<string, SymbolTableEntry>();

        public void AddOrUpdate(object variables) {
            var kvp = variables.GetType()
                .GetProperties()
                .Select<PropertyInfo, (string, double)>(x => (x.Name, Convert.ToDouble(x.GetValue(variables))));

            foreach(var (id, val) in kvp)
                AddOrUpdate(id, val);
        }

        public void AddOrUpdate(string identifier, double value) {
            if (!Entries.ContainsKey(identifier)) {
                // create one
                Entries.Add(identifier, new VariableSymbolTableEntry(identifier, value));
            } else {
                var entry = Entries[identifier];
                if (entry.Type != SymbolTableEntry.EntryType.Variable)
                    throw new System.Exception($"Identifier {identifier} type mismatch");
                (entry as VariableSymbolTableEntry).Value = value;
            }
        }

        public SymbolTableEntry Get(string identifier)
            => Entries.ContainsKey(identifier) 
            ? Entries[identifier] : null;
    }
}
