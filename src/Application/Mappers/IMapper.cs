namespace LaunchQ.TakeHomeProject.Application.Mappers
{
    /// <summary>
    /// Interface for mapping between DTOs and domain entities
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    public interface IMapper<TSource, TDestination>
    {
        /// <summary>
        /// Maps a source object to a destination object
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Destination object</returns>
        TDestination Map(TSource source);
        
        /// <summary>
        /// Maps a source object to an existing destination object
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Existing destination object</param>
        /// <returns>Updated destination object</returns>
        TDestination Map(TSource source, TDestination destination);
    }
}
