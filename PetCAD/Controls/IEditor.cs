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
        public Figure[] Figures { get; set; }

        public ChangeEventArgs(Figure[] figures)
        {
            this.Figures = figures;
        }
    }

    public class ChangingEventArgs
    {
        public string ChangingName { get; set; }

        public ChangingEventArgs(string changingName)
        {
            this.ChangingName = changingName;
        }
    }
}
