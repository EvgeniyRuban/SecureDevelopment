using System.Security.Cryptography.X509Certificates;

namespace AppCertification;

public class X509Certificate2Wrapper
{
    private X509Certificate2 _cert;
    private string _group;
    private string _certGroupName;

    public X509Certificate2Wrapper(
        X509Certificate2 cert, 
        string certGroupName, 
        string group)
    {
        _cert = cert;
        _certGroupName = certGroupName;
        _group = group;
    }

    public X509Certificate2 Certificate
    {
        get { return _cert; }
    }
    public string PublishedFor
    {
        get { return _cert.GetNameInfo(X509NameType.SimpleName, false); }
    }
    public string Published
    {
        get { return _cert.GetNameInfo(X509NameType.SimpleName, true); }
    }
    public string ExpirationDate
    {
        get { return _cert.GetExpirationDateString(); }
    }
    public string Group
    {
        get { return _group; }
    }
    public string CertGroupName
    {
        get { return _certGroupName; }
    }

    public override string ToString()
    {
        return $"Group: {Group} ({CertGroupName})\nPublishedFor: {PublishedFor}\nPublished: {Published}\nExp: {ExpirationDate}\n";
    }
}