namespace Ecc.Logic.Services
{
    public interface IEccTextFunctionInvoker
    {
        string ProccessText(string interactionId, EccReplacing replacing);
    }
}