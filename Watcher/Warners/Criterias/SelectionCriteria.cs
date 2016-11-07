using Newtonsoft.Json;
using System;
using System.Linq;
using Watcher.Models.Enums.Criterias;

namespace Watcher.Warners.Criterias
{
    public abstract class SelectionCriteria
    {


        abstract public bool CheckCriteria(Models.Process _model);

        abstract public string ToJson();

        protected SelectionCriteriaType CriteriaType;




        private class SelectionCriteriaModel
        {
            public SelectionCriteriaMode Mode { get; set; }
            public string CheckWith { get; set; }
            public ProcessFields Field { get; set; }
            public SelectionCriteriaModel Criteria { get; set; }
            public SelectionCriteriaModel[] Criterias { get; set; }
            public SelectionCriteriaType CriteriaType { get; set; }
        }

        public static SelectionCriteria CreateFromJson(string json) =>
            CreateFromModel(JsonConvert.DeserializeObject<SelectionCriteriaModel>(json));


        private static SelectionCriteria CreateFromModel(SelectionCriteriaModel model)
        {
            switch (model.CriteriaType)
            {
                case SelectionCriteriaType.Final:
                    return new ProcessSelectionCriteria(model.Field, model.Mode, model.CheckWith);
                case SelectionCriteriaType.Or:
                    return new OrSelectionCriteria(model.Criterias.Select((m) => CreateFromModel(m)).ToArray());
                case SelectionCriteriaType.And:
                    return new AndSelectionCriteria(model.Criterias.Select((m) => CreateFromModel(m)).ToArray());
                case SelectionCriteriaType.Not:
                    return new NotSelectionCriteria(CreateFromModel(model.Criteria));
                default:
                    throw new Exception();
            }
        }
    }
}
