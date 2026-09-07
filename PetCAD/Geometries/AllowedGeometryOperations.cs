using System;

namespace PetCAD.Geometries
{
    /// <summary>
    /// Допустимые операции над геометрией
    /// </summary>
    [Flags]
    public enum AllowedGeometryOperations : uint
    {
        None = 0x0,         // ничего нельзя
        Vertex = 0x1,       // может изменять внутренние вершины
        Explode = 0x2,       // может разбивать геометрию на примитивы
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }
}
