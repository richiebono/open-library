namespace LaunchQ.TakeHomeProject.Application.Mappers
{
    /// <summary>
    /// Interface para o mapeamento entre DTOs e entidades de domínio
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem</typeparam>
    /// <typeparam name="TDestination">Tipo de destino</typeparam>
    public interface IMapper<TSource, TDestination>
    {
        /// <summary>
        /// Mapeia um objeto de origem para um objeto de destino
        /// </summary>
        /// <param name="source">Objeto de origem</param>
        /// <returns>Objeto de destino</returns>
        TDestination Map(TSource source);
        
        /// <summary>
        /// Mapeia um objeto de origem para um objeto de destino existente
        /// </summary>
        /// <param name="source">Objeto de origem</param>
        /// <param name="destination">Objeto de destino existente</param>
        /// <returns>Objeto de destino atualizado</returns>
        TDestination Map(TSource source, TDestination destination);
    }
}
