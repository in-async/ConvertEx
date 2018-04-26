using System;
using System.Collections.Generic;
using System.Linq;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// 複数の <see cref="ITryParseProvider"/> を管理する <see cref="ITryParseProvider"/> クラス。
    /// </summary>
    public class CompositeTryParseProvider : TryParseProvider {

        public CompositeTryParseProvider() : this(Enumerable.Empty<ITryParseProvider>()) {
        }

        public CompositeTryParseProvider(IEnumerable<ITryParseProvider> providers) {
            if (providers == null) throw new ArgumentNullException(nameof(providers));

            Providers = providers.ToList();
        }

        /// <summary>
        /// <see cref="ITryParseProvider"/> のリスト。
        /// </summary>
        public IReadOnlyList<ITryParseProvider> Providers { get; }

        public override TryParseDelegate<T> GetDelegate<T>(IFormatProvider provider) {
            for (var i = 0; i < Providers.Count; i++) {
                var tryParse = Providers[i].GetDelegate<T>(provider);
                if (tryParse != null) {
                    return tryParse;
                }
            }
            return null;
        }

        public override TryParseDelegate<object> GetDelegate(Type conversionType, IFormatProvider provider) {
            for (var i = 0; i < Providers.Count; i++) {
                var tryParse = Providers[i].GetDelegate(conversionType, provider);
                if (tryParse != null) {
                    return tryParse;
                }
            }
            return null;
        }
    }
}