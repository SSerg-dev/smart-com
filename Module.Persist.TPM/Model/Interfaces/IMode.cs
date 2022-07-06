namespace Module.Persist.TPM.Model.Interfaces
{
    public interface IMode
    {
        TPMmode TPMmode { get; set; }
    }
    public enum TPMmode
    {
        StandartMode,
        RSMode
    }
}
