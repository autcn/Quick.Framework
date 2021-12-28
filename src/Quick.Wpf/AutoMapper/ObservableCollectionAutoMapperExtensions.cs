using AutoMapper;
using System.Collections.ObjectModel;

namespace Quick
{
    public static class ObservableCollectionAutoMapperExtensions
    {
        public static ObservableCollection<TItem> MapToObservableCollection<TItem>(this IMapper mapper, System.Collections.IEnumerable sources)
        {
            return mapper.Map<ObservableCollection<TItem>>(sources);
        }
    }
}
