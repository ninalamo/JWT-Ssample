using Microsoft.AspNetCore.Authorization;

public class AuthorizeJwtAttribute : AuthorizeAttribute
{
    //protected override void OnAuthorization(AuthorizationContext context)
    //{
    //    base.OnAuthorization(context);

    //    if (Authorized)
    //    {
    //        return;
    //    }

    //    context.HandleFailure();
    //}
}