using System.Linq;
using Watcher.Models.Enums.Criterias;

namespace Watcher.Warners.Criterias
{
    public class AndSelectionCriteria : SelectionCriteria
    {
        public AndSelectionCriteria(params SelectionCriteria[] criterias)
        {
            Criterias = criterias;
            CriteriaType = SelectionCriteriaType.And;
        }

        public SelectionCriteria[] Criterias;

        public override bool CheckCriteria(Models.Process _model) => Criterias.All((crt) => crt.CheckCriteria(_model));

        public override string ToString() =>
            "AND(" + Criterias.Aggregate("", (workingSentence, next) => workingSentence + ((workingSentence == "") ? "" : ", ") + next.ToString()) + ")";

        public override string ToJson() =>
            $"{{\"{nameof(CriteriaType)}\":{(int) CriteriaType},\"{nameof(Criterias)}\":[" +
            Criterias.Aggregate("", (workingSentence, next) => workingSentence + ((workingSentence == "") ? "" : ", ") + next.ToJson()) +
            "]}";
    }
}
