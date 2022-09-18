using System.Security.Cryptography.X509Certificates;

namespace AppCertification;

public sealed class CertificateConfiguration
{
    public X509Certificate2 RootCertificate { get; set; } = null!;
    public TimeSpan CertDuration { get; set; }
    public string CertName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string OutFolder { get; set; } = null!;
    public string Email { get; set; } = null!;
}