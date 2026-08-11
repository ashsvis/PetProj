using System;

namespace PetCAD.Figures
{
    /// <summary>
    /// Допустимые операции над геометрией
    /// </summary>
    [Flags]
    public enum AllowedMarkerOperations : uint
    {
        None = 0x0,         // ничего нельзя
        MoveOwner = 0x1,    // может перемещать фигуру целиком
        MoveVertex = 0x2,   // может перемещать вершины фигур
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }
}
