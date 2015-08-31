namespace FindWhere.Web.App_Start
{
    using System.Web.Mvc;

    public class ViewEngineConfig
    {
        public static void RegisterViewEngines(ViewEngineCollection engines)
        {
            engines.Clear();

            engines.Add(new RazorViewEngine());
        }
    }
}