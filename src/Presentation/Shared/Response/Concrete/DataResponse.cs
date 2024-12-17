using BlazorEcommerce.Shared.Constant;
using BlazorEcommerce.Shared.Response.Abstract;

namespace BlazorEcommerce.Shared.Response.Concrete;

public class DataResponse<T> : IDataResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; } // Allow Data to be nullable

    public int StatusCode { get; }
    public List<string> Messages { get; private set; } = [];

    public DataResponse(T? data, int statuscode, bool success = true)
    {
        Data = data;
        StatusCode = statuscode;
        Success = success;
        if (success)
        {
            Messages.Add(Constant.Messages.DataFound);
        }
        else
        {
            Messages.Add(Constant.Messages.NoDataFound);
        }
    }

    public DataResponse(T? data, int statuscode, string? message, bool success = true)
    {
        Data = data;
        StatusCode = statuscode;
        Success = success;
        if (message != null)
        {
            Messages.Add(message);
        }
    }

    public DataResponse(T? data, int statuscode, List<string> messages, bool success = true)
    {
        Data = data;
        StatusCode = statuscode;
        Success = success;
        Messages = messages;
    }

    public DataResponse(T? data)
    {
        Data = data;
        StatusCode = HttpStatusCodes.Accepted;
        Success = true;
    }

    T? IDataResponse<T>.Data => Data; // Explicit interface implementation to match nullability
}
