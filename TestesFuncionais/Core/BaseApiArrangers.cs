using RestSharp;

namespace TestesFuncionais.Core;

public static class BaseApiArrangers
{
    public static RestRequest CreateRequest(string resource, Method method)
    {
        var request = new RestRequest(resource, method);
        request.AddHeader("Accept", "application/json");
        
        if (method == Method.Post || method == Method.Patch || method == Method.Put)
        {
            request.AddHeader("Content-Type", "application/json");
        }

        return request;
    }

    public static object CreatePostBody()
    {
        return new { title = "foo", body = "bar", userId = 1 };
    }

    public static object CreatePatchBody()
    {
        return new { title = "updated title" };
    }
}