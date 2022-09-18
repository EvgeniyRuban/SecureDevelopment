namespace AppCertification;

public sealed class CertificateGenerationException : Exception
{
    public CertificateGenerationException( string message) : base(message)
    {
    }
}