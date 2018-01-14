namespace AI.Behavior
{
    public class AIModel
    {
        // NOTE: 行動
        public enum Behavior
        {
            Attack,
            Escape,
            Chase,
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

        public Behavior GetBehavior { get { return behavior; } }
        
        public AIModel()
        {
            subject = Subject.Enemy;
            criterion = Criterion.Hp;
            from = 0;
            to = 100;
            behavior = Behavior.Chase;
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
