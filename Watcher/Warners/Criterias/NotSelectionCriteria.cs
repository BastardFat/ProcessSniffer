using Watcher.Models.Enums.Criterias;

namespace Watcher.Warners.Criterias
{
    public class NotSelectionCriteria : SelectionCriteria
    {
        public NotSelectionCriteria(SelectionCriteria criteria)
        {
            Criteria = criteria;
            CriteriaType = SelectionCriteriaType.Not;
        }

        public SelectionCriteria Criteria;
        public override bool CheckCriteria(Models.Process _model) => !Criteria.CheckCriteria(_model);
        public override string ToString() => $"NOT({Criteria.ToString()})";
        public override string ToJson() => $"{{\"{nameof(CriteriaType)}\":{(int) CriteriaType},\"{nameof(Criteria)}\":{Criteria.ToJson()}}}";

    }
}
