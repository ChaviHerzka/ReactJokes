namespace ReactJokes.Web.Controllers
{
    public enum UserJokeInteractionStatus
    {
        Unauthenticated,
        Liked,
        Disliked,
        NeverInteracted,
        CanNoLongerInteract
    }
}
