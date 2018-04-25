namespace InAsync.Tests.TestHelpers.Models {

    public class VanillaClass {

        #region for Assert.Equals()

        public override bool Equals(object obj) {
            var @class = obj as TypeConvertableClass;
            return @class != null;
        }

        public override int GetHashCode() {
            return -1937169414;
        }

        #endregion for Assert.Equals()
    }
}