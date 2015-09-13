namespace FindWhere.Web.Infrastructure.Filters
{
    using FindWhere.Data;
    using System.Web.Mvc;

    public class LogAttribute : ActionFilterAttribute
    {
        // private FindWhereDbContext context = FindWhereDbContext.Create();

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // TODO
            // this.context.Logs.Add(new Log { Message = filterContext.HttpContext.Request.RawUrl });
            // context.SaveChanges();

            base.OnActionExecuted(filterContext);
        }
    }
}