using AutoRest.Core.Model;

namespace AutoRest.TypeScript.SuperAgent
{
    public class DictionaryTypeTs : DictionaryType
    {
        protected DictionaryTypeTs()
        {
            Name.OnGet += value => $"{{ [id: string]: {ValueType.GetImplementationName()} }}";
        }
       
    }
}
