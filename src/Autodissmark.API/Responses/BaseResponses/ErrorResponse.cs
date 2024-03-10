using Autodissmark.API.ExceptionHandling;

namespace Autodissmark.API.Responses.BaseResponses;

public record ErrorResponse(ExceptionCodes Code, string ErrorMessage) : BaseResponse;

public sealed record ErrorResponse<TModel>(ExceptionCodes Code, string ErrorMessage, IEnumerable<TModel> Data)
    : ErrorResponse(Code: Code, ErrorMessage: ErrorMessage) where TModel : class
{ }
