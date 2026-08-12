using PetCAD.Figures;
using System;

namespace PetCAD.Controls
{
    /// <summary>
    /// Interface of editor of object T
    /// </summary>
    public interface IEditor<T>
    {
        /// <summary>
        /// Start changing
        /// </summary>
        event EventHandler<ChangingEventArgs> StartChanging;

        /// <summary>
        /// Object was changed
        /// </summary>
        event EventHandler<ChangeEventArgs> Changed;

        /// <summary>
        /// Build editor interface for the object
        /// </summary>
        void Build(T obj);
    }

    public class ChangeEventArgs
    {
        public object[] Arguments { get; set; }
        public string Name { get; }

        public ChangeEventArgs(string name, params object[] args)
        {
            Name = name;
            Arguments = args;
        }
    }

    public class ChangingEventArgs
    {
        public object[] Arguments { get; set; }
        public string Name { get; }

        public ChangingEventArgs(string name, params object[] args)
        {
            Name = name;
            Arguments = args;
        }
    }
}
