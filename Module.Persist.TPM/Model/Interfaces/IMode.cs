namespace Module.Persist.TPM.Model.Interfaces
{
    public interface IMode
    {
        TPMmode TPMmode { get; set; }
    }
    public enum TPMmode
    {
        Current,
        RS,
        RA,
        Hidden,
        Archive
    }
}
