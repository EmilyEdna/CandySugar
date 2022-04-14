using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Ioc;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Enum;
using System.Threading.Tasks;

namespace CandySugar.App.Controls
{
    public class ViewModelRepository : BindableBase
    {
        protected IContainerProvider Container => ContainerLocator.Container;
        public ViewModelRepository()
        {

        }

        protected virtual T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        protected virtual async Task<List<CandyHistoryDto>> OnInitAutoKey(string input, CheckFuncType type, bool IsInsertAndQuery = false)
        {
            return await Resolve<IHistory>().InsertAndQuery(new CandyHistoryDto
            {
                CheckType = type,
                CheckText = input
            }, IsInsertAndQuery);
        }

        protected virtual void OnClearKey(CheckFuncType type)
        {
            Resolve<IHistory>().Clear(new CandyHistoryDto
            {
                CheckType = type
            });
        }

        protected virtual void OnRemoveKey(Guid PId)
        {
            Resolve<IHistory>().Remove(new CandyHistoryDto { PId = PId });
        }
    }
}
