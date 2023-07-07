namespace Code.Challenge.Application.Internal
{
    /// <summary>
    /// Shared Logic Contract for All Query/Command Services.
    /// </summary>
    internal interface ISharedLogic
    {
        /// <summary>
        /// Find a cliente position given the priority evaluation criteria
        /// </summary>
        /// <param name="personId">The Person Id.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task<int>"/> reference after the operation has completed.</returns>
        Task<int> FindClientPosition(long personId, CancellationToken cancellationToken);
    }
}