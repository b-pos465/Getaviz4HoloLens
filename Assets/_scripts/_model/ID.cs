namespace Model
{
    public class ID
    {
        public string Value { get; private set; }

        private ID()
        { }

        public static ID From(string value)
        {
            return new ID
            {
                Value = value
            };
        }

        public override string ToString() => this.Value;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            ID id = (ID)obj;
            return id.Value == this.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}