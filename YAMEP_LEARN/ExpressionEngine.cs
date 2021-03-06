using System;
using System.Collections.Generic;
using System.Linq;

namespace YAMEP_LEARN {
    public class ExpressionEngine {
        private SymbolTable _symbolTable;
        public ExpressionEngine() => _symbolTable = new SymbolTable();

        public void AddFunctions<T>() => _symbolTable.AddFunctions<T>();

        /// <summary>
        /// Evaluates an expression and returns the final result
        /// </summary>
        /// <param name="expression">the expression to evaluate</param>
        /// <param name="variables">the expression variables</param>
        /// <returns>the result</returns>
        public double Evaluate(string expression, object variables) {
            if (variables != null) {
                _symbolTable.AddOrUpdate(variables);
            }
            return Evaluate(expression);
        }

        /// <summary>
        /// Evaluates an expression and returns the final result
        /// </summary>
        /// <param name="expression">the expression to evaluate</param>
        /// <returns>the result</returns>
        public double Evaluate(string expression) {
            var astRoot = new Parser(new Lexer(new SourceScanner(expression)), _symbolTable).Parse();
            return Evaluate(astRoot);
        }

        public double Evaluate(ASTNode root) => Evaluate(root as dynamic);

        protected double Evaluate(AdditionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);
        protected double Evaluate(SubtractionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        protected double Evaluate(MultiplicationBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        protected double Evaluate(DivisionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);
        protected double Evaluate(ExponentBinaryOperatorASTNode node)
            => Math.Pow(Evaluate(node.Left as dynamic), Evaluate(node.Right as dynamic));
        protected double Evaluate(NumberASTNode node) => node.Value;
        protected double Evaluate(NegationUnaryOperatorASTNode node) => -1 * Evaluate(node.Target as dynamic);
        protected double Evaluate(FactorialUnaryOperatorASTNode node) {
            int fact(int x) => x == 1 ? 1 : x * fact(x - 1);
            var value = (int)Evaluate(node.Target as dynamic);
            if (value < 0)
                throw new Exception("Factorial only supports Positive Integers");
            value = value == 0 ? 1 : value;
            //return value < 0 ? -fact(-value) : fact(value);
            return fact(value);
        }


        protected double Evaluate(VariableIdentifierASTNode node) {
            var entry = _symbolTable.Get(node.Name);
            if (entry == null || entry.First().Type != SymbolTableEntry.EntryType.Variable) {
                throw new Exception($"Error Evaluating Exception. Variable {node.Name}");
            }
            return (entry.First() as VariableSymbolTableEntry).Value;
        }

        protected double Evaluate(FunctionASTNode node) {
            var entrys = _symbolTable.Get(node.Name);
            if (entrys == null || entrys.Where(e => e.Type == SymbolTableEntry.EntryType.Fucntion).Count() != entrys.Count)
                throw new Exception($"Error Evaluating Exception. Function {node.Name}");

            var funcEntrys = entrys.Select(x => x as FunctionSymbolTableEntry).ToList(); //(entry as List<FunctionSymbolTableEntry>);
            var funcEntry = funcEntrys.Where(e => e.MethodInfo.GetParameters().Length == node.ArgumentsNodes.Count).FirstOrDefault();
            var requiredParams = funcEntrys.Select(e => e.MethodInfo.GetParameters().Length);
            if (funcEntry == null)
                throw new Exception($"Wrong count of parameters in Function {funcEntrys.First().IdentiferName}. " +
                    $"Must be {string.Join(" or ", requiredParams)} arguments but not {node.ArgumentsNodes.Count}");

            object[] parameters = node.ArgumentsNodes.Select(arg => Evaluate(arg as dynamic)).ToArray();
            return (double)funcEntry.MethodInfo.Invoke(null, parameters);
        }
    }
}
