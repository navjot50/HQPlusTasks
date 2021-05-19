using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HQPlus.Task3.Api.Infrastructure {
    public sealed class ISODateBinder : IModelBinder {
        
        private static readonly string ISODateFormat = "yyyy-MM-dd";
        
        public Task BindModelAsync(ModelBindingContext bindingContext) {
            if (bindingContext == null) {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var strVal = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (string.IsNullOrEmpty(strVal)) {
                bindingContext.Result = ModelBindingResult.Failed();
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "The date is not provided");
                return Task.CompletedTask;
            }

            var parseResult = DateTime.TryParseExact(strVal, ISODateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);
            if (parseResult) {
                bindingContext.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Failed();
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "The date is not in yyyy-MM-dd format");
            return Task.CompletedTask;
        }
    }
}