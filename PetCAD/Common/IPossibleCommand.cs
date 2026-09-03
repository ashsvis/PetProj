namespace PetCAD.Common
{
    public interface IPossibleCommand : ICommand
    {
        bool CanExecute();
    }
}
