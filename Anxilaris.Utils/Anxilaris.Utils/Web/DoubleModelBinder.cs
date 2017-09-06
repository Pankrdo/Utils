namespace Anxilaris.Utils.Web.Mvc
{   
    using System.Globalization;
    using System.Web.Mvc;
    public class DoubleModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result != null && !string.IsNullOrEmpty(result.AttemptedValue))
            {
                if (bindingContext.ModelType == typeof(double))
                {
                    double resultingValue;
                    var newFormattedValue = result.AttemptedValue.Replace(",", ".");
                    if (double.TryParse(
                        newFormattedValue,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out resultingValue)
                    )
                    {
                        return resultingValue;
                    }
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
