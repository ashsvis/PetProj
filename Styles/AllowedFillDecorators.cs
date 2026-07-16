using System;

namespace PetProj.Styles
{
    /// <summary>
    /// Допустимые операции над геометрией
    /// </summary>
    [Flags]
    public enum AllowedFillDecorators : uint
    {
        None = 0x0,             // ничего нельзя
        LinearGradient = 0x1,   // может задавать линейный градиент
        RadialGradient = 0x2,   // может задавать радиальный градиент
        Hatch = 0x4,            // может задавать штриховку
        Texture = 0x8,          // может задавать текстуру
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }
}
