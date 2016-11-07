using System;
using Watcher.Models.Enums.Criterias;
using Watcher.Tools;

namespace Watcher.Warners.Criterias
{
    public class ProcessSelectionCriteria : SelectionCriteria
    {
        public ProcessSelectionCriteria(ProcessFields field, SelectionCriteriaMode mode, string checkWith)
        {
            Mode = mode;
            Field = field;
            CheckWith = checkWith;
            CriteriaType = SelectionCriteriaType.Final;
        }
        public SelectionCriteriaMode Mode { get; }
        public string CheckWith { get; }
        public ProcessFields Field { get; }

        public override bool CheckCriteria(Models.Process _model)
        {
            string field = (_model.GetType().GetProperty(Field.ToString()).GetValue(_model, null) as string) ?? "";

            switch (Mode)
            {
                case SelectionCriteriaMode.Equal:
                    return field.Equals(CheckWith, StringComparison.OrdinalIgnoreCase);
                case SelectionCriteriaMode.StartWith:
                    return field.StartsWith(CheckWith, StringComparison.OrdinalIgnoreCase);
                case SelectionCriteriaMode.Contains:
                    return field.Contains(CheckWith);
                default:
                    throw new ArgumentException("Selection criteria mode is not valid");
            }
        }

        public override string ToString() =>
            $"{Field.ToString()}.{Mode.ToString()}(\"{StringHelper.Escape(CheckWith)}\")";

        public override string ToJson() =>
            $"{{\"{nameof(CriteriaType)}\":{(int) CriteriaType},\"{nameof(Mode)}\":{(int) Mode},\"{nameof(CheckWith)}\":\"{StringHelper.Escape(CheckWith)}\",\"{nameof(Field)}\":{(int) Field}}}";

    }

}
