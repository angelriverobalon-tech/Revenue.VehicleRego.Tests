using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Revenue.Tests.VehicleRego.BDD.Model.Pages
{
    public abstract class BasePage<T> where T : BasePage<T>
    {
        protected IPage? page;

        protected BasePage()
        {
        }

        protected BasePage(IPage page)
        {
            this.page = page;
        }

        public virtual async Task<T> Init()
        {
            return (T)await Task.FromResult<object>(this);
        }
    }
}
