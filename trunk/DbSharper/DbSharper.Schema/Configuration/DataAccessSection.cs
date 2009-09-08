namespace DbSharper.Schema.Configuration
{
    using System.Text.RegularExpressions;

    using DbSharper.Schema.Collections;

	internal class DataAccessSection : SectionBase
    {
        #region Fields

        private Regex regexClassMethod;

        #endregion Fields

        #region Constructors

        public DataAccessSection(string methodMask)
            : base()
        {
            this.regexClassMethod = new Regex(methodMask, RegexOptions.Compiled);
        }

        #endregion Constructors

        #region Methods

        internal ClassMethod GetClassMethod(ISchema procedure)
        {
            string input = TrimName(procedure);

            Match match = this.regexClassMethod.Match(input);

            switch (match.Groups.Count)
            {
                case 2:
                    {
                        return new ClassMethod()
                        {
                            ClassName = "_Global",
                            MethodName = MappingHelper.GetPascalCase(match.Groups["Method"].Value)
                        };
                    }
                case 3:
                    {
                        return new ClassMethod()
                        {
                            ClassName = MappingHelper.GetPascalCase(match.Groups["Class"].Value),
                            MethodName = MappingHelper.GetPascalCase(match.Groups["Method"].Value)
                        };
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        #endregion Methods

        #region Nested Types

        internal class ClassMethod
        {
            #region Properties

            public string ClassName
            {
                get; set;
            }

            public string MethodName
            {
                get; set;
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}