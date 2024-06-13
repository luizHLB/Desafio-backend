namespace Product.Domain.Helpers
{
    public static class EnumHelper<T> where T : struct, IConvertible
    {
        public static int GetValue(IEnumerable<T> inputAttachments) =>
            inputAttachments.Sum(item => Convert.ToInt32(item));

        public static object GetEnums(int input, bool getEnumName)
        {
            if (getEnumName)
                return GetEnumNames(input);

            return GetEnums(input);
        }

        public static IEnumerable<T> GetEnums(int input)
        {
            var listToResponse = new List<T>();

            var items = (IEnumerable<T>)System.Enum.GetValues(typeof(T));

            foreach (var eInputAttachment in items.OrderByDescending(item => item).AsEnumerable().Where(eInput => Convert.ToInt32(eInput) <= input))
            {
                listToResponse.Add(eInputAttachment);
                input -= Convert.ToInt32(eInputAttachment);
            }

            return listToResponse;
        }

        public static IEnumerable<string> GetEnumNames(int input)
        {
            var listToResponse = new List<string>();

            var items = (IEnumerable<T>)System.Enum.GetValues(typeof(T));

            foreach (var eInputAttachment in items.OrderByDescending(item => item).AsEnumerable().Where(eInput => Convert.ToInt32(eInput) <= input))
            {
                listToResponse.Add(eInputAttachment.ToString());
                input -= Convert.ToInt32(eInputAttachment);
            }

            return listToResponse;
        }

        public static IList<T> GetEnumList() => (IList<T>)System.Enum.GetValues(typeof(T));
    }
}
