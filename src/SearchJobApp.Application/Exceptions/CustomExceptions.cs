namespace SearchJobApp.Application.Exceptions;

public class RemainingPostingQuantityException : Exception
{
    public RemainingPostingQuantityException() : base("Yetersiz ilan yayınlama hakkkı.")
    {
    }
}

public class NumberUsedBeforeException : Exception
{
    public NumberUsedBeforeException() : base("Telefon numarası daha önce kullanılmış.")
    {
    }
}

public class EmailAddressUsedBeforeException : Exception
{
    public EmailAddressUsedBeforeException() : base("Email adresi daha önce kullanılmış.")
    {
    }
}

public class EmployerNotFoundException : Exception
{
    public EmployerNotFoundException() : base("Giriş yapmak istediğiniz kullanıcı bulunamadı.")
    {
    }
}