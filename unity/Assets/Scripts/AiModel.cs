namespace AI.Behavior
{
    public class AIModel
    {
        // NOTE: 行動
        public enum Behavior
        {
            Attack,
            Escape,
            Skill,
            None,
        }

        public enum Subject
        {
            Enemy,
            Player,
        }

        public enum Criterion
        {
            Hp,
            Mp,
        }

        private Subject subject;
        private Criterion criterion;
        private float from;
        private float to;
        private Behavior behavior;
        
        public AIModel()
        {
            this.subject = Subject.Enemy;
            this.criterion = Criterion.Hp;
            this.from = 0;
            this.to = 100;
            this.behavior = Behavior.None;
        }

        public AIModel(Subject subject, Criterion criterion, float from, float to, Behavior behavior)
        {
            this.subject = subject;
            this.criterion = criterion;
            this.from = from;
            this.to = to;
            this.behavior = behavior;
        }
    }
}
