using System.Linq;
using System.Net;
using AutoRest.Core;
using AutoRest.Core.Model;

namespace AutoRest.TypeScript.SuperAgent.ModelBinder
{
    public class ModelBinderBaseTs
    {
        protected bool TryGetResponseName(Method method, out IModelType modelType, out string responseName,
            out string requestName, string moduleName = null)
        {
            responseName = null;
            requestName = null;
            modelType = null;

            var okResponse = method.Responses.ContainsKey(HttpStatusCode.OK)
                ? method.Responses[HttpStatusCode.OK]
                : method.Responses.Values.FirstOrDefault(r => r.Body != null);

            if (okResponse == null)
            {
                return false;
            }

            modelType = okResponse.Body;

            responseName = GetTypeText(modelType, moduleName);

            if (requestName == null)
            {
                requestName = $"{method.Name.RawValue}Request";
            }

            requestName = $"I{CodeNamer.Instance.PascalCase(requestName)}";

            if (!string.IsNullOrWhiteSpace(moduleName) && !requestName.StartsWith(moduleName))
            {
                requestName = $"{moduleName}.{requestName}";
            }

            if (responseName == null)
            {
                return false;
            }
            return true;
        }

        public string GetTypeText(IModelType modelType, string moduleName = null)
        {
            var seqType = modelType as SequenceTypeTs;

            var prefix = string.IsNullOrWhiteSpace(moduleName) ? "" : $"{moduleName}.";

            var name = "";

            if (seqType == null)
            {
                if (modelType == null)
                {
                    return null;
                }
                name = modelType.GetImplementationName();

                if (modelType.IsEnumType())
                {
                    return name;
                }
                if (modelType.IsDictionaryType())
                {
                    return name;
                }

                return modelType.IsPrimaryType() ? name : $"{prefix}I{name}";
            }

            var elementType = seqType.ElementType;
            name = elementType.GetImplementationName();

            return SequenceTypeTs.CreateSeqTypeText(elementType.IsPrimaryType() ? name : $"{prefix}I{name}");
        }
    }
}