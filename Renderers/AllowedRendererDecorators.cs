using System;

namespace PetProj.Renderers
{
    /// <summary>
    /// Допустимые операции над геометрией
    /// </summary>
    [Flags]
    public enum AllowedRendererDecorators : uint
    {
        None = 0x0,         // ничего нельзя
        Shadow = 0x1,       // может задавать тень
        Glow = 0x2,         // может задавать "свечение"
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }
}
