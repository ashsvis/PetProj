using System;

namespace PetProj.ObjectBindings
{
    /// <summary>
    /// Допустимые операции объектных привязок
    /// </summary>
    [Flags]
    public enum AllowedObjectBindings : uint
    {
        None = 0x0,         // ничего нельзя
        EndPoint = 0x1,     // может привязываться к конечным точкам
        Middle = 0x2,       // может привязываться к середине
        Center = 0x4,       // может привязваться к центру
        // новые режимы добавлять здесь
        All = 0xffffffff,   // всё можно
    }
}
