namespace Autodissmark.API.Responses.BaseResponses;

public record SuccessResponse : BaseResponse;
public sealed record SuccessResponse<TModel>(TModel Data) : SuccessResponse;
