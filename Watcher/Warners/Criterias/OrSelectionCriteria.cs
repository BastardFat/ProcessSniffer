using System.Linq;
using Watcher.Models.Enums.Criterias;

namespace Watcher.Warners.Criterias
{
    public class OrSelectionCriteria : SelectionCriteria
    {
        public OrSelectionCriteria(params SelectionCriteria[] criterias)
        {
            Criterias = criterias;
            CriteriaType = SelectionCriteriaType.Or;
        }

        public SelectionCriteria[] Criterias;

        public override bool CheckCriteria(Models.Process _model) =>
            Criterias.Any((crt) => crt.CheckCriteria(_model));

        public override string ToString() =>
            "OR(" + Criterias.Aggregate("", (workingSentence, next) => workingSentence + ((workingSentence == "") ? "" : ", ") + next.ToString()) + ")";

        public override string ToJson() =>
            $"{{\"{nameof(CriteriaType)}\":{(int) CriteriaType},\"{nameof(Criterias)}\":[" +
            Criterias.Aggregate("", (workingSentence, next) => workingSentence + ((workingSentence == "") ? "" : ", ") + next.ToJson()) +
            "]}";
    }
}
