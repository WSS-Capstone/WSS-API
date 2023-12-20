namespace WSS.API.Infrastructure.Config;

public class EmailUtils
{
    /// <summary>
    /// Subject confirm order
    /// </summary>
    public static string MailSubjectConfirm = "Order của bạn đã được xác nhận";

    /// <summary>
    /// Content confirm order
    /// </summary>
    public static string MailContentConfirm = "Order của bạn đã được xác nhận";
    
    /// <summary>
    /// Subject cancel order
    /// </summary>
    public static string MailSubjectCancel = "Order của bạn đã bị huỷ";

    /// <summary>
    /// Content cancel order
    /// </summary>
    public static string MailContentCancel = "Order của bạn đã bị huỷ";
    public static string MailDone = "Order của bạn đã hoàn thành";
}