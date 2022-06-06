using ZvonimirSkorin_rwa.DAL;

namespace ZvonimirSkorin_rwa.Middleware
{
    public class Auth
    {

       
        public class MyMiddleware
        {
            private readonly RequestDelegate _next;


            public MyMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            

            public async Task Invoke(HttpContext context)
            {
                string path = context.Request.Path;
                string [] parts = path.Split("/");
                if (path != "/" && root!="api")
                {
                    if (context.Request.Cookies["Token"] == null)
                    {
                        context.Response.Redirect("/");
                        return;
                    }
                    else
                    {

                        if (!AuthRepo.validateToken(context.Request.Cookies["Token"]))
                        {

                            context.Response.Cookies.Append("token", null,
                                new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });

                        }
                       
                    }
                }
                await _next(context);
            }
        }
    }
}
